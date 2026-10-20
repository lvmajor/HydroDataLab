param([switch]$SkipPresentation, [ValidateSet('python','dotnet')][string]$EtlEngine)
. "$PSScriptRoot/common.ps1"
$EtlEngine = Resolve-EtlEngine $EtlEngine
Write-Host "Préparation du moteur ETL : $EtlEngine"
if ($EtlEngine -eq 'python') { Invoke-Checked uv @('sync','--project','etl','--frozen') }
else {
    Invoke-Checked dotnet @('restore','tests/Etl.Tests','--locked-mode')
    Invoke-Checked dotnet @('build','src/Etl','--no-restore','--verbosity','quiet')
}
Invoke-Checked dotnet @('restore','tests/Api.Tests','--locked-mode')
Invoke-Checked dotnet @('build','src/Api','--no-restore','--verbosity','quiet')
Invoke-Checked npm.cmd @('ci','--prefix','web')
Invoke-Checked npm.cmd @('run','build','--prefix','web')
Invoke-Checked aspire @('restore','--apphost',$AppHost,'--non-interactive')
if (!$SkipPresentation) {
    $previousDownload = $env:PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD
    try {
        $env:PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD = '1'
        Invoke-Checked npm.cmd @('ci','--prefix','presentation')
    } finally { $env:PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD = $previousDownload }
    Invoke-Checked npm.cmd @('run','build','--prefix','presentation')
}
Write-Host 'Préparation terminée. Les données de démonstration sont déjà dans data/fixtures.'
Write-Host "Lancer scripts/verify.ps1 -EtlEngine $EtlEngine, puis scripts/start-demo.ps1 -EtlEngine $EtlEngine."
