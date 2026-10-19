# HydroDataLab · préparer le portable HQ

**Présentation et trois démos · 20 octobre 2026 · préparation avant la séance**

Ce kit transporte les diapositives avec leurs notes, le PDF de secours, les guides, un dépôt Git et les quatre images Docker préparées. **Ouvrir ce guide HTML par double-clic après extraction du ZIP.** Les boutons Copier ne lancent aucune commande.

Le parcours de présentation reste : **Copilot dans VS Code → résultats vérifiables → modification Python guidée par une spécification**. Durée : 32 minutes d’explications, 13 de démos, 10 de questions et 5 de marge.

## 1. Comprendre ce qui est prêt

| Élément | Inclus dans le ZIP | À préparer sur le portable |
|---|---|---|
| Diaporama et notes synchronisées | Site déjà compilé, polices locales | Python et navigateur; aucun npm pour projeter |
| Secours | PDF, captures et conducteur des démos | Lecteur PDF |
| Dépôt Git pédagogique | Sources, données publiques, huit tags et branche d’exercice | Git, VS Code et dépendances de développement |
| Trois démos | Scripts, spécification, tests et solution | Deux copies locales à préparer ci-dessous |
| Docker | ETL, API, Vue et dashboard Aspire | Moteur Docker avec Compose, conteneurs Linux |
| Copilot | Instructions et prompts du projet | Compte HQ autorisé, connexion et modèles disponibles |

**Après préparation**, les slides, les données de démonstration et les traitements locaux/contenerisés fonctionnent sans Internet. Les demandes à Copilot et la source de données « live » conservent leurs besoins réseau. Les installateurs, SDK et caches de dépendances ne sont pas transportés.

Le ZIP n’inclut ni les anciennes sauvegardes, ni un second ZIP des sources, ni les dépendances du poste de création. La révision précise est dans [REVISION.json](REVISION.json). Les données et leur licence sont documentées séparément dans [data/README.md](data/README.md).

## 2. Installer ou vérifier les prérequis

Utiliser les logiciels et les canaux d’installation autorisés sur le portable HQ. Faire cette étape avec le réseau disponible, avant la répétition. Les liens ci-dessous sont les documentations officielles consultées le 19 octobre 2026; les versions visées viennent du projet préparé.

| Outil | Version ou configuration à viser | Utilité / documentation |
|---|---|---|
| PowerShell | **7 ou plus**, commande `pwsh` | [Installation Microsoft](https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell-on-windows). Éviter Windows PowerShell 5.1 pour ces scripts UTF-8. |
| Git | Git pour Windows | [Installation Git](https://git-scm.com/downloads/win). Restaurer le dépôt et l’exercice. |
| Python | **3.14.x**, testé en 3.14.2; projet compatible 3.12–3.14 | [Python Windows](https://www.python.org/downloads/windows/). Ne pas choisir 3.15 pour cet ETL. |
| uv | Version de préparation : **0.11.28** | [Installation uv](https://docs.astral.sh/uv/getting-started/installation/). Nécessaire sur le poste pour l’ETL, les tests et Aspire local. |
| .NET SDK | **10.0.402**, ou correctif compatible 10.0.4xx | [Téléchargement .NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0). Installer le SDK, pas seulement le runtime; `global.json` fixe la sélection. |
| Aspire CLI | **13.6.1**, comme l’AppHost | [Installation Aspire](https://aspire.dev/get-started/install-cli/). Aucun workload Aspire à installer. |
| Node.js et npm | **24 LTS** | [Téléchargement Node.js](https://nodejs.org/en/download). Pour le frontend de l’application. |
| Docker | Moteur opérationnel, Compose v2, conteneurs **Linux amd64** | [Installation Windows](https://docs.docker.com/desktop/setup/install/windows-install/). Les quatre images fournies sont Linux/amd64. |
| VS Code et Copilot | Version et fonctions retenues pour la séance; compte HQ connecté | [Installation VS Code](https://code.visualstudio.com/docs/setup/windows). Vérifier Agents, Customizations et le sélecteur de modèles. |

Un moteur Podman peut convenir s’il est déjà autorisé et vérifié avec son fournisseur Compose. Les commandes du guide utilisent Docker; ne pas changer de moteur pendant la prestation. L’installation du moteur peut nécessiter l’intervention des TI.

Après installation, ouvrir **un nouveau terminal PowerShell 7**. Ces commandes doivent répondre :

```powershell
$PSVersionTable.PSVersion
git --version
python --version
uv --version
dotnet --list-sdks
aspire --version
node --version
npm.cmd --version
docker version
docker compose version
```

**Attendu :** les outils sont trouvés; Docker affiche client et serveur. Si `python` ouvre le Microsoft Store, corriger sa résolution ou utiliser l’interpréteur installé, par exemple `py -3.14`, à la place de `python` dans ce guide. Le bundle ne remplace pas cette préparation.

## 3. Extraire et vérifier le kit

Copier **le ZIP et son fichier `.sha256` voisin** sur le portable. Comparer `Get-FileHash -Algorithm SHA256 <chemin-du-ZIP>` à la valeur du fichier `.sha256`, puis extraire entièrement le ZIP avec l’Explorateur. Ne pas travailler à l’intérieur de l’archive.

Choisir un dossier local autorisé, par exemple `C:\Formation\HydroDataLab-HQ`. Adapter ce chemin une seule fois ci-dessous; il doit contenir ce guide et `servir-presentation.py`. Prévoir aussi de l’espace pour les dépendances et les images importées : une marge de **10 Go libres** est raisonnable, sans être une mesure garantie de l’installation du moteur.

Dans le terminal de pilotage, conserver ces variables :

```powershell
$Kit = 'C:\Formation\HydroDataLab-HQ'
$Depot = Join-Path $Kit 'HydroDataLab'
$Live = Join-Path $Depot 'artifacts/repetition-live'
$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $Kit
python ./verifier-kit.py
if ($LASTEXITCODE -ne 0) { throw 'Kit incomplet ou modifie : verifier la copie avant de continuer.' }
```

**Attendu :** toutes les empreintes sont correctes. Ce contrôle lit les fichiers, sans installer de logiciel ni démarrer l’application. Les fichiers ajoutés plus tard, comme les deux copies de travail, ne l’empêchent pas de vérifier le contenu livré.

Si le terminal est fermé, rejouer le bloc des variables. `$Depot` est la solution de secours; `$Live` sera l’exercice. Ne pas reprendre le chemin `F:\PoC\...` du poste de création dans les autres guides.

## 4. Restaurer le dépôt principal

Exécuter une seule fois dans un dossier de destination inexistant :

```powershell
Set-Location -LiteralPath $Kit
if (Test-Path -LiteralPath $Depot) { throw 'Depot deja present : conserver son travail et reprendre a la preparation.' }
git clone --branch main ./developpement/HydroDataLab.bundle "$Depot"
if ($LASTEXITCODE -ne 0) { throw 'Restauration Git interrompue.' }
Set-Location -LiteralPath $Depot
git status --short --branch
git tag --list
./scripts/doctor.ps1 -WithDocker
dotnet --version
```

**Attendu :** branche `main`, aucun changement local; huit tags `00-socle` à `07-solution`; SDK compatible avec `global.json`. Si `doctor` signale un outil absent, revenir aux prérequis. Dans VS Code, ouvrir `$Depot`, se connecter à Copilot avec le compte HQ et vérifier les modèles autorisés. Une licence ou une fonction absente ne se corrige pas avec le bundle.

Le bundle contient aussi `codex/preparation-demo-06`, normalement visible après clonage sous `origin/codex/preparation-demo-06`. Ce dépôt n’a pas de dépôt GitHub distant : `origin` désigne le fichier de transport local.

## 5. Préparer l’application locale

Dans le dépôt principal, avec le réseau disponible :

```powershell
Set-Location -LiteralPath $Depot
./scripts/prepare.ps1 -SkipPresentation
./scripts/verify.ps1
```

**Attendu :** dépendances installées, tests Python/.NET/Vue et build Vue réussis. Arrêter ici si la préparation échoue. `-SkipPresentation` évite d’installer et de reconstruire Slidev : le site livré est déjà prêt à présenter.

NuGet, npm, PyPI et les téléchargements de runtimes doivent être accessibles par les mécanismes autorisés du poste. Faire résoudre les problèmes de proxy/certificats par la configuration habituelle HQ; ne pas désactiver les vérifications TLS. Aucun téléchargement ne doit être découvert pendant la séance.

## 6. Charger Docker et essayer l’ETL hors réseau

Démarrer le moteur en mode conteneurs Linux, puis :

```powershell
Set-Location -LiteralPath $Kit
docker info --format '{{.OSType}} / {{.Architecture}}'
docker load --input ./images/hydrodatalab-images.tar
if ($LASTEXITCODE -ne 0) { throw 'Chargement des images interrompu.' }
docker image inspect hydrodatalab-etl:formation --format '{{.Id}}'
docker run --rm --pull never --network none hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /tmp/result
if ($LASTEXITCODE -ne 0) { throw 'Essai ETL Docker en echec.' }
```

**Attendu :** moteur Linux sur architecture x86-64/amd64; images chargées; ETL avec **168 observations et 4 séries**, sans téléchargement. `--network none` coupe le réseau de ce conteneur, pas celui du portable. Les données sont déjà dans l’image Python.

Une seule archive est à charger : ETL, API, Vue et dashboard Aspire. Ne pas récupérer l’ancien complément typographique. Les images chargées appartiennent au moteur; il n’est pas nécessaire de recopier le gros TAR dans `$Depot` ou `$Live`.

## 7. Préparer la copie d’exercice

Cette copie garde l’interface et l’analyse ETL récentes, mais laisse le calcul de couverture à réaliser avec Copilot. Les tags historiques restent intacts.

```powershell
Set-Location -LiteralPath $Kit
if (Test-Path -LiteralPath $Live) { throw 'Exercice deja present : conserver ses modifications.' }
New-Item -ItemType Directory -Force (Split-Path -Parent $Live) | Out-Null
git clone --branch main ./developpement/HydroDataLab.bundle "$Live"
if ($LASTEXITCODE -ne 0) { throw 'Copie exercice interrompue.' }
git -C "$Live" switch -c codex/repetition-01 9e1ecde478f4f5d70634a42c720fa10fd224eab7
if ($LASTEXITCODE -ne 0) { throw 'Branche exercice non creee.' }
Set-Location -LiteralPath $Live
./scripts/prepare.ps1 -SkipPresentation
git status --short --branch
New-Item -ItemType Directory -Force etl/.test-output | Out-Null
uv run --project etl pytest specs/test_coverage_acceptance.py -q --basetemp etl/.test-output/avant
```

**Attendu :** `codex/repetition-01` et **deux tests d’acceptation en échec** (code de sortie 1), car le calcul renvoie encore `None`. Une erreur d’import, de dépendance ou de fixture n’est pas l’échec pédagogique attendu. Ne pas demander à Copilot de corriger ces deux tests avant la démo.

Ouvrir `$Live` dans une deuxième fenêtre VS Code; distinguer les fenêtres par leur chemin. Ne pas utiliser un ancien tag à la place de cette copie préparée. Ne pas lancer le `verify.ps1` complet de l’exercice en espérant un résultat vert avant l’implémentation.

## 8. Répéter les deux démarrages

Une seule application utilise **Vue sur le port 53170**. Aspire attribue automatiquement les ports API/Python. Avant ces essais, arrêter toute pile Compose lancée avec ce kit (section 11).

Premier démarrage de l’exercice :

```powershell
Set-Location -LiteralPath $Live
./scripts/start-demo.ps1
```

Ouvrir [HydroDataLab](http://localhost:53170). Source **Jeu de démonstration**, scénario de retrait décoché → **Lancer la collecte**. Attendre **Terminé** et vérifier **Couverture : Non calculée**. Noter le lien Aspire affiché dans le terminal.

Revenir au principal :

```powershell
Set-Location -LiteralPath $Live
./scripts/stop-demo.ps1
Set-Location -LiteralPath $Depot
./scripts/start-demo.ps1
./scripts/verify-http.ps1
```

**Attendu :** application principale saine. Faire une collecte complète, explorer **Parcours ETL**, vérifier **24 moyennes horaires**, puis **Voir les détails** d’une exécution. Garder la planification désactivée. Une collecte avec retrait d’une mesure donne **23/24 = 95,83 %** pour le niveau d’eau; le pourcentage global est différent.

Après ces deux premiers démarrages réussis, `./scripts/start-demo.ps1 -NoBuild` peut accélérer les transitions. Chronométrer principal → exercice → principal avant la séance. Le dashboard Aspire et ses adresses directes peuvent changer à chaque lancement.

## 9. Ouvrir la présentation sur deux écrans

Dans **un deuxième terminal**, depuis le dossier qui contient ce guide :

```powershell
Set-Location -LiteralPath 'C:\Formation\HydroDataLab-HQ'
python ./servir-presentation.py
```

Adapter ce chemin à celui choisi en section 3. Garder le terminal ouvert. Le serveur est lié à la boucle locale et ne nécessite ni npm, ni Astro, ni reconstruction.

- **Portable :** [vue présentateur](http://127.0.0.1:53173/presenter/1), notes et commandes.
- **Projecteur :** [vue audience](http://127.0.0.1:53173/1), plein écran `F11`.
- **Windows :** `Win + P` → **Étendre**, deux fenêtres du même navigateur et du même profil.

Garder **127.0.0.1:53173 dans les deux fenêtres**, sans mélanger avec `localhost`. Vérifier la synchronisation des couches de contexte (slide 10) et des coûts (16). Remettre première couche et 1 essai. Élargir et rehausser le panneau des notes; les précisions se déplient au clic.

Ouvrir aussi le [conducteur des trois démos](handouts/conducteur-demos.html) et le [PDF de secours](documents/formation-hydrodatalab.pdf). Le PDF sert au repli; il fige les interactions.

**Ce guide remplace la préparation initiale du conducteur pour ce kit.** Reprendre ensuite directement à **Démo 1**. Conserver les variables `$Depot` et `$Live` définies ici. Ne pas lancer `start-presentation.ps1` en plus : le serveur du kit occupe déjà 53173. Les slides restent indépendantes des changements de copie Git.

## 10. Le jour de la présentation

Rejouer les variables de la section 3 dans le terminal de pilotage, puis :

```powershell
Set-Location -LiteralPath $Depot
git status --short --branch
./scripts/start-demo.ps1 -NoBuild
```

Utiliser `-NoBuild` seulement après les essais réussis de la section 8. Démarrer le serveur des slides dans son autre terminal. Se connecter à Copilot avant l’arrivée du groupe.

| Moment | Manipulation | Support |
|---|---|---|
| Démo 1 · 4 min | Copilot en lecture seule dans `$Depot`, `main` | Slide 7, conducteur Démo 1 |
| Démo 2 · 5 min | CLI, ETL visuel, code/test, détails, ETL Docker | Slide 18, conducteur Démo 2 |
| Transition SDD · 2 min | Arrêter `$Depot`, lancer `$Live` avec `-NoBuild` | Slide 19 reste projetée |
| Démo 3 · 4 min | Nouvelle session Copilot dans `$Live`, calcul Python, tests, indicateur | Slide 20, conducteur Démo 3 |
| Secours à 2:10 de la démo 3 | Annoncer la solution préparée; arrêter `$Live`, relancer `$Depot` | Conducteur « Secours » |

Ne pas lancer Compose pendant ce parcours : il occuperait le même port Vue et ses images contiennent déjà la solution finale. Le petit conteneur ETL de la démo 2 ne publie aucun port et peut être exécuté pendant le mode local.

Avant de commencer, confirmer :

- Les deux vues de présentation sont synchronisées et les notes lisibles.
- Les deux dossiers démarrent; l’exercice affiche « Non calculée » et `main` fournit le secours.
- Copilot fonctionne avec le compte et un modèle autorisé; les prompts sont ouverts.
- Le test Docker `--network none` réussit; le PDF et les captures s’ouvrent hors ligne.
- La transition tient dans son créneau; notifications coupées et alimentation branchée.

Après une répétition, ne pas écraser les modifications Copilot de `$Live`. Utiliser la section **Fin et nouvelle répétition** du conducteur pour une nouvelle tentative. `PROGRESS.md` du dépôt et l’état Git donnent les repères après une interruption.

## 11. Secours : application complète dans Docker

Ce parcours est facultatif et sert à montrer l’application finale quand le parcours natif est indisponible. Il ne remplace pas l’exercice de modification Python en direct. Avant de le lancer, arrêter l’application native depuis **la copie actuellement active** :

```powershell
# Choisir $Depot ou $Live selon la copie active.
Set-Location -LiteralPath $Depot
./scripts/stop-demo.ps1
Set-Location -LiteralPath $Kit
docker compose -p hydrodatalab-training --env-file compose/images.env -f compose/docker-compose.yaml up -d --pull never --no-build --wait --wait-timeout 90
if ($LASTEXITCODE -ne 0) { throw 'Demarrage Compose en echec.' }
```

Ouvrir [Vue](http://localhost:53170) et le [dashboard Compose](http://localhost:53174). Le `--wait` confirme l’état démarré des services configurés; vérifier aussi la page et une collecte complète. Choisir **Jeu de démonstration** pour rester indépendant du réseau. Pour consulter les journaux :

```powershell
Set-Location -LiteralPath $Kit
docker compose -p hydrodatalab-training --env-file compose/images.env -f compose/docker-compose.yaml logs --tail 80
```

Arrêter en conservant les volumes, avant de reprendre le mode natif :

```powershell
Set-Location -LiteralPath $Kit
docker compose -p hydrodatalab-training --env-file compose/images.env -f compose/docker-compose.yaml stop --timeout 30
```

## 12. Résoudre les blocages courants

| Symptôme | Action |
|---|---|
| Accents `prÃ©...` | Ouvrir `pwsh` (PowerShell 7), pas `powershell.exe` 5.1. |
| Script refusé par la politique HQ | Faire approuver l’exécution selon les mécanismes internes. Ce guide ne demande pas de contournement. |
| `dotnet --version` échoue dans le dépôt | Installer le SDK demandé par `global.json`; une autre bande 10.0.xxx peut ne pas convenir. |
| Restauration uv/npm/NuGet impossible | Traiter les accès réseau/proxy/certificats avant la séance; ne pas improviser une installation en direct. |
| Python 3.15 sélectionné pour l’ETL | Le projet attend Python 3.12 à 3.14; utiliser l’interpréteur compatible préparé. |
| Vue 53170 déjà occupé | Arrêter la pile HydroDataLab active avec sa commande de fin; une seule pile à la fois. |
| Slides 53173 déjà servies | Réutiliser ce serveur ou arrêter son terminal avec Ctrl+C. Ne pas lancer deux serveurs. |
| Les vues ne se synchronisent pas | Même navigateur/profil, même hôte et même port, puis recharger les deux fenêtres. |
| Docker ne trouve pas l’image | Charger le TAR dans le moteur/contexte réellement utilisé; contrôler avec `docker image inspect`. |
| Portable ARM64 | Les images fournies ciblent amd64; vérifier leur exécution avant la séance ou utiliser le parcours local et les captures. |
| Copilot indisponible | Utiliser la solution préparée en l’annonçant; les traitements fixture et les slides restent disponibles. |

La validation technique du ZIP ne remplace pas la répétition sur le portable HQ : accès Copilot, politiques du poste et projecteur doivent encore être confirmés sur place.
