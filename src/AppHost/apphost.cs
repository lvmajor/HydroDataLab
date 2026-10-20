#:package Aspire.Hosting.Docker@13.6.1
#:package Aspire.Hosting.JavaScript@13.6.1
#:package Aspire.Hosting.Python@13.6.1
#:package MessagePack@2.5.307
#:sdk Aspire.AppHost.Sdk@13.6.1
#:property AspireUseCliBundle=true

var builder = DistributedApplication.CreateBuilder(args);
var root = Path.GetFullPath("../..", builder.AppHostDirectory);
var etlEngine = (builder.Configuration["Hydro:EtlEngine"] ?? "python").Trim().ToLowerInvariant();
if (etlEngine is not ("python" or "dotnet"))
    throw new InvalidOperationException("Hydro:EtlEngine doit être 'python' ou 'dotnet' (variable Hydro__EtlEngine).");
builder.AddDockerComposeEnvironment("compose")
    .WithDashboard(dashboard => dashboard
        .WithImage("mcr.microsoft.com/dotnet/aspire-dashboard", "13.6.1")
        .WithEnvironment("ASPIRE_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS", "true")
        .WithEnvironment("Dashboard__Otlp__AuthMode", "Unsecured")
        .PublishAsDockerComposeService((resource, service) =>
        {
            service.Ports.Clear();
            service.Ports.Add("127.0.0.1:53174:18888");
        }));

if (builder.ExecutionContext.IsPublishMode)
{
    var etl = builder.AddDockerfile(etlEngine == "dotnet" ? "etl-dotnet" : "python", root,
            etlEngine == "dotnet" ? "src/Etl/Dockerfile" : "etl/Dockerfile")
        .WithHttpEndpoint(targetPort: etlEngine == "dotnet" ? 8080 : 8001)
        .WithVolume("hydrolab-exports", "/app/outputs")
        .WithHttpHealthCheck("/health");
    var api = builder.AddDockerfile("api", root, "src/Api/Dockerfile")
        .WithHttpEndpoint(targetPort: 8080)
        .WithVolume("hydrolab-state", "/state")
        .WithEnvironment("Etl__BaseUrl", etl.GetEndpoint("http"))
        .WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://compose-dashboard:18889")
        .WithEnvironment("OTEL_EXPORTER_OTLP_PROTOCOL", "grpc")
        .WithEnvironment("OTEL_SERVICE_NAME", "hydrodatalab-api")
        .WithHttpHealthCheck("/health")
        .WaitFor(etl);
    builder.AddDockerfile("web", Path.Combine(root, "web"))
        .WithHttpEndpoint(port: 53170, targetPort: 80)
        .WithExternalHttpEndpoints()
        .PublishAsDockerComposeService((resource, service) =>
        {
            service.Ports.Clear();
            service.Ports.Add("127.0.0.1:53170:80");
        })
        .WaitFor(api);
}
else
{
    var runtime = Environment.GetEnvironmentVariable("HYDRO_RUNTIME_DIR") ?? Path.Combine(root, ".runtime", "main");
    if (etlEngine == "dotnet") runtime = Path.Combine(runtime, "dotnet");
    Directory.CreateDirectory(runtime);
    IResourceBuilder<IResource> etlResource;
    EndpointReference etlEndpoint;
    if (etlEngine == "dotnet")
    {
        var etl = builder.AddProject("etl-dotnet", Path.Combine(root, "src", "Etl", "HydroDataLab.Etl.csproj"), launchProfileName: null)
            .WithHttpEndpoint()
            .WithEnvironment("HYDRO_FIXTURE_DIR", Path.Combine(root, "data", "fixtures"))
            .WithEnvironment("HYDRO_OUTPUT_DIR", Path.Combine(runtime, "exports"))
            .WithHttpHealthCheck("/health");
        etlResource = etl;
        etlEndpoint = etl.GetEndpoint("http");
    }
    else
    {
        var etl = builder.AddUvicornApp("python", Path.Combine(root, "etl"), "hydrodatalab.api:app")
            .WithUv(install: false)
            .WithEnvironment("HYDRO_FIXTURE_DIR", Path.Combine(root, "data", "fixtures"))
            .WithEnvironment("HYDRO_OUTPUT_DIR", Path.Combine(runtime, "exports"))
            .WithHttpHealthCheck("/health");
        etlResource = etl;
        etlEndpoint = etl.GetEndpoint("http");
    }
    var api = builder.AddProject("api", Path.Combine(root, "src", "Api", "HydroDataLab.Api.csproj"), launchProfileName: null)
        .WithHttpEndpoint()
        .WithEnvironment("Etl__BaseUrl", etlEndpoint)
        .WithEnvironment("Hydro__DataDir", Path.Combine(runtime, "api"))
        .WithHttpHealthCheck("/health")
        .WaitFor(etlResource);
    builder.AddViteApp("web", Path.Combine(root, "web"))
        .WithNpm(install: false)
        .WithEndpoint("http", endpoint => endpoint.Port = 53170)
        .WithEnvironment("API_BASE_URL", api.GetEndpoint("http"))
        .WaitFor(api);
}

builder.Build().Run();
