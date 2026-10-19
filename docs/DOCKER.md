# Exécuter la même chaîne en conteneurs

Le modèle source est `src/AppHost/apphost.cs`. `aspire publish` génère `artifacts/compose/docker-compose.yaml`; ce fichier n'est pas modifié à la main. Trois Dockerfiles préparent Python, .NET et Vue. Le dashboard Aspire est un quatrième conteneur. L'AppHost lui-même reste un outil local de modélisation.

```powershell
./scripts/prepare-docker.ps1
./scripts/docker-demo.ps1
./scripts/verify-http.ps1 -BaseUrl http://localhost:53170
./scripts/docker-demo.ps1 -Action Stop
```

Les scripts acceptent `-DockerContext podman-machine-default`, `-DockerHost` pour une connexion explicite, ou `-Engine podman` pour le client Podman direct. Ils ne changent pas le contexte global. Sur le poste de construction, Docker Desktop n'a pas démarré; les tests ont utilisé Podman 6.1.2. Sa liaison Docker par named pipe s'est aussi bloquée : la construction directe `-Engine podman` a fonctionné. Confirmer le moteur autorisé et sa connexion Compose sur le portable HQ.

La préparation arrête l'application native pour libérer les ports et l'exécutable AppHost Windows. Elle construit les images avec `--load` pour Docker ou `--format docker` pour Podman (conservation des healthchecks), et remplit un fichier `images.env` séparé. Le lancement utilise `--pull never --no-build` : aucun téléchargement en séance. Le frontend écoute sur `127.0.0.1:53170`; API et Python restent sur le réseau Compose. Le dashboard local utilise `127.0.0.1:53174`, sans authentification pour cette démonstration locale.

Les volumes `hydrolab-state` et `hydrolab-exports` conservent SQLite et les résultats. Le nom de projet Compose est `hydrodatalab-training`. L'action Stop ne supprime aucun volume. L'historique conteneur est indépendant de l'historique natif. Ne pas exécuter Compose pendant la démonstration de génération au jalon 06 : les images préparées contiennent la solution finale.

## Python seul en conteneur

```powershell
docker run --rm --network none hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /tmp/result
uv run --project etl python etl/tests/verify_container.py
# Avec le moteur utilisé pour la validation :
uv run --project etl python etl/tests/verify_container.py --docker-context podman-machine-default
```

La preuve compare les deux CSV, la table Parquet et le rapport métier JSON, hors horodatage de génération et diagnostics temporels. Un volume ou `docker cp` permet de conserver les exports hors du conteneur.

## Transporter et restaurer les images avant la séance

**Mise à jour ETL du 19 octobre 2026 :** l’archive complète contient maintenant Python, API et Vue avec les analyses et les détails d’exécution. Charger uniquement cette archive. L’ancien complément `hydrodatalab-web-typography.tar` est dépassé : ne pas le charger après la nouvelle archive, car il rétablirait une ancienne interface.

L’export du **19 octobre à 16:29:52 UTC** contient quatre images, manifest vérifié, **1 205 309 440 octets**. SHA-256 :

```text
7e5a7b8d6258cd2ee2bf62c0b6e3bfebe7d2cbef2880981e61ee923e4d312046
```

| Image | Début de l’identifiant |
|---|---|
| `hydrodatalab-etl:formation` | `7113c1396e38` |
| `hydrodatalab-api:formation` | `650c30b91ae7` |
| `hydrodatalab-web:formation` | `9dd779a816f9` |
| `mcr.microsoft.com/dotnet/aspire-dashboard:13.6.1` | `536e41832541` |

Le fichier `.manifest.json` adjacent contient les identifiants complets. Parité hors réseau vérifiée pour observations CSV, Parquet, synthèse horaire CSV et rapport métier. Seuls les horodatages de génération et les diagnostics temporels sont exclus de la comparaison métier; leurs statuts de réussite sont vérifiés séparément.

### Contenu du kit portable

Copier ensemble le projet et les éléments suivants, en conservant leur emplacement relatif :

- `artifacts/hydrodatalab-images.tar`;
- `artifacts/hydrodatalab-images.tar.sha256` et `artifacts/hydrodatalab-images.tar.manifest.json`;
- **le dossier complet `artifacts/compose/`**, contenant `docker-compose.yaml`, `images.env` et `.env`.

Le dossier `artifacts/` est ignoré par Git. **Un bundle Git ou le ZIP du code seul ne transporte donc ni les images ni Compose généré.** Ajouter ces fichiers séparément à l'archive finale ou au support de transport. Les scripts utilisent explicitement `--env-file artifacts/compose/images.env` : `docker-compose.yaml` et `images.env` sont indispensables au démarrage. Le `.env` généré par Aspire est conservé avec le dossier pour la traçabilité.

Les dépendances installées du parcours natif et les volumes Docker ne sont pas inclus dans l'archive d'images. Sur un nouveau poste, les volumes du démonstrateur sont créés au premier lancement; le snapshot public est déjà intégré à l'image Python.

### Restaurer sur le poste cible

Le moteur autorisé et les prérequis des scripts doivent être installés avant la séance. Depuis la racine du projet, vérifier l'archive copiée :

```powershell
$imageArchiveHash = (Get-FileHash artifacts/hydrodatalab-images.tar -Algorithm SHA256).Hash.ToLowerInvariant()
if ($imageArchiveHash -ne '7e5a7b8d6258cd2ee2bf62c0b6e3bfebe7d2cbef2880981e61ee923e4d312046') {
    throw "L'empreinte de l'archive ne correspond pas à l'export validé."
}
Test-Path artifacts/compose/docker-compose.yaml
Test-Path artifacts/compose/images.env

```

Les deux vérifications de chemins doivent renvoyer `True`. Choisir ensuite la séquence correspondant au moteur installé. **Le chargement précède le démarrage hors ligne** et ne nécessite ni reconstruction ni téléchargement.

Avec Docker :

```powershell
docker load --input artifacts/hydrodatalab-images.tar
./scripts/docker-demo.ps1
./scripts/verify-http.ps1 -BaseUrl http://localhost:53170
```

Avec Podman :

```powershell
podman load --input artifacts/hydrodatalab-images.tar
./scripts/docker-demo.ps1 -Engine podman
./scripts/verify-http.ps1 -BaseUrl http://localhost:53170
```

Pour un contexte Docker explicite, ajouter `--context <nom>` à `docker load` et le même `-DockerContext <nom>` au script. Le lancement utilise `--pull never --no-build`. Le script `prepare-docker.ps1` correspond à la préparation initiale : il publie Compose, construit les images et télécharge le dashboard; le parcours portable ci-dessus utilise les fichiers et images déjà préparés.

### Si Compose généré n'a pas été transporté

Sur un poste dont les SDK, outils et dépendances ont déjà été préparés, republier le modèle **avant la séance hors réseau**, puis recréer le fichier de sélection des images :

```powershell
./scripts/stop-demo.ps1
aspire publish --apphost src/AppHost/apphost.cs -o artifacts/compose --non-interactive
@(
    'PYTHON_IMAGE=docker.io/library/hydrodatalab-etl:formation'
    'API_IMAGE=docker.io/library/hydrodatalab-api:formation'
    'WEB_IMAGE=docker.io/library/hydrodatalab-web:formation'
) | Set-Content artifacts/compose/images.env -Encoding ascii
```

Cette publication peut nécessiter une restauration de packages si le poste n'est pas préparé. Copier ensuite tout `artifacts/compose/` avec le kit. `docker-compose.yaml` reste produit par Aspire, sans retouche manuelle.

### Produire une nouvelle archive

Après validation d'une nouvelle version des quatre images :

```powershell
./scripts/export-images.ps1 -Engine podman
# Variante avec Docker :
./scripts/export-images.ps1 -Engine docker
```

Choisir une seule commande; ajouter `-Force` pour remplacer explicitement une archive existante. Le script exporte les images présentes, vérifie le manifest et leurs identifiants, puis produit les fichiers de preuve. Une nouvelle archive aura sa propre empreinte : utiliser celle de son fichier `.sha256`.
