$ErrorActionPreference = 'Stop'
$script:RepoRoot = Split-Path -Parent $PSScriptRoot
Set-Location -LiteralPath $script:RepoRoot
$script:AppHost = Join-Path $script:RepoRoot 'src/AppHost/apphost.cs'
function Invoke-Checked {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$Command a échoué (code $LASTEXITCODE)." }
}
function Invoke-RepoGit {
    param([string[]]$Arguments)
    Invoke-Checked git (@('-c', "safe.directory=$($script:RepoRoot.Replace('\','/'))") + $Arguments)
}
