using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Quartz;
namespace HydroDataLab.Api;

public sealed class PipelineCoordinator(RunStore store, ISchedulerFactory schedulers, ILogger<PipelineCoordinator> logger)
{
    private readonly SemaphoreSlim scheduleLock = new(1, 1);
    private static readonly JobKey PeriodicJobKey = new("periodic-pipeline");
    private static readonly TriggerKey PeriodicTriggerKey = new("periodic-pipeline-trigger");
    public async Task<string?> QueueAsync(RunRequest request, string? retryOf = null)
    {
        if (!request.IsValid) throw new ArgumentException("Invalid run request.");
        var id = store.TryCreate(request, retryOf);
        if (id is null) return null;
        try
        {
            var scheduler = await schedulers.GetScheduler();
            var job = JobBuilder.Create<PipelineJob>().WithIdentity(id)
                .UsingJobData("runId", id).UsingJobData("source", request.Source)
                .UsingJobData("simulateFailure", request.SimulateFailure ? "true" : "false")
                .UsingJobData("qualityScenario", request.QualityScenario).Build();
            await scheduler.ScheduleJob(job, TriggerBuilder.Create().WithIdentity("run-" + id).StartNow().Build());
            logger.LogInformation("Exécution {RunId} placée dans Quartz ({Source})", id, request.Source);
            return id;
        }
        catch (Exception exception) { store.Complete(id, null, "Impossible de planifier : " + exception.Message); throw; }
    }
    public async Task RestoreScheduleAsync() => await ApplyScheduleAsync(store.ReadSchedule());
    public async Task SetScheduleAsync(ScheduleSettings settings)
    {
        await scheduleLock.WaitAsync();
        try { await ApplyScheduleAsync(settings); store.WriteSchedule(settings); }
        finally { scheduleLock.Release(); }
    }
    private async Task ApplyScheduleAsync(ScheduleSettings settings)
    {
        var scheduler = await schedulers.GetScheduler();
        if (await scheduler.GetJobDetail(PeriodicJobKey) is not null) await scheduler.DeleteJob(PeriodicJobKey);
        if (!settings.Enabled) return;
        var job = JobBuilder.Create<PeriodicPipelineJob>().WithIdentity(PeriodicJobKey).Build();
        var trigger = TriggerBuilder.Create().WithIdentity(PeriodicTriggerKey)
            .StartAt(DateTimeOffset.UtcNow.AddMinutes(settings.IntervalMinutes))
            .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(settings.IntervalMinutes)).RepeatForever().WithMisfireInstruction(SimpleTriggerMisfireInstruction.NextWithRemainingCount)).Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}
public sealed class PeriodicPipelineJob(PipelineCoordinator coordinator) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
        => await coordinator.QueueAsync(new RunRequest());
}
public sealed class PipelineJob(RunStore store, IHttpClientFactory clients, ILogger<PipelineJob> logger) : IJob
{
    public const string ActivitySourceName = "HydroDataLab.Pipeline";
    private static readonly ActivitySource Activities = new(ActivitySourceName);
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        var id = context.MergedJobDataMap.GetString("runId")!;
        if (!store.TryStart(id)) return;
        using var activity = Activities.StartActivity("ETL HydroDataLab");
        activity?.SetTag("hydro.run_id", id);
        using var scope = logger.BeginScope(new Dictionary<string, object> { ["RunId"] = id });
        string? execution = null;
        try
        {
            var body = new { runId = id, source = context.MergedJobDataMap.GetString("source"),
                simulateFailure = context.MergedJobDataMap.GetString("simulateFailure") == "true",
                qualityScenario = context.MergedJobDataMap.GetString("qualityScenario") };
            logger.LogInformation("Extraction, transformation et export ETL pour {RunId}", id);
            using var response = await clients.CreateClient("etl").PostAsJsonAsync("run", body, cancellationToken);
            var report = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var message = $"ETL HTTP {(int)response.StatusCode}";
                try
                {
                    using var failure = JsonDocument.Parse(report);
                    if (failure.RootElement.TryGetProperty("detail", out var detail))
                    {
                        if (detail.ValueKind == JsonValueKind.Object)
                        {
                            if (detail.TryGetProperty("execution", out var steps)) execution = steps.GetRawText();
                            if (detail.TryGetProperty("message", out var reason)) message += " : " + reason.GetString();
                        }
                        else if (detail.ValueKind == JsonValueKind.String) message += " : " + detail.GetString();
                    }
                }
                catch (JsonException) { /* Une réponse proxy non JSON reste une erreur HTTP lisible. */ }
                throw new HttpRequestException(message);
            }
            using var document = JsonDocument.Parse(report);
            if (document.RootElement.GetProperty("runId").GetString() != id) throw new InvalidDataException("Identifiant incohérent dans le rapport ETL.");
            store.Complete(id, report, null);
            logger.LogInformation("Exécution {RunId} réussie et rapport conservé", id);
        }
        catch (OperationCanceledException exception)
        {
            store.Complete(id, null, "Traitement interrompu ou délai dépassé : " + exception.Message, interrupted: true);
            activity?.SetStatus(ActivityStatusCode.Error, "interrupted");
            throw new JobExecutionException(exception) { RefireImmediately = false };
        }
        catch (Exception exception)
        {
            store.Complete(id, null, exception.Message, execution: execution);
            activity?.SetStatus(ActivityStatusCode.Error, exception.Message);
            logger.LogError(exception, "Échec du traitement {RunId}", id);
            throw new JobExecutionException(exception) { RefireImmediately = false };
        }
    }
}
