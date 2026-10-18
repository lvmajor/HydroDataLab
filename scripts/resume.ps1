param([switch]$Start)
. "$PSScriptRoot/common.ps1"
Get-Content PROGRESS.md
Invoke-RepoGit @('status','--short','--branch')
Invoke-RepoGit @('log','-5','--oneline','--decorate')
& aspire ps
if (Test-Path '.runtime/main/api') { Get-ChildItem -LiteralPath '.runtime/main/api' | Select-Object Name,Length,LastWriteTime }
Write-Host 'Aucun nettoyage ou reset effectué. Une exécution abandonnée sera marquée interrompue au redémarrage.'
if ($Start) { & "$PSScriptRoot/start-demo.ps1" }
