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

La preuve compare CSV, table Parquet et rapport JSON, hors horodatage de génération. Un volume ou `docker cp` permet de conserver les exports hors du conteneur.

## Transporter les images avant la séance

```powershell
docker save -o artifacts/hydrodatalab-images.tar hydrodatalab-etl:formation hydrodatalab-api:formation hydrodatalab-web:formation mcr.microsoft.com/dotnet/aspire-dashboard:13.6.1
# Sur l'autre poste, avec son moteur déjà autorisé et démarré :
docker load -i artifacts/hydrodatalab-images.tar
```

Ajouter `--context <nom>` après `docker` si nécessaire. Copier aussi `artifacts/compose/` avec ses deux fichiers de configuration et le dossier préparé. Le bundle Git conserve le code et les données, pas les images, les dépendances installées ou les volumes.
