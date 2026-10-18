# ETL Python HydroDataLab

Python contient l'extraction, les règles métier et les exports. La CLI et FastAPI appellent exactement `pipeline.run_pipeline`; le conteneur exécute ce même paquet. L'API .NET pilote les tâches et ne recalcule aucun indicateur BI.

Depuis la racine du dépôt, avec Python 3.14 et uv :

```powershell
uv sync --project etl --frozen
uv run --project etl python -m hydrodatalab.extract --source fixture --date 2026-10-17 --output outputs/raw
uv run --project etl python -m hydrodatalab run --source fixture --date 2026-10-17 --output outputs/cli
uv run --project etl python -m hydrodatalab run --source live --output outputs/live
uv run --project etl python -m hydrodatalab run --source fixture --quality-scenario missing --output outputs/missing
uv run --project etl uvicorn hydrodatalab.api:app --host 127.0.0.1 --port 8001
```

`hydrodatalab.extract` reste autonome : il n'importe ni pandas, ni le pipeline, ni le calcul de couverture. Il peut être utilisé dès le checkpoint extraction. L'extraction `fixture` vérifie les empreintes SHA-256; `live` récupère la dernière journée civile terminée à Toronto avec pagination et au plus trois tentatives réseau par page. Un échec du mode connecté demeure un échec explicite : sélectionner `fixture` pour le secours sans réseau.

`--date YYYY-MM-DD` sélectionne une journée civile **terminée** dans `America/Toronto`, pour `extract` comme pour `run`. Sans date, `fixture` utilise sa journée archivée et `live` la journée précédente. Une date différente du 17 octobre 2026 est refusée avec le snapshot livré; elle n'est jamais remplacée discrètement. En mode connecté, la date doit aussi être encore disponible dans l'horizon de conservation du fournisseur. Les fenêtres de 23/25 heures sont calculées à partir des minuits locaux. Rejouer un UUID avec une date explicitement différente est refusé.

Les exports se trouvent dans `<output>/<runId>/`. Les variables facultatives sont `HYDRO_FIXTURE_DIR` et `HYDRO_OUTPUT_DIR`. Les données sources, leurs licences et leur horodatage d'extraction sont décrits dans `data/README.md`; les règles dans `docs/DATA_DICTIONARY.md`.

## Interface HTTP

- `GET /health` → `{"status":"ok"}`.
- `POST /run` avec `{"runId":"UUID","source":"fixture","simulateFailure":false,"qualityScenario":"complete"}` → rapport complet. `missing` retire un point d'eau valide après normalisation, en mémoire. `simulateFailure` produit un HTTP 503 avec un message exploitable par le pilote.
- `GET /artifacts/{runId}/observations.csv`, `observations.parquet` ou `report.json` → fichier publié. UUID et nom de fichier sont validés.

La route d'exécution est synchrone (`def`) et FastAPI la déporte dans son pool de threads; elle ne bloque pas la boucle événementielle. Un verrou non bloquant limite `/run` à **un pipeline actif par processus Python** : un second appel reçoit immédiatement HTTP 409, tandis que `/health` reste disponible. Le verrou est libéré après succès ou erreur. Une déconnexion ou un délai dépassé côté API .NET n'annule pas un calcul Python déjà commencé : le verrou reste détenu jusqu'à sa fin.

L'API .NET gère également son admission, l'historique et les reprises. Le service Python doit rester en **un seul worker Uvicorn et une seule instance**; le Dockerfile impose `--workers 1`. Le verrou mémoire ne coordonne pas plusieurs processus, plusieurs conteneurs ou une CLI lancée séparément. Pour plusieurs instances, il faudrait un verrou distribué ou une file de tâches durable. Le pipeline est idempotent pour un UUID déjà publié. Des fichiers temporaires laissés par un arrêt brutal ne constituent pas un résultat publié.

## Tests

```powershell
New-Item -ItemType Directory -Force etl/.test-output | Out-Null
uv run --project etl pytest etl/tests --basetemp etl/.test-output/pytest
uv run --project etl pytest specs/test_coverage_acceptance.py --basetemp etl/.test-output/acceptance
```

Les tests vérifient les vrais fichiers du snapshot, les exports CSV/Parquet, la parité CLI/HTTP, zéro/null, doublons, dates futures, unités, intégrité, pagination, panne réseau, publication atomique et les journées de 23/25 heures. Les cas réseau simulés n'envoient aucune requête externe. Le mode connecté se vérifie explicitement par la commande CLI `--source live`.

Pour préparer le checkpoint 06, remplacer **seulement** `etl/hydrodatalab/coverage.py` par `etl/demo/coverage_stub.py`. La page affiche alors la couverture indisponible. Les tests d'acceptation séparés échouent jusqu'à l'implémentation de `specs/coverage.md`; les tests unitaires qui vérifient déjà la couverture échouent également, ce qui est attendu pour ce checkpoint. Le checkpoint 07 restaure la fonction complète. Ne pas modifier le snapshot pendant cet exercice.

## Conteneur

Le contexte Docker est la **racine** du dépôt. Les versions de Python et uv sont fixées; les dépendances sont verrouillées par `uv.lock`.

Le conteneur possède un `HEALTHCHECK` Python stdlib sur `/health` toutes les 5 secondes, délai maximal 3 secondes, période de démarrage 15 secondes. Compose peut donc attendre sa disponibilité avec `--wait`.

```powershell
docker build --load -f etl/Dockerfile -t hydrodatalab-etl:formation .
docker run --rm -p 8001:8001 hydrodatalab-etl:formation
docker run --rm --mount "type=bind,source=$((Get-Location).Path)/outputs,target=/app/outputs" hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /app/outputs/docker
```

Créer `outputs` avant la commande avec montage. Après construction, le mode `fixture` fonctionne sans réseau. Le même dossier de fixtures est embarqué; la comparaison de résultats porte sur les lignes et valeurs, pas sur l'UUID ni l'instant de génération.

### Preuve de parité Windows/Linux

Validation effectuée le **18 octobre 2026**, Python 3.14.2 natif Windows et conteneur Linux via le contexte existant `podman-machine-default` (serveur 6.1.2), sans modifier le contexte Docker global. Le conteneur de vérification utilise `--network none` et est supprimé après récupération des exports. Résultat : 168 observations, 4 séries, aucune anomalie; CSV identiques **octet pour octet**, tables Parquet identiques avec leurs types et rapports JSON identiques hors instant de génération. SHA-256 CSV : `e7b8e903869e406a0af50ae261801251e4adb1691362997aec8003357846d04f`.

```powershell
docker --context podman-machine-default build --load -f etl/Dockerfile -t hydrodatalab-etl:formation .
uv run --project etl python etl/tests/verify_container.py --docker-context podman-machine-default
```

Le script écrit les preuves dans `etl/.test-output/container-parity/<runId>/verification.json`. Pour Docker Desktop fonctionnel, omettre `--docker-context`. Les contextes distants peuvent ne pas voir un montage de disque Windows : cette vérification utilise `docker cp` pour rester indépendante du partage des lecteurs.

Si la liaison Docker vers Podman ne répond plus alors que le client Podman fonctionne, les mêmes opérations sont possibles sans changer les contextes globaux :

```powershell
podman build --format docker -f etl/Dockerfile -t docker.io/library/hydrodatalab-etl:formation .
uv run --project etl python etl/tests/verify_container.py --engine podman --image docker.io/library/hydrodatalab-etl:formation
```

Le format `docker` préserve le `HEALTHCHECK`; Podman peut l'ignorer avec son format OCI par défaut.

L'audit vérifie aussi l'immuabilité des octets et des dates de modification lors d'un rejeu, le refus d'un changement de source/scénario/date, et deux exécutions concurrentes du même UUID sous les comportements de renommage Windows/Linux. La télémétrie actuelle comprend les journaux HTTP et le suivi par UUID; aucun exporteur OpenTelemetry Python supplémentaire n'est activé.
