param(
    [ValidateSet('podman', 'docker')][string]$Engine = 'podman',
    [string]$DockerHost,
    [string]$DockerContext,
    [string]$ArchivePath = 'artifacts/hydrodatalab-images.tar',
    [switch]$Force
)

. "$PSScriptRoot/common.ps1"
if ($DockerHost -and $DockerContext) { throw 'Choisir DockerHost ou DockerContext, pas les deux.' }
if ($Engine -eq 'podman' -and ($DockerHost -or $DockerContext)) { throw 'DockerHost et DockerContext concernent uniquement le moteur docker.' }

$engineArgs = @()
if ($DockerHost) { $engineArgs += @('--host', $DockerHost) }
if ($DockerContext) { $engineArgs += @('--context', $DockerContext) }
$images = @(
    'docker.io/library/hydrodatalab-etl:formation',
    'docker.io/library/hydrodatalab-api:formation',
    'docker.io/library/hydrodatalab-web:formation',
    'mcr.microsoft.com/dotnet/aspire-dashboard:13.6.1'
)

if (-not [IO.Path]::IsPathRooted($ArchivePath)) { $ArchivePath = Join-Path $RepoRoot $ArchivePath }
$ArchivePath = [IO.Path]::GetFullPath($ArchivePath)
if ((Test-Path -LiteralPath $ArchivePath) -and -not $Force) { throw "L'archive existe déjà : $ArchivePath. Utiliser -Force pour la remplacer." }
New-Item -ItemType Directory -Force (Split-Path -Parent $ArchivePath) | Out-Null
$partial = "$ArchivePath.partial-$([guid]::NewGuid().ToString('N'))"
$expected = @()
foreach ($image in $images) {
    $id = & $Engine @engineArgs image inspect $image --format '{{.Id}}'
    if ($LASTEXITCODE -ne 0) { throw "Image absente ou moteur indisponible : $image" }
    $id = ($id | Out-String).Trim() -replace '^sha256:', ''
    if ($id -notmatch '^[a-f0-9]{64}$') { throw "Identifiant d'image inattendu : $image" }
    $expected += [pscustomobject]@{ Image = $image; Id = $id }
}

Write-Host "Export des quatre images déjà présentes via $Engine..."
$saveArgs = @('save')
if ($Engine -eq 'podman') { $saveArgs += @('--format', 'docker-archive', '--multi-image-archive', '--quiet') }
$saveArgs += @('--output', $partial) + $images
Invoke-Checked $Engine ($engineArgs + $saveArgs)
if (-not (Test-Path -LiteralPath $partial)) { throw "Le moteur n'a pas produit le fichier archive." }
$length = (Get-Item -LiteralPath $partial).Length
if ($length -lt 1MB) { throw "Archive anormalement petite : $length octets. Fichier conservé : $partial" }

Write-Host "Validation du manifest de l'archive..."
$manifestJson = & tar -xOf $partial manifest.json
if ($LASTEXITCODE -ne 0) { throw "manifest.json introuvable; archive partielle conservée : $partial" }
$manifest = @(($manifestJson | Out-String) | ConvertFrom-Json)
if ($manifest.Count -ne $images.Count) { throw "L'archive contient $($manifest.Count) entrées au lieu de $($images.Count)." }
foreach ($item in $expected) {
    $canonicalTag = $item.Image -replace '^docker.io/library/', ''
    $entry = @($manifest | Where-Object {
        $tags = @($_.RepoTags | ForEach-Object { $_ -replace '^docker.io/library/', '' })
        $tags -contains $canonicalTag
    })
    if ($entry.Count -ne 1) { throw "Image absente ou ambiguë dans le manifest : $($item.Image)" }
    if ($entry[0].Config -notmatch ('(^|/)' + [regex]::Escape($item.Id) + '(\.json)?$')) {
        throw "La configuration archivée ne correspond pas à l'image inspectée : $($item.Image)"
    }
    if (@($entry[0].Layers).Count -eq 0) { throw "L'image ne contient aucune couche : $($item.Image)" }
}

Move-Item -LiteralPath $partial -Destination $ArchivePath -Force:$Force
$sha256 = (Get-FileHash -LiteralPath $ArchivePath -Algorithm SHA256).Hash.ToLowerInvariant()
$evidence = [pscustomobject]@{
    exportedAtUtc = [DateTime]::UtcNow.ToString('o')
    engine = $Engine
    archive = (Split-Path -Leaf $ArchivePath)
    bytes = $length
    sha256 = $sha256
    imageCount = $manifest.Count
    images = $expected
    manifestValidated = $true
}
$evidence | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath "$ArchivePath.manifest.json" -Encoding utf8NoBOM
"$sha256  $(Split-Path -Leaf $ArchivePath)" | Set-Content -LiteralPath "$ArchivePath.sha256" -Encoding ascii
Write-Host ("Archive validée : {0} ({1:N1} Mio), {2} images." -f $ArchivePath, ($length / 1MB), $manifest.Count)
Write-Host 'Copier aussi les fichiers .sha256 et .manifest.json. Restaurer avec docker load --input <archive> ou podman load --input <archive>.'
