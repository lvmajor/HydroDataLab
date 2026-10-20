param([switch]$NoBuild, [ValidateSet('python','dotnet')][string]$EtlEngine)
. "$PSScriptRoot/common.ps1"
$EtlEngine = Resolve-EtlEngine $EtlEngine
$branch = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" symbolic-ref --short -q HEAD
if (!$branch) { $branch = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" describe --tags --exact-match HEAD 2>$null }
if (!$branch) { $branch = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" rev-parse --short HEAD }
if (!$branch) { throw 'Impossible de déterminer le jalon Git pour isoler son historique.' }
$runtimeKey = $branch -replace '[^A-Za-z0-9_-]', '-'
$env:HYDRO_RUNTIME_DIR = Join-Path $RepoRoot ".runtime/$runtimeKey"
Write-Host "Moteur ETL : $EtlEngine"
$history = if ($EtlEngine -eq 'dotnet') { Join-Path $env:HYDRO_RUNTIME_DIR 'dotnet' } else { $env:HYDRO_RUNTIME_DIR }
Write-Host "Historique du jalon $branch : $history"
$options = @('start','--apphost',$AppHost,'--launch-profile','http','--non-interactive')
if ($NoBuild) { $options += '--no-build' }
Invoke-Checked aspire ($options + @('--',"--Hydro:EtlEngine=$EtlEngine"))
Invoke-Checked aspire @('wait','web','--status','healthy','--timeout','60','--apphost',$AppHost)
Write-Host 'HydroDataLab : http://localhost:53170 — API via http://localhost:53170/api'
Write-Host 'Ports directs API/ETL attribués par Aspire : consulter les endpoints du dashboard.'
Write-Host 'Ouvrir aussi le lien du tableau de bord Aspire affiché ci-dessus.'
