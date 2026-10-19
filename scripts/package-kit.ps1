#requires -Version 7.0
param()

. "$PSScriptRoot/common.ps1"

$gitState = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" status --porcelain
if ($LASTEXITCODE -ne 0) { throw 'Impossible de lire le dépôt Git.' }
if ($gitState) { throw 'Le dépôt doit être propre. Enregistrer les changements avant de créer le kit.' }

$required = @(
    'LICENSE',
    'artifacts/hydrodatalab-images.tar',
    'artifacts/hydrodatalab-images.tar.sha256',
    'artifacts/hydrodatalab-images.tar.manifest.json',
    'artifacts/hydrodatalab-web-typography.tar',
    'artifacts/hydrodatalab-web-typography.tar.sha256',
    'artifacts/compose/docker-compose.yaml',
    'artifacts/compose/images.env',
    'artifacts/compose/.env',
    'presentation/dist/index.html',
    'presentation/exports/formation-hydrodatalab.pdf',
    'handouts',
    'docs/screenshots',
    'data/fixtures/manifest.json'
)
foreach ($path in $required) {
    if (-not (Test-Path -LiteralPath (Join-Path $RepoRoot $path))) { throw "Source manquante : $path" }
}
foreach ($folder in @('handouts', 'docs/screenshots')) {
    if (-not @(Get-ChildItem -LiteralPath $folder -File -Recurse -Force).Count) { throw "Dossier vide : $folder" }
}

$fixtureManifest = Get-Content data/fixtures/manifest.json -Raw | ConvertFrom-Json
foreach ($name in @('demand.json', 'hydrometeo.json')) {
    $metadata = @($fixtureManifest.sources | Where-Object file -eq $name)
    $actual = (Get-FileHash -LiteralPath "data/fixtures/$name" -Algorithm SHA256).Hash.ToLowerInvariant()
    if ($metadata.Count -ne 1 -or $actual -ne $metadata[0].sha256) { throw "Snapshot incohérent : $name" }
}

$archive = Join-Path $RepoRoot 'artifacts/hydrodatalab-images.tar'
$proof = Get-Content -LiteralPath "$archive.manifest.json" -Raw | ConvertFrom-Json
$declaredHash = ((Get-Content -LiteralPath "$archive.sha256" -Raw).Trim() -split '\s+')[0].ToLowerInvariant()
Write-Host "Vérification de l'archive des images..."
$archiveHash = (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash.ToLowerInvariant()
if ($archiveHash -ne $declaredHash -or $archiveHash -ne $proof.sha256 -or (Get-Item -LiteralPath $archive).Length -ne $proof.bytes) {
    throw "La taille ou l'empreinte de l'archive ne correspond pas aux preuves."
}
$archiveManifestText = & tar -xOf $archive manifest.json
if ($LASTEXITCODE -ne 0) { throw "Manifest de l'archive illisible." }
$archiveManifest = @(($archiveManifestText | Out-String) | ConvertFrom-Json)
$images = @('docker.io/library/hydrodatalab-etl:formation', 'docker.io/library/hydrodatalab-api:formation', 'docker.io/library/hydrodatalab-web:formation', 'mcr.microsoft.com/dotnet/aspire-dashboard:13.6.1')
if ($archiveManifest.Count -ne 4 -or @($proof.images).Count -ne 4 -or -not $proof.manifestValidated) { throw 'Les quatre images validées sont requises.' }
foreach ($image in $images) {
    $record = @($proof.images | Where-Object Image -eq $image)
    $tag = $image -replace '^docker.io/library/', ''
    $entry = @($archiveManifest | Where-Object { @($_.RepoTags | ForEach-Object { $_ -replace '^docker.io/library/', '' }) -contains $tag })
    if ($record.Count -ne 1 -or $entry.Count -ne 1 -or $record[0].Id -notmatch '^[a-f0-9]{64}$') { throw "Image manquante ou ambiguë : $image" }
    if ($entry[0].Config -notmatch ('(^|/)' + [regex]::Escape($record[0].Id) + '(\.json)?$')) { throw "Identifiant archivé incohérent : $image" }
}

$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$kit = Join-Path $RepoRoot "artifacts/kit-hq-$stamp"
if (Test-Path -LiteralPath $kit) { throw "Le kit existe déjà et sera conservé : $kit" }
$oldBundles = @(Get-ChildItem -LiteralPath artifacts -Filter 'HydroDataLab-*.bundle' -File | ForEach-Object FullName)
& "$PSScriptRoot/backup.ps1"
$newBundles = @(Get-ChildItem -LiteralPath artifacts -Filter 'HydroDataLab-*.bundle' -File | Where-Object { $oldBundles -notcontains $_.FullName })
if ($newBundles.Count -ne 1) { throw "Impossible d'identifier un unique bundle fraîchement généré; relancer après une seconde." }
$bundle = $newBundles[0]
$zip = [IO.Path]::ChangeExtension($bundle.FullName, '.zip')
if (-not (Test-Path -LiteralPath $zip)) { throw 'Le ZIP de code fraîchement généré est absent.' }

# Aucun kit existant n'est supprimé ou remplacé. Un échec conserve le dossier partiel.
New-Item -ItemType Directory -Path $kit -ErrorAction Stop | Out-Null
function Copy-KitDirectory {
    param([string]$Source, [string]$Destination)
    New-Item -ItemType Directory -Path $Destination -ErrorAction Stop | Out-Null
    Get-ChildItem -LiteralPath $Source -Force | ForEach-Object {
        Copy-Item -LiteralPath $_.FullName -Destination $Destination -Recurse -ErrorAction Stop
    }
}
New-Item -ItemType Directory -Path "$kit/developpement", "$kit/images", "$kit/documents" | Out-Null
Copy-Item -LiteralPath $bundle.FullName, $zip -Destination "$kit/developpement/"
Copy-Item -LiteralPath $archive, "$archive.sha256", "$archive.manifest.json" -Destination "$kit/images/"
$webArchive = Join-Path $RepoRoot 'artifacts/hydrodatalab-web-typography.tar'
$webHash = ((Get-Content -LiteralPath "$webArchive.sha256" -Raw).Trim() -split '\s+')[0]
if ((Get-FileHash -LiteralPath $webArchive -Algorithm SHA256).Hash.ToLowerInvariant() -ne $webHash) { throw 'Archive de mise à jour Vue invalide.' }
Copy-Item -LiteralPath $webArchive, "$webArchive.sha256" -Destination "$kit/images/"
Copy-KitDirectory 'artifacts/compose' "$kit/compose"
Copy-KitDirectory 'presentation/dist' "$kit/presentation"
Copy-KitDirectory 'handouts' "$kit/handouts"
Copy-KitDirectory 'docs/screenshots' "$kit/captures"
Copy-KitDirectory 'docs' "$kit/docs"
Copy-KitDirectory 'data' "$kit/data"
Copy-Item -LiteralPath LICENSE -Destination "$kit/LICENSE"
Copy-Item -LiteralPath presentation/exports/formation-hydrodatalab.pdf -Destination "$kit/documents/"

$server = @'
"""Présentation statique locale : Python standard library, sans npm."""
import argparse
import mimetypes
from functools import partial
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
from urllib.parse import unquote, urlsplit


class PresentationHandler(SimpleHTTPRequestHandler):
    def prepare_path(self):
        path = unquote(urlsplit(self.path).path)
        # Compatibilité avec les anciens builds Slidev à chemins relatifs.
        marker = "/assets/"
        if marker in path:
            self.path = marker + path.split(marker, 1)[1]
        target = Path(self.translate_path(self.path))
        if not target.exists() and not Path(path).suffix and marker not in path:
            self.path = "/index.html"

    def do_GET(self):
        self.prepare_path()
        super().do_GET()

    def do_HEAD(self):
        self.prepare_path()
        super().do_HEAD()


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--port", type=int, default=53173)
    args = parser.parse_args()
    root = Path(__file__).resolve().parent / "presentation"
    if not (root / "index.html").is_file():
        parser.error("Le dossier presentation/index.html est manquant")
    mimetypes.add_type("application/javascript", ".js")
    mimetypes.add_type("application/javascript", ".mjs")
    server = ThreadingHTTPServer(("127.0.0.1", args.port), partial(PresentationHandler, directory=str(root)))
    print(f"Présentation : http://localhost:{args.port}/1", flush=True)
    print("Arrêt : Ctrl+C", flush=True)
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        pass
    finally:
        server.server_close()
'@
$server | Set-Content -LiteralPath "$kit/servir-presentation.py" -Encoding utf8NoBOM

$guide = @'
# Démarrer le kit HydroDataLab

Ce dossier réunit les images testées, Compose, le diaporama statique, le PDF, les notes, les captures et les sauvegardes du code. Les données du mode fixture sont déjà intégrées à l'image Python. Les volumes et historiques des anciens postes ne sont pas transportés.

## Prérequis préparés avant la séance

- Docker avec Compose, ou Podman avec un fournisseur Compose, installé, autorisé et démarré.
- PowerShell 7 pour les commandes et scripts PowerShell (encodage utf8NoBOM).
- Python 3 pour le petit serveur de présentation; aucune dépendance Python supplémentaire ni npm n'est nécessaire.
- Un navigateur. Git n'est nécessaire que pour reprendre le développement.

Ouvrir PowerShell dans ce dossier. SHA-256 de l'archive d'images : `__IMAGE_HASH__`. La preuve complète est dans `images/hydrodatalab-images.tar.manifest.json`. `SHA256SUMS.json` recense les empreintes de tous les autres fichiers du kit; l'inventaire ne peut pas inclure sa propre empreinte.

## Application hors ligne

```powershell
Get-FileHash images/hydrodatalab-images.tar -Algorithm SHA256
docker load --input images/hydrodatalab-images.tar
Get-FileHash images/hydrodatalab-web-typography.tar -Algorithm SHA256
docker load --input images/hydrodatalab-web-typography.tar
docker compose -p hydrodatalab-training --env-file compose/images.env -f compose/docker-compose.yaml up -d --pull never --no-build --wait --wait-timeout 90
```

Comparer l'empreinte à celle indiquée ci-dessus avant le chargement. Avec Podman, remplacer `docker` par `podman` dans les commandes; son fournisseur Compose doit être disponible. L'application écoute sur **http://localhost:53170** et le dashboard sur **http://localhost:53174**. Choisir la source **fixture** pour fonctionner sans réseau. La source live et les agents Copilot demandent leurs accès réseau habituels.

Charger la mise à jour typographique APRÈS l’archive complète. Comparer son empreinte au fichier `images/hydrodatalab-web-typography.tar.sha256`. Cette petite archive remplace uniquement l’image web.

Arrêter les conteneurs en conservant les volumes SQLite et les exports :

```powershell
docker compose -p hydrodatalab-training --env-file compose/images.env -f compose/docker-compose.yaml stop --timeout 30
```

## Présentation sans npm

```powershell
python servir-presentation.py
```

Ouvrir **http://localhost:53173/1** pour l'audience et **http://localhost:53173/presenter/1** pour les notes du présentateur. Le serveur prend en charge les liens directs vers les diapositives. Les consignes de manipulation, minutages et procédures de secours restent dans les notes; projeter uniquement la vue audience. Le guide `handouts/guide-presentateur.md` est aussi disponible. Arrêter le serveur avec Ctrl+C.

Secours sans serveur : ouvrir `documents/formation-hydrodatalab.pdf`. Les notes et requêtes sont dans `handouts/`, les captures dans `captures/`.

Deux écrans : choisir Étendre (Win + P), ouvrir les deux vues dans le même navigateur/profil avec le même hôte et port. Les couches de contexte (13) et les boutons des coûts (16) se pilotent dans le panneau Current → Slides du présentateur; leurs réglages se synchronisent avec l’audience et sont conservés après rechargement. Avant la séance, sélectionner la première couche et 1 essai. Recharger les deux fenêtres après une mise à jour du kit.

## Reprendre le développement

```powershell
git clone developpement/__BUNDLE_NAME__ HydroDataLab
```

Le ZIP voisin contient également le code. Lire ensuite le README du dépôt cloné. Les SDK, dépendances installées et caches du parcours natif ne sont pas contenus dans le bundle; effectuer sa préparation avant une séance hors réseau. Pour utiliser les scripts Compose du dépôt cloné, y recopier `compose/` dans `artifacts/compose/` et les fichiers d'images dans `artifacts/`. Les images chargées restent disponibles dans le moteur du poste.
'@
$guide.Replace('__IMAGE_HASH__', $archiveHash).Replace('__BUNDLE_NAME__', $bundle.Name) |
    Set-Content -LiteralPath "$kit/DEMARRER.md" -Encoding utf8NoBOM

Write-Host 'Calcul des empreintes de tous les fichiers du kit...'
$files = @(Get-ChildItem -LiteralPath $kit -File -Recurse -Force | Sort-Object FullName | ForEach-Object {
    [pscustomobject]@{
        path = [IO.Path]::GetRelativePath($kit, $_.FullName).Replace('\', '/')
        bytes = $_.Length
        sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
    }
})
$copiedArchive = @($files | Where-Object path -eq 'images/hydrodatalab-images.tar')
if ($copiedArchive.Count -ne 1 -or $copiedArchive[0].sha256 -ne $archiveHash) {
    throw "L'empreinte de l'archive copiée diffère de la source. Le kit partiel est conservé pour diagnostic."
}
Invoke-Checked 'git' @('-c', "safe.directory=$($RepoRoot.Replace('\','/'))", 'bundle', 'verify', "$kit/developpement/$($bundle.Name)")
[pscustomobject]@{
    schemaVersion = 1
    generatedAtUtc = [DateTime]::UtcNow.ToString('o')
    excludes = @('SHA256SUMS.json')
    files = $files
} | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath "$kit/SHA256SUMS.json" -Encoding utf8NoBOM
Write-Host "Kit prêt : $kit"
Write-Host "$($files.Count) fichiers répertoriés. Ouvrir DEMARRER.md sur le poste cible."
