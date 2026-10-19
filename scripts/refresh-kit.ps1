param([Parameter(Mandatory)][string]$KitPath)
. "$PSScriptRoot/common.ps1"

# Update the existing kit; replace the consolidated image archive only when its hash changes.
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
$archive = Join-Path $RepoRoot 'artifacts/hydrodatalab-images.tar'
$newHash = ((Get-Content -LiteralPath "$archive.sha256" -Raw).Trim() -split '\s+')[0]
$proof = Get-Content -LiteralPath "$archive.manifest.json" -Raw | ConvertFrom-Json
if ((Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash.ToLowerInvariant() -ne $newHash -or $newHash -ne $proof.sha256 -or -not $proof.manifestValidated) { throw 'Archive consolidée invalide.' }
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
# Handouts also link to original captures using their source-tree paths.
New-Item -ItemType Directory -Path "$kit/presentation/public" -Force | Out-Null
Get-ChildItem -LiteralPath (Join-Path $RepoRoot 'presentation/public') -Force | ForEach-Object {
    Copy-Item -LiteralPath $_.FullName -Destination "$kit/presentation/public" -Recurse -Force
}
Copy-Item -LiteralPath (Join-Path $RepoRoot 'presentation/exports/formation-hydrodatalab.pdf') -Destination "$kit/documents/formation-hydrodatalab.pdf" -Force
$guide = Get-Content -LiteralPath "$kit/DEMARRER.md" -Raw
if ($image[0].sha256 -ne $newHash) {
    $partialArchive = Join-Path $kit "images/hydrodatalab-images.tar.partial-$([guid]::NewGuid().ToString('N'))"
    Copy-Item -LiteralPath $archive -Destination $partialArchive
    if ((Get-FileHash -LiteralPath $partialArchive -Algorithm SHA256).Hash.ToLowerInvariant() -ne $newHash) { throw "Copie de l’archive invalide; original du kit conservé." }
    Move-Item -LiteralPath $partialArchive -Destination "$kit/images/hydrodatalab-images.tar" -Force
}
Copy-Item -LiteralPath "$archive.sha256", "$archive.manifest.json" -Destination "$kit/images/" -Force
$guide = $guide.Replace($image[0].sha256, $newHash)
# Remove only obsolete loading instructions, retaining any old archive as an unused backup.
$guide = ($guide -split "`r?`n" | Where-Object {
    $_ -notmatch '^(Get-FileHash|docker load|podman load).*hydrodatalab-web-typography' -and
    $_ -notmatch '^Mise à jour Vue :|^Charger la mise à jour typographique APRÈS'
}) -join "`n"
if (-not $guide.Contains('Archive ETL consolidée')) {
    $guide += "`nArchive ETL consolidée : charger uniquement images/hydrodatalab-images.tar. Ne pas charger ensuite l’ancien complément hydrodatalab-web-typography.tar, qui restaurerait une interface dépassée.`n"
}
$guide = $guide -replace 'HydroDataLab-\d{8}-\d{6}\.bundle', $bundle.Name
$guide = $guide -replace 'couches de contexte \(\d+\)', 'couches de contexte (10)'
$guide = $guide -replace 'boutons des coûts \(\d+\)', 'boutons des coûts (16)'
$guide | Set-Content -LiteralPath "$kit/DEMARRER.md" -Encoding utf8NoBOM
$head = (& git rev-parse HEAD).Trim()
@("Mise à jour : $([DateTime]::UtcNow.ToString('o'))", "Commit : $head", "Bundle actif : $($bundle.Name)", 'Archive Docker consolidée et vérifiée; aucun nouveau kit complet créé.') |
    Set-Content -LiteralPath "$kit/REVISION.txt" -Encoding utf8NoBOM
$files = @(Get-ChildItem -LiteralPath $kit -File -Recurse -Force | Where-Object FullName -ne "$kit\SHA256SUMS.json" | Sort-Object FullName | ForEach-Object {
    [pscustomobject]@{path=[IO.Path]::GetRelativePath($kit,$_.FullName).Replace('\','/');bytes=$_.Length;sha256=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant()}
})
[pscustomobject]@{schemaVersion=1;generatedAtUtc=[DateTime]::UtcNow.ToString('o');excludes=@('SHA256SUMS.json');files=$files} |
    ConvertTo-Json -Depth 5 | Set-Content -LiteralPath "$kit/SHA256SUMS.json" -Encoding utf8NoBOM
Invoke-Checked 'git' @('bundle','verify',"$kit/developpement/$($bundle.Name)")
Write-Host "Kit existant actualisé avec les images ETL consolidées : $kit"
Write-Host "$($files.Count) fichiers inventoriés; commit $head"
