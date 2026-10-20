param([string]$BaseUrl = 'http://localhost:53170')
. "$PSScriptRoot/common.ps1"
function Wait-Run([string]$Id) {
    $deadline = (Get-Date).AddSeconds(45)
    do {
        $run = Invoke-RestMethod "$BaseUrl/api/runs/$Id"
        if ($run.status -in @('succeeded','failed','interrupted')) { return $run }
        Start-Sleep -Milliseconds 250
    } while ((Get-Date) -lt $deadline)
    throw "Délai dépassé pour $Id"
}
function Start-Run([string]$Body) {
    return (Invoke-RestMethod "$BaseUrl/api/runs" -Method Post -ContentType 'application/json' -Body $Body).id
}
$settings = Invoke-RestMethod "$BaseUrl/api/schedule"
if ($settings.enabled) { throw 'Désactiver la planification avant la répétition.' }
$id = Start-Run '{"source":"fixture","qualityScenario":"complete"}'
if ((Wait-Run $id).status -ne 'succeeded') { throw 'Le traitement complet a échoué.' }
$report = Invoke-RestMethod "$BaseUrl/api/report"
if ($report.summary.observationCount -ne 168 -or $report.series.Count -ne 4) { throw 'Contrat métier invalide.' }
if (@($report.series | Where-Object { $_.coverage.percent -ne 100 }).Count) { throw 'Couverture complète attendue.' }
$id = Start-Run '{"source":"fixture","qualityScenario":"missing"}'
if ((Wait-Run $id).status -ne 'succeeded') { throw 'Le scénario manquant a échoué.' }
$report = Invoke-RestMethod "$BaseUrl/api/report"
$water = $report.series | Where-Object id -eq 'water_level'
if ($water.coverage.percent -ne 95.83) { throw 'Couverture 23/24 attendue.' }
$id = Start-Run '{"source":"fixture","simulateFailure":true}'
if ((Wait-Run $id).status -ne 'failed') { throw 'Échec simulé attendu.' }
$retry = Invoke-RestMethod "$BaseUrl/api/runs/$id/retry" -Method Post
if ((Wait-Run $retry.id).status -ne 'succeeded') { throw 'Relance échouée.' }
foreach ($file in @('observations.csv','observations.parquet','report.json','hourly.csv')) {
    $response = Invoke-WebRequest "$BaseUrl/api/artifacts/$($retry.id)/$file" -UseBasicParsing
    if ($response.StatusCode -ne 200 -or $response.RawContentLength -lt 100) { throw "Export invalide : $file" }
}
Write-Host 'HTTP OK : 168 observations, 4 séries, couverture, échec, relance, CSV/Parquet/JSON/horaire.'
