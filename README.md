# HydroDataLab

Formation « Des agents IA aux pipelines Python fiables » — Hydro-Québec, 20 octobre 2026.

Projet pédagogique local : données publiques, ETL Python, API ASP.NET Core, Vue, Quartz et Aspire.

Le support contient **22 diapositives + 6 annexes**, trois démonstrations (4 + 5 + 4 minutes), les notes et un PDF. Le pipeline utilise les données publiques du **17 octobre 2026** : 96 demandes électriques, 24 niveaux d'eau et 48 températures. Les stations Gouin Nord et Gouin Bois sont distinctes. Un niveau en mètres ne représente pas une réserve en GWh.

## Commencer

Dans **PowerShell 7**, à la racine du dossier :

```powershell
./scripts/doctor.ps1
./scripts/prepare.ps1
./scripts/verify.ps1
./scripts/start-demo.ps1
```

Ouvrir **http://localhost:53170**, puis « Lancer le traitement ». L'historique et les exports sont conservés dans `.runtime/main/`. Aspire affiche son propre lien de tableau de bord au démarrage. Le mode natif ne demande pas Docker. Pour arrêter : `./scripts/stop-demo.ps1`.

Dans un deuxième terminal :

```powershell
./scripts/start-presentation.ps1
```

Ouvrir **http://localhost:53173**; ajouter `/presenter` pour les notes. Le [PDF de secours](presentation/exports/formation-hydrodatalab.pdf) fonctionne sans serveur. Les polices et les éléments visuels sont locaux.

## Le cœur Python, sans infrastructure

```powershell
uv sync --project etl --frozen
uv run --project etl python -m hydrodatalab run --source fixture --date 2026-10-17 --output outputs/cli
New-Item -ItemType Directory -Force etl/.test-output | Out-Null
uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/manual
```

`--source live` consulte explicitement Hydro-Québec; `--date` sélectionne une journée locale terminée. Sans date, live utilise la veille à Toronto. Le mode fixture refuse une autre date que celle de son manifeste. Il ne remplace jamais silencieusement une panne réseau par un échantillon. Voir [etl/README.md](etl/README.md).

## Application et conteneurs

| Composant | Responsabilité | Version figée |
|---|---|---|
| Python / pandas / pytest | Extraire, valider, transformer, exporter | Versions exactes dans `etl/uv.lock` |
| FastAPI | Adapter le même cœur Python à HTTP | `etl/uv.lock` |
| ASP.NET Core | Consulter les résultats, piloter les tâches | .NET SDK 10.0.402 / runtime 10.0.13 |
| Quartz + SQLite | Planifier et conserver l'historique | Quartz 4.5.1 |
| Vue / TypeScript | Explorer les séries et leur qualité | `web/package-lock.json` |
| Aspire | Démarrer les composants et observer l'application | 13.6.1 |
| Slidev | Présenter la formation et exporter le PDF | `presentation/package-lock.json` |

Préférer Node **24 LTS** sur le portable de présentation. Python 3.14.2 est testé; le projet accepte 3.12 à 3.14. Installer Git, .NET, uv, Node et Aspire avant `prepare.ps1`. Les téléchargements sont nécessaires à la préparation initiale. Après préparation, les parcours fixture et le diaporama sont autonomes.

La publication part de `src/AppHost/apphost.cs`. Les scripts `prepare-docker.ps1` et `docker-demo.ps1` utilisent Compose généré par Aspire. Voir [docs/DOCKER.md](docs/DOCKER.md). Les images doivent être construites avant la séance. Les ports API/Python restent privés en conteneurs; Vue les utilise via nginx.

## Restaurer les conteneurs sans réseau

L'archive préparée le **18 octobre 2026**, [artifacts/hydrodatalab-images.tar](artifacts/hydrodatalab-images.tar), contient les **quatre images déjà testées** : Python, API, Vue et dashboard Aspire. Taille : **1 205 382 656 octets** (~1 149,5 Mio). Son SHA-256 est :

```text
3c3c775373607b6f6cec5f2bdea7cc9506bfe4c5182e9df04bfce8ea4690ebfe
```

Transporter le dossier du projet, l'archive et ses fichiers `.sha256` / `.manifest.json`, ainsi que **tout `artifacts/compose/`**. Ce dernier contient `docker-compose.yaml`, `images.env` et le `.env` généré par Aspire. **Le bundle Git et le ZIP du code ne contiennent pas ces artefacts**, car `artifacts/` est ignoré par Git; ils doivent être ajoutés au kit portable séparément. Les scripts de lancement requièrent `docker-compose.yaml` et `images.env` à leur emplacement d'origine.

Sur le poste cible, avec le moteur installé et démarré, vérifier l'empreinte puis charger les images **avant** le lancement hors ligne :

```powershell
Get-FileHash artifacts/hydrodatalab-images.tar -Algorithm SHA256
docker load --input artifacts/hydrodatalab-images.tar
./scripts/docker-demo.ps1
```

Avec Podman, utiliser `podman load --input artifacts/hydrodatalab-images.tar`, puis `./scripts/docker-demo.ps1 -Engine podman`. Le lancement impose `--pull never --no-build`. La [procédure détaillée](docs/DOCKER.md#transporter-et-restaurer-les-images-avant-la-séance) explique aussi comment republier Compose sur un poste préparé si ses fichiers n'ont pas été transportés. Une nouvelle archive peut être produite avec `./scripts/export-images.ps1 -Engine podman` ou `-Engine docker`.

## Présenter et reprendre

Le **kit de transfert hors ligne** est assemblé par `./scripts/package-kit.ps1` dans `artifacts/kit-hq-<date-heure>/`, après préparation et depuis un dépôt propre. Il rassemble les sauvegardes Git, les images, Compose, le diaporama statique, le PDF et les guides. Ouvrir son `DEMARRER.md` : la présentation peut être servie avec Python seul, sans npm. Copier ce dossier complet sur le portable et sur un autre support avant la séance.

- [Guide du présentateur minuté](handouts/guide-presentateur.md), [requêtes Copilot](handouts/prompts.md), [aide-mémoire](handouts/aide-memoire.md), [guide participant](handouts/guide-participant.md).
- [Conducteur des trois démos](handouts/conducteur-demos.html) : lecture sans serveur, commandes et prompts à copier, passages entre les versions, résultats attendus et secours. [Source éditable](handouts/conducteur-demos.md); régénérer la page avec `node presentation/scripts/build-demo-guide.mjs`.
- [Matrice des modèles datée](handouts/matrice-modeles.md) et [sources officielles](docs/SOURCES.md). Confirmer le sélecteur et les politiques HQ le 19 octobre.
- [Personnalisations VS Code 1.142](handouts/personnalisations-vscode.md) : formats, portées, AGENTS.md, sept concepts, panneau Instructions et Canvas. Les références des exemples sont cliquables dans le site et le PDF.
- [Jalons Git et démonstration en direct](docs/CHECKPOINTS.md).
- [Journal de reprise](PROGRESS.md), `./scripts/resume.ps1` et `./scripts/backup.ps1`.
- [Captures de secours](docs/screenshots/) et [contrat des données](docs/DATA_DICTIONARY.md).

Les traitements sont rejouables; un UUID publié conserve ses résultats. Les exports sont publiés par renommage atomique. L'API refuse un deuxième traitement actif. Un redémarrage marque les anciennes exécutions actives comme interrompues; la relance est explicite. La planification est désactivée par défaut et conservée dans SQLite.

Ce démonstrateur local n'est pas un service de production : aucune authentification utilisateur ni gestion multi-instance. La politique interne HQ et l'accès aux modèles doivent être vérifiés sur le poste du présentateur. La répétition du 19 octobre reste une action à réaliser sur ce poste.

## Conditions d'utilisation

Le code pédagogique et la documentation sont sous [licence MIT](LICENSE). Les données **ne sont pas couvertes par cette licence** : attribution Hydro-Québec et **CC BY-NC 4.0**, selon les jeux sources; détails et empreintes dans [data/README.md](data/README.md) et `data/fixtures/manifest.json`. Les dépendances conservent leurs propres licences. Aucun logo ni approbation officielle Hydro-Québec n'est revendiqué.
