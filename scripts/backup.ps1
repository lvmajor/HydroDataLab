. "$PSScriptRoot/common.ps1"
$state = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" status --porcelain
if ($LASTEXITCODE -ne 0) { throw 'Impossible de lire Git.' }
if ($state) { throw 'Le bundle ne sauvegarde que Git. Enregistrer les modifications dans un commit avant la sauvegarde.' }
New-Item -ItemType Directory -Force artifacts | Out-Null
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$bundle = "artifacts/HydroDataLab-$stamp.bundle"
Invoke-RepoGit @('bundle','create',$bundle,'--all')
Invoke-RepoGit @('bundle','verify',$bundle)
Invoke-RepoGit @('archive','--format=zip',"--output=artifacts/HydroDataLab-$stamp.zip",'HEAD')
Write-Host "Sauvegarde portable : $bundle"
Write-Host 'Copier artifacts sur un autre support. Pour restaurer : git clone <fichier.bundle> HydroDataLab'
Write-Host 'Les dépendances et images Docker ne sont pas incluses; conserver aussi la copie préparée du dossier pour le mode hors réseau.'
