# Moteur ETL .NET

Alternative au paquet `etl/hydrodatalab`, sélectionnable par Aspire avec `Hydro:EtlEngine=dotnet` (variable `Hydro__EtlEngine`). L'API de pilotage conserve Quartz et SQLite; ce service effectue les calculs. Le contrat HTTP, les UUID, les scénarios et les noms d'exports sont communs aux deux moteurs.

## Préparer et lancer

Depuis la racine, PowerShell 7 :

```powershell
./scripts/prepare.ps1 -EtlEngine dotnet -SkipPresentation
./scripts/verify.ps1 -EtlEngine dotnet
./scripts/start-demo.ps1 -EtlEngine dotnet
```

Cette préparation ne consulte ni PyPI ni les distributions CPython. Elle restaure les paquets .NET depuis les sources NuGet configurées sur le poste et prépare Vue via npm. Les dépendances, dont Parquet.Net et ses dépendances transitives, sont verrouillées. Aucun `NuGet.config` spécifique ni contournement TLS n'est ajouté.

Aspire ne crée qu'un service de calcul : `etl-dotnet` ou `python`. Il transmet son endpoint à l'API via `Etl__BaseUrl`. La CLI peut aussi recevoir `aspire run --apphost src/AppHost/apphost.cs -- --Hydro:EtlEngine=dotnet`. `Hydro:EtlEngine` est une clé de configuration .NET standard; les scripts proposent le paramètre `-EtlEngine`, puis la variable d'environnement, puis `python` par défaut. Le choix se fait au démarrage, pas pendant un traitement.

Les résultats natifs .NET vont dans `<HYDRO_RUNTIME_DIR>/dotnet/`, ou `.runtime/main/dotnet/` sans cette variable. Les scripts utilisent la branche Git comme racine runtime. Cette isolation conserve un historique et une planification indépendants pour chaque moteur.

## CLI sans Aspire

```powershell
dotnet restore src/Etl --locked-mode
dotnet run --project src/Etl -- run --source fixture --date 2026-10-17 --output outputs/dotnet-cli
dotnet run --project src/Etl -- run --source fixture --quality-scenario missing --output outputs/dotnet-cli
dotnet run --project src/Etl -- extract --source fixture --output outputs/dotnet-snapshot
dotnet run --project src/Etl -- --help
```

`run` accepte aussi `--run-id UUID` et `--simulate-failure`. `extract` exige `--output`. `--source live` consulte Hydro-Québec; `--date` doit désigner une journée terminée à Toronto, sinon la veille est choisie. Sans sous-commande, l'exécutable démarre le service HTTP ASP.NET Core; utiliser `--urls http://localhost:8002` pour un lancement autonome.

`HYDRO_FIXTURE_DIR` et `HYDRO_OUTPUT_DIR` remplacent les dossiers par défaut. Un snapshot extrait comprend les deux fichiers bruts et un manifeste SHA-256 compatible avec Python. Les fixtures du dépôt ne sont jamais modifiées par le scénario manquant.

## Équivalence et validation

Le cœur conserve les valeurs absentes, les zéros, le dernier doublon, les unités, les offsets UTC et les journées civiles de 23/25 heures. Seules quatre mesures valides donnent une moyenne horaire. Une journée incomplète ne donne pas d'estimation d'énergie. La publication des quatre exports est atomique; rejouer un UUID restitue le rapport existant et refuse des paramètres différents.

```powershell
dotnet test tests/Etl.Tests
# Facultatif pour les développeurs disposant des deux moteurs :
dotnet build src/Etl
uv run --project etl --frozen python scripts/verify-etl-parity.py
```

La comparaison couvre les scénarios complet/manquant, les anomalies et deux changements d'heure. Elle compare les rapports en excluant les horodatages et durées d'exécution, les CSV octet par octet et les types logiques/valeurs des Parquet. Les métadonnées propres à pandas et à Parquet.Net ainsi que les octets compressés des Parquet peuvent différer. La suite .NET et les parcours HTTP ne nécessitent pas Python.

## Réseau et conteneurs

Le mode `fixture` n'effectue aucune requête vers Hydro-Québec. `live` utilise `HttpClient`, avec trois tentatives au maximum pour les pannes transitoires et un délai de 20 secondes par requête. Il ne remplace jamais une erreur par une fixture. L'accès au miroir NuGet ne garantit pas à lui seul l'accès HTTPS aux données publiques; les certificats et règles réseau du poste restent applicables.

`src/Etl/Dockerfile` utilise les images SDK/ASP.NET Core .NET 10, copie les fixtures et expose le port interne 8080. La branche publication de l'AppHost sélectionne ce Dockerfile avec le même paramètre. Les scripts historiques `prepare-docker.ps1`, `export-images.ps1` et les kits transportables restent dédiés aux images Python déjà préparées. Une archive de ces images ne contient pas le nouveau moteur .NET. Aucun déploiement cloud n'est nécessaire.
