param([Parameter(Mandatory)][string]$KitPath)
. "$PSScriptRoot/common.ps1"

# Keep the four-image archive; copy only the separate Vue update with the documents.
$artifactRoot = (Resolve-Path -LiteralPath (Join-Path $RepoRoot 'artifacts')).Path
$kit = (Resolve-Path -LiteralPath $KitPath).Path
$kitItem = Get-Item -LiteralPath $kit -Force
if (-not $kitItem.PSIsContainer -or $kitItem.Parent.FullName -ne $artifactRoot -or $kitItem.Name -notmatch '^kit-hq-\d{8}-\d{6}$') {
    throw 'Le kit doit être un sous-dossier kit-hq-AAAAMMJJ-HHMMSS de artifacts.'
}
if (($kitItem.Attributes -band [IO.FileAttributes]::ReparsePoint) -or @(Get-ChildItem -LiteralPath $kit -Recurse -Force | Where-Object { $_.Attributes -band [IO.FileAttributes]::ReparsePoint }).Count) {
    throw 'Un lien de système de fichiers interdit cette mise à jour.'
}
$gitState = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" status --porcelain
if ($LASTEXITCODE -ne 0 -or $gitState) { throw 'Enregistrer les changements dans Git avant la mise à jour du kit.' }
foreach ($relative in @('DEMARRER.md','SHA256SUMS.json','images/hydrodatalab-images.tar','developpement')) {
    if (-not (Test-Path -LiteralPath (Join-Path $kit $relative))) { throw "Kit incomplet : $relative" }
}
foreach ($relative in @('presentation/dist/index.html','presentation/exports/formation-hydrodatalab.pdf')) {
    if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $relative))) { throw "Source manquante : $relative" }
}
$manifest = Get-Content -LiteralPath "$kit/SHA256SUMS.json" -Raw | ConvertFrom-Json
$webArchive = Join-Path $RepoRoot 'artifacts/hydrodatalab-web-typography.tar'
$webHash = ((Get-Content -LiteralPath "$webArchive.sha256" -Raw).Trim() -split '\s+')[0]
if ((Get-FileHash -LiteralPath $webArchive -Algorithm SHA256).Hash.ToLowerInvariant() -ne $webHash) { throw 'Archive de mise à jour Vue invalide.' }
$image = @($manifest.files | Where-Object path -eq 'images/hydrodatalab-images.tar')
if ($image.Count -ne 1 -or (Get-FileHash -LiteralPath "$kit/images/hydrodatalab-images.tar" -Algorithm SHA256).Hash.ToLowerInvariant() -ne $image[0].sha256) {
    throw 'Archive Docker conservée invalide; ne pas poursuivre.'
}
$previous = @(Get-ChildItem -LiteralPath artifacts -Filter 'HydroDataLab-*.bundle' -File | ForEach-Object FullName)
& "$PSScriptRoot/backup.ps1"
$newBundles = @(Get-ChildItem -LiteralPath artifacts -Filter 'HydroDataLab-*.bundle' -File | Where-Object { $_.FullName -notin $previous })
if ($newBundles.Count -ne 1) { throw 'Un unique bundle fraîchement généré est requis.' }
$bundle = $newBundles[0]
$zip = [IO.Path]::ChangeExtension($bundle.FullName, '.zip')
Copy-Item -LiteralPath $bundle.FullName, $zip -Destination "$kit/developpement/"

# No existing backup is deleted. Old hashed assets can remain harmlessly unused.
foreach ($pair in @(@('handouts','handouts'),@('docs','docs'))) {
    Get-ChildItem -LiteralPath (Join-Path $RepoRoot $pair[0]) -Force | ForEach-Object {
        Copy-Item -LiteralPath $_.FullName -Destination (Join-Path $kit $pair[1]) -Recurse -Force
    }
}
Get-ChildItem -LiteralPath (Join-Path $RepoRoot 'presentation/dist') -Force | Where-Object Name -ne 'index.html' | ForEach-Object {
    Copy-Item -LiteralPath $_.FullName -Destination "$kit/presentation" -Recurse -Force
}
Copy-Item -LiteralPath (Join-Path $RepoRoot 'presentation/dist/index.html') -Destination "$kit/presentation/index.html" -Force
Copy-Item -LiteralPath (Join-Path $RepoRoot 'presentation/exports/formation-hydrodatalab.pdf') -Destination "$kit/documents/formation-hydrodatalab.pdf" -Force
$guide = Get-Content -LiteralPath "$kit/DEMARRER.md" -Raw
Copy-Item -LiteralPath $webArchive, "$webArchive.sha256" -Destination "$kit/images/" -Force
if (-not $guide.Contains('docker load --input images/hydrodatalab-web-typography.tar')) {
    $guide = $guide.Replace('docker load --input images/hydrodatalab-images.tar', "docker load --input images/hydrodatalab-images.tar`nGet-FileHash images/hydrodatalab-web-typography.tar -Algorithm SHA256`ndocker load --input images/hydrodatalab-web-typography.tar")
    $guide += "`nMise à jour Vue : charger l’archive typographique APRÈS l’archive complète, avant Compose. Comparer son empreinte à images/hydrodatalab-web-typography.tar.sha256. La petite archive remplace uniquement l’image web; les autres images restent conservées.`n"
}
$guide = $guide -replace 'HydroDataLab-\d{8}-\d{6}\.bundle', $bundle.Name
$guide = $guide.Replace('couches de contexte (11)', 'couches de contexte').Replace('boutons des coûts (14)', 'boutons des coûts')
$guide | Set-Content -LiteralPath "$kit/DEMARRER.md" -Encoding utf8NoBOM
$head = (& git rev-parse HEAD).Trim()
@("Mise à jour : $([DateTime]::UtcNow.ToString('o'))", "Commit : $head", "Bundle actif : $($bundle.Name)", 'Archive Docker conservée et vérifiée; aucun nouveau kit complet créé.') |
    Set-Content -LiteralPath "$kit/REVISION.txt" -Encoding utf8NoBOM
$files = @(Get-ChildItem -LiteralPath $kit -File -Recurse -Force | Where-Object FullName -ne "$kit\SHA256SUMS.json" | Sort-Object FullName | ForEach-Object {
    [pscustomobject]@{path=[IO.Path]::GetRelativePath($kit,$_.FullName).Replace('\','/');bytes=$_.Length;sha256=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant()}
})
[pscustomobject]@{schemaVersion=1;generatedAtUtc=[DateTime]::UtcNow.ToString('o');excludes=@('SHA256SUMS.json');files=$files} |
    ConvertTo-Json -Depth 5 | Set-Content -LiteralPath "$kit/SHA256SUMS.json" -Encoding utf8NoBOM
Invoke-Checked 'git' @('bundle','verify',"$kit/developpement/$($bundle.Name)")
Write-Host "Kit actualisé avec la petite mise à jour Vue, sans recopier l’archive complète : $kit"
Write-Host "$($files.Count) fichiers inventoriés; commit $head"
