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
  "artifacts": { "csv": "observations.csv", "parquet": "observations.parquet", "report": "report.json" }
}
```

Les séries sont `demand`, `water_level`, `temperature_min`, `temperature_max`. Une valeur absente reste `null`. `coverage` peut être `null` au checkpoint de la démonstration avant l'implémentation. Les points sont triés chronologiquement. Les dates de période sont des journées civiles dans America/Toronto, fin exclusive. Les horodatages des points sont des instants UTC.

## Pilotage API .NET
- `GET /health` : état.
- `GET /api/runs` : tableau, ordre décroissant; objets `{id, source, status, stage, createdAtUtc, startedAtUtc, completedAtUtc, error, qualityScenario}`.
- `GET /api/runs/{id}` : un objet.
- `POST /api/runs` corps `{source:"fixture|live",simulateFailure:false,qualityScenario:"complete|missing"}` : HTTP 202 `{id}`; 409 si une exécution est active.
- `POST /api/runs/{id}/retry` : HTTP 202 `{id}` pour une nouvelle exécution, mêmes paramètres avec simulateFailure=false.
- `GET /api/schedule` : `{enabled:false, intervalMinutes:5}`.
- `PUT /api/schedule` : même objet, intervalle entre 1 et 60 minutes.
- `GET /api/report` : rapport ci-dessus.
- `GET /api/artifacts/{runId}/{file}` : export csv/parquet/report.json, chemin validé.

Statuts : `queued`, `running`, `succeeded`, `failed`, `interrupted`. Étapes : `queued`, `extract-transform-export`, `persist`, `complete`. L'historique est durable; après redémarrage les exécutions en cours sont marquées interrompues. La planification périodique est désactivée au premier démarrage.

## Environnement
Python : `HYDRO_OUTPUT_DIR`, `HYDRO_FIXTURE_DIR` facultatif. .NET : `Python__BaseUrl`, `Hydro__DataDir`. Vite : `API_BASE_URL` pour son proxy `/api`. Le script `start-demo.ps1` sélectionne un répertoire runtime distinct par branche/tag via `HYDRO_RUNTIME_DIR`.

En mode Aspire natif, les ports hôtes de Python et de l’API sont attribués automatiquement. Leurs adresses sont transmises par références d’endpoints (`Python__BaseUrl` vers .NET, `API_BASE_URL` vers Vite), sans numéro à recopier. Vue reste sur `http://localhost:53170`; le navigateur et `scripts/verify-http.ps1` accèdent à l’API par `/api` sur cette même adresse. Les URL directes du lancement courant figurent dans le dashboard Aspire.

Hors Aspire, fournir explicitement `Python__BaseUrl` à l’API et `API_BASE_URL` au serveur Vite. Une configuration absente est signalée; les anciens ports 53171/53172 ne servent plus de repli. En Compose, les ports internes Python 8001 et API 8080 et leur réseau privé restent inchangés.
