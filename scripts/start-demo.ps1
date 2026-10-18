param([switch]$NoBuild)
. "$PSScriptRoot/common.ps1"
$branch = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" symbolic-ref --short -q HEAD
if (!$branch) { $branch = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" describe --tags --exact-match HEAD 2>$null }
if (!$branch) { $branch = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" rev-parse --short HEAD }
if (!$branch) { throw 'Impossible de déterminer le jalon Git pour isoler son historique.' }
$runtimeKey = $branch -replace '[^A-Za-z0-9_-]', '-'
$env:HYDRO_RUNTIME_DIR = Join-Path $RepoRoot ".runtime/$runtimeKey"
Write-Host "Historique du jalon $branch : $env:HYDRO_RUNTIME_DIR"
$options = @('start','--apphost',$AppHost,'--launch-profile','http','--non-interactive')
if ($NoBuild) { $options += '--no-build' }
Invoke-Checked aspire $options
Invoke-Checked aspire @('wait','web','--status','healthy','--timeout','60','--apphost',$AppHost)
Write-Host 'HydroDataLab : http://localhost:53170 — API : http://localhost:53171'
Write-Host 'Ouvrir aussi le lien du tableau de bord Aspire affiché ci-dessus.'
