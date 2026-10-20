# HydroDataLab · les trois démos

**Conducteur privé du présentateur · 20 octobre 2026 · 4 + 5 + 4 minutes**

Ouvrir [la version lisible dans le navigateur](conducteur-demos.html) sur le portable. Elle fonctionne par double-clic, sans serveur, et propose des boutons Copier. Le public voit VS Code, HydroDataLab ou les diapositives, jamais ce conducteur.

## Repères à garder sous les yeux

**Sujet de la formation : GitHub Copilot dans VS Code et ses bonnes pratiques.** HydroDataLab rend visibles les résultats à vérifier. Démo 1 : 09:00–13:00; démo 2 : 29:30–34:30; transition SDD : 34:30–36:30; démo 3 : 36:30–40:30. Le parcours se termine à 45:00, puis questions et marge. Contexte interactif : slide 11; coûts : slide 17.

| Moment | Dossier / version | À montrer | Durée |
|---|---|---|---:|
| Démo 1 · slide 7 | Dépôt principal, `main` | Une mission en lecture seule, ses instructions et ses preuves | 4 min |
| Démo 2 · slide 20 | Dépôt principal, `main` | Exécution → avant/après → code/test → détails → conteneur | 5 min |
| Transition · slide 21 | Arrêter `main`, démarrer la copie d’exercice | Garder la slide SDD projetée pendant la manipulation | Dans les 2 min de SDD |
| Démo 3 · slide 22 | Copie d’exercice, branche `codex/repetition-01` issue de `codex/preparation-demo-06` | Règle BI → code Python → tests → 100 %, puis 95,83 % | 4 min |
| Secours, si nécessaire | Retour au dépôt principal, `main` | Solution préparée, annoncée comme telle | Décider à 2:10 de la démo 3 |

**Pourquoi deux dossiers?** Le dépôt principal garde les diapositives récentes et l’application complète. La copie permet de modifier Python sans toucher au secours. `main` conserve la solution métier du jalon 07 et ajoute les améliorations de préparation, dont les ports dynamiques. La branche préparée `codex/preparation-demo-06`, issue du jalon 06, intègre les ports dynamiques, la typographie et le parcours ETL détaillé, sans implémenter la couverture. Le commit préparé `9e1ecde` est transporté dans le bundle; aucune résolution de conflit ou application manuelle de commits n’est nécessaire. Les changements de version sont préparés avant la séance; pendant la présentation, on change de dossier et d’application active.

Un **tag** est un jalon fixe; une **branche** conserve une nouvelle tentative. La branche existante `demo-live` n’est pas nécessaire à ce parcours. **Une seule application à la fois** utilise le port fixe Vue 53170; les ports API/Python sont attribués par Aspire. Le serveur des diapositives, sur 53173, reste lancé depuis le dépôt principal. La typographie Vue utilise un corps de 18 px et des libellés de 16 px : faire défiler la page pour montrer chaque zone, plutôt que réduire le zoom pour tout faire tenir.

## Avant la séance · préparer une fois

### A. Ouvrir un terminal PowerShell 7 de pilotage

Utiliser **PowerShell 7 (`pwsh`)**, comme prévu dans le README. Windows PowerShell 5.1 (`powershell.exe`) peut afficher les accents comme `prÃ©requis` en lisant nos scripts UTF-8 sans BOM. Si nécessaire, taper `pwsh` pour ouvrir la version 7, puis exécuter le bloc ci-dessous. Sur le portable HQ, vérifier sa présence avant la séance. [Explication Microsoft sur l’encodage](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_character_encoding?view=powershell-5.1).

Garder ce terminal ouvert pendant la répétition. Adapter uniquement le chemin du dépôt si le projet est ailleurs sur le portable HQ. Rejouer ce bloc si le terminal a été fermé : les variables PowerShell ne survivent pas à sa fermeture.

```powershell
$Depot = 'F:\PoC\Formation Copilot - HQ'
$Live = Join-Path $Depot 'artifacts/repetition-live'
Set-Location -LiteralPath $Depot
$PSVersionTable.PSVersion
git status --short --branch
```

**Attendu :** `## main`, sans fichiers modifiés. Si des changements apparaissent, les examiner et les enregistrer avant une bascule. Ce guide ne demande aucun `reset --hard`. S’il s’agit d’une autre branche propre, `git switch main` revient à la version de présentation.

```powershell
./scripts/doctor.ps1
```

**Attendu :** PowerShell 7 ou plus récent, Git, .NET, Aspire, Node, npm et uv trouvés. Ce diagnostic ne teste pas la connexion Copilot. Vérifier celle-ci dans VS Code, avec le compte et les modèles autorisés sur le poste HQ.

**Où faut-il uv?** Sur le poste pour la CLI Python, les tests et l’application Aspire en processus locaux : les trois démos utilisent ce parcours. L’image `hydrodatalab-etl:formation` embarque aussi uv et ses dépendances déjà installées; son exécution seule ou la pile Compose déjà préparée ne demande pas uv sur le poste. L’exécution conteneurisée lance Python/Uvicorn directement. Aspire ne conteneurise pas automatiquement les programmes du parcours local. Voir `etl/Dockerfile` dans le dépôt et la [documentation uv/Docker](https://docs.astral.sh/uv/guides/integration/docker/). Dans le kit, le Dockerfile se trouve dans le code à restaurer depuis le bundle.

**Quels ports utiliser?** Vue garde **http://localhost:53170**. Aspire choisit les ports de l’API et de Python et transmet leurs adresses automatiquement : aucune modification manuelle de configuration après un redémarrage. Pour consulter les URL directes, ouvrir les endpoints **api** et **python** dans le dashboard du lancement courant. Pour consulter l’API depuis le navigateur, utiliser **http://localhost:53170/api/report**; `./scripts/verify-http.ps1` passe également par Vue. Le diaporama indépendant reste sur 53173. En Compose, seuls Vue (53170) et le dashboard (53174) sont publiés sur le poste; les ports internes API/Python restent privés et inchangés. [Documentation réseau Aspire](https://aspire.dev/fundamentals/networking-overview/).

Sur un nouveau poste, installer les dépendances avant la répétition, avec le réseau disponible. Arrêter d’abord l’application native avec `./scripts/stop-demo.ps1` si elle tourne. Si Compose tourne, utiliser `./scripts/docker-demo.ps1 -Action Stop` avec les options de moteur employées au démarrage. Puis :

```powershell
Set-Location -LiteralPath $Depot
./scripts/prepare.ps1
./scripts/verify.ps1
```

**Attendu sur la version complète :** validations réussies. Ne pas réinstaller les dépendances pendant la séance. Docker est facultatif pour l’application locale.

### B. Créer la copie d’exercice, sans copier les grosses archives

Exécuter ce bloc **une seule fois**. Si le dossier existe déjà, il reste intact : passer à « Fin et nouvelle répétition ». Un bundle est un fichier de transport Git local; il évite les problèmes de propriété Windows du clonage direct d’un dossier. Il contient l’historique et les sources, pas les images Docker ni les dépendances installées. Aucun dépôt en ligne n’est créé. Le fichier `.runtime/repetition-source.bundle` sert au transfert et sera actualisé lors d’une nouvelle répétition.

```powershell
Set-Location -LiteralPath $Depot
if (Test-Path -LiteralPath $Live) { throw 'Copie existante : la conserver et reprendre la section Fin et nouvelle répétition.' }
New-Item -ItemType Directory -Force .runtime | Out-Null
git bundle create .runtime/repetition-source.bundle --all
if ($LASTEXITCODE -ne 0) { throw 'Création du bundle interrompue.' }
git clone --branch main .runtime/repetition-source.bundle "$Live"
if ($LASTEXITCODE -ne 0) { throw 'Clonage interrompu.' }
git -C "$Live" switch -c codex/repetition-01 9e1ecde478f4f5d70634a42c720fa10fd224eab7
if ($LASTEXITCODE -ne 0) { throw 'Branche exercice non créée.' }
Set-Location -LiteralPath $Live
./scripts/prepare.ps1 -SkipPresentation
git status --short --branch
```

**Attendu :** branche `codex/repetition-01`, dossier propre. La branche part d’une préparation déjà validée issue de 06 : ports, typographie, parcours ETL et détails d’exécution. La couverture reste à implémenter. Aucun cherry-pick n’est nécessaire. Les huit tags restent intacts. Ouvrir cette copie dans une deuxième fenêtre VS Code par **Fichier → Nouvelle fenêtre → Ouvrir un dossier**. Garder aussi la fenêtre du dépôt principal. Identifier les deux fenêtres par leur chemin, même si leur nom de projet ressemble au même nom.

Contrôler l’état de départ de l’exercice :

```powershell
Set-Location -LiteralPath $Live
New-Item -ItemType Directory -Force etl/.test-output | Out-Null
uv run --project etl pytest specs/test_coverage_acceptance.py -q --basetemp etl/.test-output/avant
```

**Attendu : deux tests en échec**, parce que `calculate_coverage` renvoie encore `None`. Les dépendances fonctionnent; la règle BI reste à implémenter. Une erreur d’import ou une fixture absente n’est pas cet échec attendu.

### C. Répéter les démarrages avant de chronométrer

Arrêter d’abord la version principale si elle tourne. Puis démarrer l’exercice :

```powershell
Set-Location -LiteralPath $Live
./scripts/start-demo.ps1
```

Ouvrir **http://localhost:53170**. Choisir **Jeu de démonstration**, laisser **Retirer une mesure de niveau d’eau** décoché, cliquer **Lancer la collecte**. Attendre **Terminé** et vérifier **Couverture : Non calculée**. Cette vérification prépare aussi la compilation nécessaire à un prochain lancement rapide.

Revenir à la version complète pour le début de la répétition :

```powershell
Set-Location -LiteralPath $Live
./scripts/stop-demo.ps1
Set-Location -LiteralPath $Depot
./scripts/start-demo.ps1
```

Le script attend que Vue soit prêt. Recharger la page et lancer une collecte complète; garder **Qualité par série** replié pour la démo 2. Ouvrir le lien du **dashboard Aspire** donné par le terminal. Son adresse peut changer entre les deux applications : conserver le lien de chaque lancement. Vérifier que la planification reste désactivée.

**Chronométrer maintenant** le passage principal → exercice et le retour. Les prochains démarrages peuvent employer `-NoBuild`, uniquement après ce premier démarrage réussi dans chaque dossier. Si la transition dépasse la plage disponible, privilégier le parcours préparé et annoncer le secours; ne pas découvrir un téléchargement en séance.

### D. Préparer le passage en conteneur

Dans le terminal de pilotage, tester le moteur autorisé et l’image déjà chargée :

```powershell
docker version
docker image inspect hydrodatalab-etl:formation --format '{{.Id}}'
docker run --rm --pull never --network none hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /tmp/result
```

**Attendu :** résumé avec 168 observations, 4 séries, aucun téléchargement. `--network none` concerne ce conteneur; il prouve que cet ETL utilise la copie locale. L’archive d’images et sa restauration sont décrites dans [DOCKER](../docs/DOCKER.md). Si le poste utilise Podman, remplacer `docker` par `podman`. Avec un contexte Docker explicite, ajouter `--context NOM_VALIDE` après `docker` à chaque commande, sans changer le contexte global.

Si le moteur ne répond pas, garder le parcours Python natif et la [capture Compose préparée](../docs/screenshots/08-compose.png). Ne pas lancer la pile Compose pendant ces démos : elle occuperait les mêmes ports et ses images contiennent déjà la solution finale.

### E. Préparer les écrans et les onglets

Dans **un deuxième terminal**, depuis le dépôt principal, lancer le diaporama si 53173 n’est pas déjà servi :

```powershell
Set-Location -LiteralPath 'F:\PoC\Formation Copilot - HQ'
./scripts/start-presentation.ps1
```

Garder ce terminal ouvert. `Win + P` → **Étendre**. Même navigateur, même profil, mêmes adresses pour la synchronisation :

- Portable : **http://127.0.0.1:53173/presenter/1**, plus ce conducteur.
- Projecteur : **http://127.0.0.1:53173/1**, plein écran `F11`.
- Démonstration : **http://localhost:53170**, dashboard Aspire et deux fenêtres VS Code déjà ouvertes.

Déplacer la fenêtre à montrer sur le projecteur; garder le conducteur et les notes sur le portable. Couper les notifications. Les étapes 1 et 2 se déroulent dans le dépôt principal. La transition vers l’exercice aura lieu pendant la slide 21. Préparer les commandes et les prompts ci-dessous dans des onglets; ne pas les retaper devant le public.

## Démo 1 · cadrer une mission · 4 min

**Slide 7 → VS Code du dépôt principal (`main`). Question au public : « Quelles sources permettent de vérifier sa réponse? »**

### 0:00–0:45 · Où travaille l’agent?

**Faire :** ouvrir la fenêtre Agents via la palette de commandes de VS Code; utiliser **Chat: Open Agents Window** si ce libellé est disponible dans la version installée. Vérifier le dossier HydroDataLab, la branche `main`, le moteur Copilot et le modèle autorisé préparé, généralement Auto Balance. Créer une session pour cette tâche et garder les confirmations d’actions. Repérer `/plan` pour une future modification, sans lancer une deuxième demande ici.

**Dire :** « Une session porte une tâche. Le dossier indique les fichiers accessibles; la branche indique la version du travail. » Ne pas créer de worktree ici : la notion a déjà été expliquée en slide 6.

### 0:45–1:30 · Des réglages qui ont un but

**Faire :** montrer la portée Utilisateur/Espace de travail dans les paramètres (`Ctrl+,`), le zoom préparé pour la projection et le panneau **Changes**.

**Dire :** « Mes préférences de lecture sont personnelles; les règles du projet sont partagées dans Git. Changes permettra d’examiner ce qui a changé. » Le panneau peut être vide : la première mission ne modifie rien. Le vrai diff sera montré à la démo 3.

### 1:30–2:20 · Une règle vérifiable

**Faire :** ouvrir **Customizations** et passer directement à **Instructions**, puis montrer `.github/copilot-instructions.md` et `.github/instructions/python.instructions.md`. Retrouver la règle des valeurs absentes et des zéros. Le tour des rubriques viendra à la slide 12. Si l’interface ne liste pas un fichier, ouvrir le fichier et vérifier le chargement réel dans les références/diagnostics; ne pas affirmer qu’il a été chargé uniquement parce qu’il existe.

**Dire :** « Cette consigne distingue une mesure égale à zéro d’une mesure absente. Nous allons illustrer son effet juste après la démo. » Réserver le calcul sur `[10, 0, None]` à la slide 8 et le format du fichier à la slide 9.

### 2:20–4:00 · Demander, puis contrôler une preuve

**Copier dans Copilot :**

```text
Repère le calcul des indicateurs et explique comment il est testé.
Ne modifie aucun fichier. Suis le chemin des données locales préparées
dans data/fixtures jusqu’au rapport. Lis docs/CONTRACT.md et les instructions pertinentes.
Identifie le fichier Python de transformation et les tests à utiliser.
Cite les fichiers consultés et signale toute information manquante.
Réponse attendue : un schéma en quatre étapes et les commandes de vérification.
```

**Montrer :** les références/fichiers réellement consultés, puis une fonction ou un test cité. **Dire :** « Je peux vérifier cette affirmation dans le code. » Revenir aux slides. Si la réponse tarde ou que Copilot est indisponible, ouvrir `etl/hydrodatalab/pipeline.py` et un test local, en disant que la génération n’a pas abouti en direct.

**Réussite :** le public distingue le lieu de travail, les instructions et une preuve. Aucun fichier n’a été modifié pour cette mission.

## Démo 2 · vérifier un résultat · 5 min

**Slide 20 → terminal du dépôt principal (`main`). Question : « Quelles preuves faut-il avant d’accepter le résultat de l’agent? »**

### 0:00–0:45 · Rejouer un résultat

```powershell
Set-Location -LiteralPath $Depot
$Execution = uv run --project etl python -m hydrodatalab run --source fixture --date 2026-10-17 --output outputs/cli | ConvertFrom-Json
if ($LASTEXITCODE -ne 0) { throw 'Le traitement Python a échoué.' }
$Execution.summary | Format-List
$Execution.indicators | Format-List
$Export = Join-Path 'outputs/cli' $Execution.runId
Get-ChildItem -LiteralPath $Export | Select-Object Name,Length
Get-Content -LiteralPath (Join-Path $Export 'hourly.csv') -TotalCount 5
```

**Montrer :** 168 observations, 4 séries, `observations.csv`, `observations.parquet`, `hourly.csv`, `report.json`. Le CSV horaire contient 24 lignes calculées à partir des 96 demandes de 15 minutes. Le dossier de résultats inclut un identifiant unique de traitement; il n’est pas simplement `outputs/cli/report.json`.

**Dire :** « Ce traitement Python est autonome. CSV et Parquet servent à l’analyse; JSON explique le résultat et sa provenance. »

### 0:45–2:15 · Rendre la transformation visible dans Vue

**Faire :** ouvrir http://localhost:53170 et utiliser la collecte complète réussie pendant la préparation (Jeu de démonstration, sans mesure retirée). Repérer **Terminé** dans **Dernières exécutions**. Si le rapport ne correspond pas, lancer une nouvelle collecte complète; ne pas prétendre que les clics sur les onglets exécutent les étapes.

**Faire et montrer :** dans **Parcours ETL**, cliquer successivement :

1. **1 · Extraire** : `date`, `valeurs_demandetotal` — vrais champs du fournisseur, 168 lignes sur les quatre séries.
2. **2 · Transformer** : `timestamp_utc`, `value`, `unit` — dates UTC, nombres typés, unités vérifiées, doublons retirés. Sur le jeu propre, le nombre de lignes reste le même : nettoyer ne signifie pas supprimer arbitrairement des données.
3. **3 · Charger / exporter** : ouvrir **Afficher la synthèse horaire**. Quatre créneaux de 15 minutes deviennent une moyenne; une heure incomplète reste absente. Montrer la courbe horaire et le lien vers son CSV; éviter une manipulation de téléchargement pendant ce créneau.

**Dire :** « Voici un résultat concret à vérifier avant d’accepter le travail de l’agent : quatre mesures deviennent une moyenne horaire. Le calcul vient de Python; Vue affiche son résultat. »

**Développer seulement la pointe horaire moyenne.** Les autres valeurs restent des repères pour les questions, pas trois explications supplémentaires dans le créneau :

- Pointe horaire moyenne : **22 009,75 MW**, 17 octobre à **18 h** Toronto. Maximum des heures complètes, différent du maximum ponctuel de la courbe de 15 minutes.
- Énergie demandée estimée : **489 442 MWh**, somme de la demande × 0,25 h. Hypothèse : puissance constante pendant chaque créneau; estimation descriptive, pas facturation ni prévision.
- Variation du niveau observé : **+3 cm** entre 00 h et 23 h, à Gouin Nord. Ce n’est pas un volume d’eau ou une réserve d’énergie.

### 2:15–3:15 · Relier le résultat au code et au test

**Faire :** dans VS Code, ouvrir **etl/hydrodatalab/analysis.py**, puis **etl/tests/test_analysis.py**, déjà préparés dans des onglets. Montrer la règle de synthèse horaire et un test avec quatre créneaux valides. Ne pas lire tout le module.

**Dire :** « À la première démo, Copilot a cité des fichiers. Nous pouvons maintenant confronter son explication au calcul et à un test. » Les onglets Vue explorent un rapport enregistré; ils ne recalculent rien au clic. Les minima et maxima de température restent deux séries distinctes.

### 3:15–4:15 · Lire les détails d’une exécution

**Faire :** dans **Dernières exécutions**, ouvrir **Voir les détails** sur le succès préparé. Montrer les paramètres, les quatre étapes Python, leurs durées et un export lié à cet identifiant. Si le créneau le permet, montrer un seul journal correspondant dans Aspire; aucun tour de toutes les ressources.

**Dire :** « Quartz déclenche la tâche; Python traite; SQLite conserve le bilan. Aspire regroupe les processus et les journaux. » Les étapes Python sont un bilan reçu à la fin, pas une progression animée. Les anciens exports restent rattachés à leur propre exécution. La planification reste désactivée.

### 4:15–5:00 · Rejouer dans une image préparée

```powershell
docker run --rm --pull never --network none hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /tmp/result
```

Utiliser la variante Docker/Podman déjà validée. **Montrer :** le résumé et l’indicateur horaire correspondant au natif. **Dire :** « Ce résultat est rejouable avec le code et ses dépendances dans une image. » Les fichiers de /tmp/result disparaissent avec ce conteneur. La comparaison complète CSV/Parquet/JSON est une vérification préalable décrite dans [DOCKER](../docs/DOCKER.md); le résumé seul ne la prouve pas.

**Arrêt à 5:00.** Si Docker est indisponible, annoncer que l’on utilise le résultat préparé ou la capture, puis revenir à SDD. Ne pas diagnostiquer le moteur en direct. Si un passage précédent dépasse son créneau, omettre le journal Aspire; conserver la preuve dans le code et le test.

### Variante 3:15–5:00 · Échec et relance à la place d’Aspire et Docker

Choisir cette variante **avant la séance**, ou l’utiliser pendant les questions. Elle remplace les deux derniers créneaux; elle ne s’y ajoute pas.

**Faire :** cliquer **Démontrer un échec contrôlé**, attendre **Échec**, puis **Voir les détails**. Montrer **Extraire les sources** en échec et le message; les étapes suivantes n’ont pas tourné. Cliquer **Relancer ce traitement**, attendre **Terminé**, puis ouvrir les détails. La relance pointe vers la première tentative; l’ancienne erreur et le dernier rapport valide sont préservés.

**Dire :** « Nous vérifions aussi le comportement en échec. La panne est simulée et annoncée. » Si la relance tarde, montrer la capture [exécution](../docs/screenshots/etl-execution.png) en l’annonçant. Ne pas activer la planification pour créer du mouvement.

**Réussite :** le public sait relier un résultat aux données, au code, au test et à une exécution. La démo illustre comment évaluer le travail de Copilot; les détails BI et l’infrastructure restent disponibles pour les questions.

## Transition · préparer la démo 3

**Garder la slide 21 (SDD) sur le projecteur.** Pendant ses deux minutes, lancer ce bloc dans le terminal de pilotage, puis expliquer la mini-spécification. Les deux dossiers ont déjà démarré avec succès pendant la préparation.

```powershell
Set-Location -LiteralPath $Depot
./scripts/stop-demo.ps1
Set-Location -LiteralPath $Live
git status --short --branch
./scripts/start-demo.ps1 -NoBuild
```

**Attendu :** la branche de cette répétition, avec `coverage.py` encore à implémenter. Recharger Vue, lancer une collecte complète et vérifier **Non calculée**. Si l’indicateur est déjà calculé, ne pas poursuivre comme si l’exercice était neuf : vérifier le chemin et préparer une nouvelle branche selon la dernière section.

**Dire :** « Voici la version de départ de notre exercice. L’interface est prête; il lui manque le calcul Python. » Passer à la fenêtre VS Code de `$Live`, puis à la slide 22. La présentation reste servie depuis `$Depot`; aucun ancien support du tag 06 n’est lancé.

## Démo 3 · une règle devient un contrôle · 4 min

**Slide 22 → Vue puis VS Code de la copie d’exercice. Question : « Une seule mesure manque : notre contrôle le détecte-t-il? »**

### 0:00–0:40 · Montrer le besoin et l’état initial

**Montrer :** **Couverture : Non calculée**, puis ces trois fichiers dans la copie d’exercice :

- `specs/coverage.md` : la règle et ses critères;
- `etl/hydrodatalab/coverage.py` : la fonction qui renvoie encore `None`;
- `specs/test_coverage_acceptance.py` : les tests de réussite.

**Dire :** « 24 mesures horaires attendues : 24 donnent 100 %, 23 donnent 95,83 %. Les doublons n’ajoutent rien et zéro reste valide. Les journées de changement d’heure n’ont pas toutes 24 heures. » Les deux tests d’acceptation rouges ont été vérifiés pendant la préparation; ne pas gaspiller la fenêtre de génération à les relancer ici.

### 0:40–2:10 · Déléguer une modification délimitée

Créer une nouvelle session Copilot **dans la copie d’exercice**, avec le modèle autorisé retenu avant la séance. **Copier :**

```text
Implémente calculate_coverage dans etl/hydrodatalab/coverage.py.
Respecte specs/coverage.md, la signature existante et docs/CONTRACT.md.
Les timestamps reçus correspondent déjà aux observations dont la valeur est valide.
Compte des créneaux distincts alignés et inclus dans [début, fin).
Début et fin sont des dates civiles America/Toronto; fin exclusive.
Calcule expected à partir des bornes converties en UTC et de interval_minutes,
pas à partir des lignes reçues. Gère les journées de 23 et 25 heures.
Retourne observed, expected, percent arrondi à deux décimales.
Ne recrée pas l’API ni l’interface. Ne modifie pas les données brutes.
Exécute depuis la racine de cette copie :
uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live
Lis les échecs et corrige la fonction. Termine par le diff et les résultats
vérifiés; n’affirme pas avoir lancé un test sans résultat d’exécution.
```

**Montrer :** le périmètre demandé et les confirmations d’actions utiles. Ne pas approuver machinalement une modification de tests qui réduirait les critères de réussite.

**À 2:10, décider :** si le code et les tests ne sont pas prêts, arrêter la génération dans Copilot et passer à la section Secours. Phrase : « Je passe à la solution préparée pour garder le temps de vérifier. »

### 2:10–3:00 · Lire la preuve

**Montrer :** le diff dans **Changes**, puis la sortie réelle des tests. Si l’agent n’a pas exécuté la validation, la lancer dans le terminal de cette copie :

```powershell
Set-Location -LiteralPath $Live
git diff --stat
git diff -- etl/hydrodatalab/coverage.py
uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live
```

**Attendu pour la solution préparée :** 43 tests réussis. Si des tests utiles ont été ajoutés, le total peut augmenter. Contrôler les jours de 23/25 heures et la mesure manquante. Les tests en échec interdisent d’annoncer une solution validée.

### 3:00–4:00 · Voir le résultat et sa limite

**Faire :** après l’enregistrement Python et son rechargement, retourner à Vue. Lancer une **nouvelle** collecte complète, déplier **Qualité par série**. Montrer le niveau d’eau à **24 / 24 · 100 %**. Cocher **Retirer une mesure de niveau d’eau**, relancer, montrer **23 / 24 · 95,83 %**.

**Dire, puis marquer une pause :** « La règle que nous venons de définir détecte maintenant la mesure manquante. » La synthèse globale affiche **167 / 168, soit 99,4 %** : ce n’est pas le même dénominateur. Le retrait est en mémoire; les données brutes restent intactes.

Si l’ancien rapport reste visible, attendre la fin de la nouvelle collecte et rafraîchir. Modifier le code ne recalcule pas rétroactivement un rapport déjà publié. Si le rechargement Python n’a pas eu lieu, utiliser le secours au lieu de diagnostiquer devant le public.

**Réussite :** une règle compréhensible, du code examiné, des tests réussis et un changement visible. Revenir à la slide 23, sélection de skills.

## Secours · revenir à la solution préparée

**Déclencheur :** à 2:10 de la démo 3, tests encore rouges, génération trop lente ou indisponible. Arrêter d’abord la génération Copilot, puis annoncer le passage au code préparé. Les modifications de l’exercice restent sur disque : aucun checkout ni nettoyage dans ce dossier.

```powershell
Set-Location -LiteralPath $Live
./scripts/stop-demo.ps1
Set-Location -LiteralPath $Depot
./scripts/start-demo.ps1 -NoBuild
```

Recharger Vue, utiliser la fenêtre VS Code principale. Pour montrer la modification déjà préparée :

```powershell
Set-Location -LiteralPath $Depot
git diff 06-specification 07-solution -- etl/hydrodatalab/coverage.py
uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/secours
```

Puis effectuer l’avant/après **100 % → 95,83 %** avec deux nouvelles collectes. Le code n’a pas été généré en direct : le dire explicitement. En cas de panne complète, montrer [la capture de qualité](../docs/screenshots/03-qualite-mesure-absente.png) et les exports préparés. Cette capture montre 99,4 % au global, pas le détail 95,83 % replié; utiliser le rapport JSON ou le test pour cette valeur.

## Fin et nouvelle répétition

### Conserver la tentative

Arrêter la génération et l’application d’exercice avant de changer de branche. Si le secours principal est encore actif, l’arrêter également depuis `$Depot`. Dans la copie d’exercice :

```powershell
Set-Location -LiteralPath $Live
./scripts/stop-demo.ps1
git status --short
git diff
```

Après avoir examiné les fichiers, conserver les changements voulus. Exemple si seule la fonction a changé :

```powershell
git add -- etl/hydrodatalab/coverage.py
git commit -m "Répétition : calcul de couverture et validation"
git status --short --branch
```

Ajouter explicitement les éventuels tests pertinents avant le commit. Si Git n’a pas d’identité configurée sur le portable, renseigner l’identité de travail appropriée **localement à cette copie**. Ne pas faire passer la tentative pour validée si les tests échouent; choisir un message de commit qui le précise. On peut aussi conserver les fichiers tels quels et arrêter ici.

### Repartir sans écraser la tentative précédente

Depuis un dossier d’exercice propre et les applications arrêtées, créer une **nouvelle branche au nom unique**. Cette commande ne réutilise ni ne réinitialise une branche existante.

```powershell
$BrancheSuivante = 'codex/repetition-' + (Get-Date -Format 'yyyyMMdd-HHmmss')
Set-Location -LiteralPath $Live
if (git status --porcelain) { throw 'Conserver les changements avant de repartir.' }
git -C "$Depot" bundle create .runtime/repetition-source.bundle --all
if ($LASTEXITCODE -ne 0) { throw 'Actualisation du bundle impossible.' }
git fetch "$Depot/.runtime/repetition-source.bundle" main codex/preparation-demo-06
if ($LASTEXITCODE -ne 0) { throw 'La copie ne peut pas lire le dépôt principal.' }
git switch -c $BrancheSuivante 9e1ecde478f4f5d70634a42c720fa10fd224eab7
if ($LASTEXITCODE -ne 0) { throw 'Bascule interrompue.' }
./scripts/start-demo.ps1
```

La tentative précédente reste dans sa branche. Le nouveau nom donne un historique applicatif distinct; relancer une collecte pour obtenir le rapport initial **Non calculée**. Le bundle actualisé et le `fetch` transportent la branche préparée depuis le dépôt principal local; ils ne contactent pas GitHub. Une ancienne copie sans ces corrections peut suivre ce même parcours de nouvelle branche, après conservation de son travail. Il est inutile de recloner le projet ou de recopier les archives Docker à chaque répétition. Pour le début du parcours complet, arrêter cette copie puis relancer `$Depot` comme à la préparation C.

### Reprise après un redémarrage du PC

Rouvrir ce conducteur, rejouer le bloc A pour `$Depot` et `$Live`, puis :

```powershell
Set-Location -LiteralPath $Live
git status --short --branch
git log -3 --oneline
./scripts/resume.ps1
```

Lire `PROGRESS.md` et `DEMO_STEP.md` dans le dossier concerné avant toute modification. Démarrer seulement la version souhaitée. Relancer séparément le serveur de présentation depuis `$Depot`. Aucun fichier, historique SQLite ou export n’est supprimé par ces commandes.

### À noter après chaque répétition

Dans un fichier local tel que `.runtime/notes-repetition.txt`, noter la branche, les durées réelles des trois démos et de la transition, le moteur conteneur validé, le résultat des tests et le prochain point à revoir. Ce fichier reste privé au poste et n’entre pas dans Git. Copier la tentative et ses notes sur le support de sauvegarde habituel; le bundle du dépôt principal n’inclut pas les nouveaux commits du clone d’exercice.

**Avant de présenter :** les trois durées tiennent dans 4/5/4 minutes; l’avant/après est lisible depuis le fond de la salle; le secours démarre; Copilot est connecté; l’image est chargée; les diapositives et le PDF sont accessibles hors réseau. La répétition humaine sur le portable HQ reste à effectuer, même si les commandes ont été contrôlées sur le poste de préparation.

Captures de secours de la nouvelle analyse : [avant](../docs/screenshots/etl-avant.png), [après](../docs/screenshots/etl-apres.png), [synthèse et indicateurs](../docs/screenshots/etl-synthese.png), [détail d’exécution](../docs/screenshots/etl-execution.png). Elles montrent un traitement réellement exécuté; les annoncer comme captures préparées.
