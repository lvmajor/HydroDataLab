# Contrats de HydroDataLab

## Pipeline Python
Racine du paquet : `etl/`, module `hydrodatalab`. Commande : `uv run --project etl python -m hydrodatalab run --source fixture --output outputs/cli`. Commande extraction seule : sous-commande `extract`. Adaptateur : `hydrodatalab.api:app` via uvicorn, port 8001 par défaut.

`GET /health` renvoie `{ "status": "ok" }`.
`POST /run` reçoit `{ "runId": "uuid", "source": "fixture|live", "simulateFailure": false, "qualityScenario": "complete|missing" }`. Il exécute le même cœur que la CLI et renvoie le rapport complet ci-dessous. Échec HTTP non-2xx avec message lisible. Un scénario `missing` retire une observation de niveau d'eau dans une copie en mémoire et le signale dans le rapport; les données originales restent intactes.

## Rapport / API de consultation
`GET /api/report` renvoie le dernier rapport réussi, ou un rapport initial de démonstration fourni par le même moteur Python. Avant le premier succès, répondre 404 avec message explicite.

```json
{
  "schemaVersion": 1,
  "runId": "uuid",
  "source": "fixture",
  "generatedAtUtc": "2026-10-18T12:00:00Z",
  "period": { "start": "2026-10-17", "end": "2026-10-18", "timezone": "America/Toronto" },
  "qualityScenario": "complete",
  "summary": { "observationCount": 168, "seriesCount": 4, "invalidCount": 0, "duplicateCount": 0 },
  "series": [
    {
      "id": "demand", "label": "Demande électrique", "station": "Québec", "unit": "MW", "kind": "demand", "intervalMinutes": 15,
      "points": [{ "timestamp": "2026-10-17T04:00:00Z", "value": 25000.0 }],
      "minimum": 25000.0, "maximum": 25000.0, "latest": 25000.0,
      "coverage": { "observed": 96, "expected": 96, "percent": 100.0 }
    }
  ],
  "provenance": [{ "label": "Hydro-Québec", "url": "https://donnees.hydroquebec.com/", "license": "CC BY-NC 4.0", "retrievedAtUtc": "2026-10-18T12:00:00Z" }],
  "warnings": [],
    "artifacts": { "csv": "observations.csv", "parquet": "observations.parquet", "report": "report.json", "hourly": "hourly.csv" }
}
```

Les séries sont `demand`, `water_level`, `temperature_min`, `temperature_max`. Une valeur absente reste `null`. `coverage` peut être `null` au checkpoint de la démonstration avant l'implémentation. Les points sont triés chronologiquement. Les dates de période sont des journées civiles dans America/Toronto, fin exclusive. Les horodatages des points sont des instants UTC.

## Pilotage API .NET
- `GET /health` : état.
- `GET /api/runs` : tableau, ordre décroissant; objets `{id, source, status, stage, createdAtUtc, startedAtUtc, completedAtUtc, error, qualityScenario}`.
- `GET /api/runs/{id}` : un objet.
- `GET /api/runs/{id}/details` : `{run, retryOf, simulateFailure, report, execution}`. `report` appartient à cette exécution, pas au dernier succès. Métadonnées absentes des anciens traitements = `null`. `execution.steps` reprend les étapes mesurées par Python, y compris celles reçues en erreur.
- `POST /api/runs` corps `{source:"fixture|live",simulateFailure:false,qualityScenario:"complete|missing"}` : HTTP 202 `{id}`; 409 si une exécution est active.
- `POST /api/runs/{id}/retry` : HTTP 202 `{id}` pour une nouvelle exécution, mêmes paramètres avec simulateFailure=false.
- `GET /api/schedule` : `{enabled:false, intervalMinutes:5}`.
- `PUT /api/schedule` : même objet, intervalle entre 1 et 60 minutes.
- `GET /api/report` : rapport ci-dessus.
- `GET /api/artifacts/{runId}/{file}` : export observations.csv/observations.parquet/report.json/hourly.csv, chemin validé.

Statuts : `queued`, `running`, `succeeded`, `failed`, `interrupted`. Étapes : `queued`, `extract-transform-export`, `persist`, `complete`. L'historique est durable; après redémarrage les exécutions en cours sont marquées interrompues. La planification périodique est désactivée au premier démarrage.

## Environnement
Python : `HYDRO_OUTPUT_DIR`, `HYDRO_FIXTURE_DIR` facultatif. .NET : `Python__BaseUrl`, `Hydro__DataDir`. Vite : `API_BASE_URL` pour son proxy `/api`. Le script `start-demo.ps1` sélectionne un répertoire runtime distinct par branche/tag via `HYDRO_RUNTIME_DIR`.

En mode Aspire natif, les ports hôtes de Python et de l’API sont attribués automatiquement. Leurs adresses sont transmises par références d’endpoints (`Python__BaseUrl` vers .NET, `API_BASE_URL` vers Vite), sans numéro à recopier. Vue reste sur `http://localhost:53170`; le navigateur et `scripts/verify-http.ps1` accèdent à l’API par `/api` sur cette même adresse. Les URL directes du lancement courant figurent dans le dashboard Aspire.

Hors Aspire, fournir explicitement `Python__BaseUrl` à l’API et `API_BASE_URL` au serveur Vite. Une configuration absente est signalée; les anciens ports 53171/53172 ne servent plus de repli. En Compose, les ports internes Python 8001 et API 8080 et leur réseau privé restent inchangés.

## Analyse BI et traçabilité (ajout compatible, 19 octobre)

`schemaVersion: 1` reçoit trois champs optionnels pour que les anciens rapports restent lisibles :

- `etl` : `rawCount`, `normalizedCount`, `validCount`, `missingCount`, `scenarioRemovedCount`; `rawDemandSample` (quatre premières lignes de demande reçues, champs `date` et `valeurs_demandetotal`) et `normalizedDemandSample` (quatre premières lignes de demande triées après validation, champs `timestamp_utc`, `value`, `unit`). Les échantillons ne sont pas nécessairement en correspondance ligne par ligne lorsque la source contient des anomalies.
- `analysis` : `hourly`, `completeHours`, `finishedHours`, `indicators`. Chaque ligne horaire contient `startUtc`, `localHour` (ISO avec décalage), `observed`, `expected:4`, `meanDemandMW` ou `null`. Calculs détaillés dans DATA_DICTIONARY.md et `etl/hydrodatalab/analysis.py`.
- `execution.steps` : `{id,label,status,startedAtUtc,completedAtUtc,durationMs,error?}`. Étapes extract, normalize, analyze, export; arrêt au premier échec. Bilan disponible en fin de réponse Python, pas un flux de progression. Les durées excluent la publication finale JSON, le transport et SQLite; ne pas les additionner pour prétendre obtenir la durée totale API.

`artifacts.hourly` désigne `hourly.csv`, mêmes colonnes que `analysis.hourly`. Le répertoire entier est publié atomiquement; une relance HTTP avec le même UUID restitue le rapport déjà publié. Une nouvelle tentative via `/retry` crée un nouvel UUID et conserve `retryOf`.

Les erreurs Python structurées renvoient `detail: {message, execution}` (execution peut être null); l’API conserve les diagnostics d’échec dans SQLite. Les interruptions réseau n’inventent pas un bilan Python. Quartz reçoit un `JobExecutionException` après enregistrement de l’échec, sans réexécution immédiate; la relance reste explicite. La table additionnelle `run_details` préserve la table historique `runs`. Statut et diagnostics de fin sont enregistrés dans une même transaction.

Les nouveaux calculs n’appellent pas `coverage.calculate_coverage` : le jalon 06 garde son exercice Python et peut afficher les analyses avec la couverture « Non calculée ». Les données sources ne sont jamais réécrites.
