$ErrorActionPreference = 'Stop'
$script:RepoRoot = Split-Path -Parent $PSScriptRoot
Set-Location -LiteralPath $script:RepoRoot
$script:AppHost = Join-Path $script:RepoRoot 'src/AppHost/apphost.cs'
function Resolve-EtlEngine {
    param([string]$EtlEngine)
    if ([string]::IsNullOrWhiteSpace($EtlEngine)) { $EtlEngine = $env:Hydro__EtlEngine }
    if ([string]::IsNullOrWhiteSpace($EtlEngine)) { $EtlEngine = 'python' }
    $EtlEngine = $EtlEngine.Trim().ToLowerInvariant()
    if ($EtlEngine -notin @('python','dotnet')) { throw 'EtlEngine / Hydro__EtlEngine doit être python ou dotnet.' }
    return $EtlEngine
}
function Invoke-Checked {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Command a échoué (code $LASTEXITCODE)." }
}
function Invoke-RepoGit {
    param([string[]]$Arguments)
    Invoke-Checked git (@('-c', "safe.directory=$($script:RepoRoot.Replace('\','/'))") + $Arguments)
}
