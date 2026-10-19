# Guide présentateur — HydroDataLab

Formation du **mardi 20 octobre 2026**. Public principal : citoyens développeurs, analystes BI/Python; gestionnaires et développeurs débutants/intermédiaires. Version documentaire : 18 octobre. Objectif : déléguer une petite transformation vérifiable, avec son contrat et ses preuves.

## Minutage strict

| Horloge | Diapositive | Explication | Démo |
|---|---:|---:|---:|
| 00:00–00:30 | 1 — Ouverture | 30 s | |
| 00:30–01:30 | 2 — Votre terrain de développement | 1 min | |
| 01:30–02:00 | 3 — Parcours | 30 s | |
| 02:00–04:00 | 4 — Séries et sens | 2 min | |
| 04:00–06:00 | 5 — Modèle, harnais, outils et agent | 2 min | |
| 06:00–08:00 | 6 — Dépôt, commit, branche et worktree | 2 min | |
| 08:00–13:00 | 7 — Première mission dans Agents | 1 min | 4 min |
| 13:00–15:00 | 8 — Instructions permanentes | 2 min | |
| 15:00–17:00 | 9 — Portée et diagnostic | 2 min | |
| 17:00–19:00 | 10 — Sept concepts et leurs rôles | 2 min | |
| 19:00–22:00 | 11 — Couches bilingues du contexte | 3 min | |
| 22:00–24:00 | 12 — Tokens et reprises | 2 min | |
| 24:00–26:00 | 13 — Auto | 2 min | |
| 26:00–28:00 | 14 — Coûts : quatre tâches × cinq modèles | 2 min | |
| 28:00–29:00 | 15 — HydraFusion | 1 min | |
| 29:00–35:00 | 16 — Pipeline | 1 min | 5 min |
| 35:00–38:00 | 17 — Survol SDD et mini-spec | 3 min | |
| 38:00–42:00 | 18 — Python live | | 4 min |
| 42:00–45:00 | 19 — Pratique, ressources et 30 s sur Canvas | 3 min | |
| 45:00–55:00 | Questions | 10 min | |
| 55:00–60:00 | Marge | 5 min | |

Total contenu : **32 minutes d’explications + 13 minutes de démonstrations**. Annexes 20–25 : seulement en réponse aux questions. Les notes détaillées sont intégrées à `presentation/slides.md` et au mode présentateur Slidev.

Révision 1.142 : la diapositive 10 présente les sept concepts sans détailler leur configuration; la fiche [personnalisations-vscode.md](personnalisations-vscode.md) fournit les formats. La 11 montre cinq couches en anglais et en français, leurs fichiers et leurs conditions d’utilisation. Cliquer une couche illustre son apport au contexte du modèle; ce dessin ne définit pas une priorité. La 14 compare simultanément quatre tâches en colonnes et cinq modèles en lignes : même volume supposé entre modèles d’une tâche, barres sur une échelle commune, losanges pour les choix à essayer. Le bouton + multiplie les essais; les coûts restent des scénarios fictifs, pas un benchmark. La 17 nomme SDD et annonce la formation de l’autre équipe. Canvas occupe 30 secondes dans les trois minutes de conclusion; l’annexe 22 fournit les prérequis. La capture de l’annexe 20 vient d’un autre projet et montre des listes vides : ne pas la présenter comme une preuve de chargement de HydroDataLab.

Pour la règle zéro/absence : trois températures fictives `[10, 0, None]` donnent deux mesures sur trois, soit 66,67 %. Remplacer None par zéro inventerait une mesure. Les contrôles de plausibilité restent propres à chaque série. Les liens officiels au bas des diapositives sont cliquables dans le site et dans le PDF.

## Ce que voit le public

Les diapositives expliquent les notions et annoncent ce que les participants pourront observer ou vérifier. Les raccourcis clavier, commandes, minutages et procédures de secours se trouvent dans ce guide et dans les notes du mode présentateur. Projeter la fenêtre de présentation, pas la vue des notes.

Ne pas supposer de connaissances Git ou Docker. Définir les termes avant de les employer : modèle et agent (5), dépôt/commit/branche/worktree (6), session/planification/autorisations (7), instruction (8), skill et rôle personnalisé (10), contexte (11), token (12), API et image Docker (16), spécification et critère de réussite (17). Le worktree est une option expliquée, pas un prérequis ni une manipulation demandée. Le lexique est repris dans l'aide-mémoire.

À chaque démonstration, donner une question au public : « Quelles sources vérifient cette réponse? », « D'où vient ce résultat et comment reprendre après une erreur? », « Une mesure manque : le contrôle le détecte-t-il? ». Les repères de la fenêtre Agents et ses réglages se montrent dans la première démo, avec leur utilité : lisibilité, portée du réglage, confirmation des actions et comparaison avant/après.

## L’ouverture et le moment fort

La nouvelle diapositive 2 occupe une minute. Le contexte est celui communiqué par le présentateur : des analystes BI/Python qui peuvent expérimenter avec un cadre de développement plus souple, tout en conservant les règles de sécurité de leur organisation. Décrire le trajet : développer ETL et visualisations, tester localement, préparer code et dépendances pour un déploiement conteneurisé. Définir ETL et conteneur brièvement. Les accès, données et environnements doivent être autorisés pour l’usage. Ne pas présenter cette liberté comme une exception aux politiques HQ; elles ne sont pas documentées dans ce support. Réduire la couverture et l’annonce du parcours à 30 secondes chacune préserve les 45 minutes.

**Le moment fort principal est la démo 3, de 38:00 à 42:00** : passer d’une règle en français à un calcul Python testé, puis voir son résultat dans la même interface. Avant le prompt, montrer « Couverture : Non calculée » sur le jalon 06. Après les tests, revenir à la qualité par série : le niveau horaire affiche 100 % pour les données complètes, puis 95,83 % quand une mesure est retirée en mémoire. Marquer une pause : « Vous voyez maintenant ce que cette règle permet de détecter. » La couverture globale de 99,4 % reste un indicateur différent.

Le passage du même cœur Python de la commande locale au conteneur, pendant la démo 2, renforce la pertinence pour ce public. Les images, l’interface, l’API et le suivi Aspire sont préparés; seule la petite évolution Python est demandée à Copilot en direct. Ce moment dépend du résultat réel de la génération; à 2:10, la solution de secours est annoncée clairement. Canvas et HydraFusion sont des capsules explicatives, pas des démonstrations intégrées. La répétition du 19 octobre doit confirmer que l’avant/après est lisible et tient dans les quatre minutes.

## Préparer lundi, sur le portable HQ

Depuis la racine de la version finale (`main` ou `07-solution`), exécuter `./scripts/doctor.ps1`, `./scripts/prepare.ps1`, puis `./scripts/verify.ps1`. Lire leur aide et le README avant d’ajouter des options. Le pipeline ne nécessite aucune donnée interne. Docker/Compose et Aspire doivent être vérifiés sur ce portable avant la séance; ne pas installer ni télécharger d’image devant le public.

- Vérifier connexion Copilot, modèle autorisé, version VS Code et moteur Copilot. Confirmer Auto Balance et GPT-6.1 Sol; choisir un remplacement autorisé avant la séance si nécessaire.
- Les paramètres expérimentaux ne sont pas requis pour réussir la formation. HydraFusion reste une capsule facultative; pas d’exécution si le pool/politiques HQ ne le permettent pas.
- Instantané confirmé : 168 observations du 17 octobre 2026, extraites le 18 octobre à 19:44:01 UTC. Vérifier empreintes et manifeste, sans remplacer les fixtures pour obtenir un résultat plus spectaculaire.
- Préparer deux dossiers de travail distincts : live depuis `06-specification`, secours depuis `07-solution`. Commande pour créer la branche live depuis un dépôt propre : `./scripts/checkpoint.ps1 -Tag 06-specification -Branch demo-live`. Ce script refuse un dossier modifié et arrête Aspire avant la bascule. Utiliser les scripts de la version finale et le guide `docs/CHECKPOINTS.md` pour préparer les copies : tous les scripts ne sont pas présents dans les premiers tags. Préparer séparément la copie de secours 07-solution; ne pas écraser un dossier sale.
- Lancer `./scripts/start-demo.ps1` et tester le parcours. Ports réservés : Vue 53170, API 53171, Python 53172, slides 53173.
- Ouvrir le support : `./scripts/start-presentation.ps1` (ou `cd presentation`, `npm ci`, `npm run dev`); http://localhost:53173. Pour le build autonome : `npm run build` puis `npm run preview`. L’installation réseau se fait avant la séance.
- Ouvrir aussi `presentation/exports/formation-hydrodatalab.pdf`. Il contient 25 pages; les interactions sont figées mais leur explication reste lisible.
- Pour régénérer ce PDF : `npm --prefix presentation run export` utilise Chrome/Edge/Chromium et Python avec `pypdf`. Au besoin, définir `SLIDEV_PYTHON_PATH` vers cet interpréteur. Le post-traitement convertit les liens d’annexes en destinations PDF sans serveur; projeter le PDF déjà préparé ne demande aucune de ces dépendances.
- Tester projection 16:9, zoom du navigateur, notifications désactivées et alimentation branchée. Faire une répétition chronométrée sans dépendre du réseau.

### Préparer les jalons et les conteneurs

- `00` à `03` : suivre les commandes propres à chaque jalon dans [CHECKPOINTS](../docs/CHECKPOINTS.md); ne pas appeler les scripts absents d’un jalon.
- `04-pilotage` : exécuter `./scripts/prepare.ps1 -SkipPresentation`, puis `./scripts/start-demo.ps1`. Le support Slidev n’est livré qu’à partir de `05-docker`.
- `05-docker`, `06-specification`, `07-solution` : le script `prepare.ps1` complet est disponible. Au jalon 06, les échecs des tests de couverture sont volontaires avant l’exercice; ils ne signifient pas que l’installation a échoué.
- Préparer et tester les images avec [DOCKER](../docs/DOCKER.md) avant la séance. `prepare-docker.ps1` arrête l’application native. Après une répétition Compose, appeler `./scripts/docker-demo.ps1 -Action Stop` avec le même contexte, puis relancer le natif. Ne lancer qu’une pile à la fois sur 53170.
- La génération Python live s’effectue en mode natif au jalon 06 : les images Docker préparées contiennent la solution finale. Le démarrage sélectionne `.runtime/<branche-ou-tag>`; la copie de secours a aussi son dossier distinct.
- Conserver une copie du PDF et du conducteur hors du dossier qui change de tag. Pour le secours, cloner localement le dépôt dans un nouveau dossier, y sélectionner `07-solution`, exécuter `prepare.ps1` et répéter. Le bundle Git ne transporte pas les dépendances ni les images : les préparer sur place ou les transférer comme indiqué dans DOCKER.

## Démo 1 — 4 minutes

1. **0:00–0:45** : ouvrir **Chat: Open Agents Window** et montrer le dossier et la branche déjà expliqués. Session = conversation pour une tâche; `/plan` demande une démarche avant l'implémentation. Le menu de cette fenêtre ne contient pas actuellement l'agent Plan : utiliser `/plan` dans la session Copilot. Copilot est l'environnement choisi et Auto sélectionne un modèle; ses niveaux seront expliqués ensuite. Présenter les confirmations d'actions.
2. **0:45–1:30** : ouvrir `Ctrl+,`, ajuster la lisibilité et expliquer la portée : Utilisateur pour ses projets, Espace de travail pour le projet ouvert. Montrer **Changes**, la comparaison avant/après d'un fichier.
3. **1:30–2:20** : ouvrir **Chat: Open Customizations**. Instruction = consigne écrite pour l'agent. Retrouver la règle Python qui distingue zéro d'une absence.
4. **2:20–4:00** : lancer le prompt de découverte de `prompts.md`. Traduire **References** par « sources consultées », ouvrir un fichier cité et vérifier une affirmation avec le public.

Résultat attendu : le public sait où l'agent travaille, quelles actions il peut faire et comment vérifier une affirmation. Si l'authentification est indisponible, montrer les fichiers et les règles locaux en annonçant que la réponse ne sera pas générée en direct. Ne pas créer de worktree pendant cette démonstration.

## Démo 2 — 5 minutes

- 0:00–1:00 : depuis la racine, `uv run --project etl python -m hydrodatalab run --source fixture --date 2026-10-17 --output outputs/cli`; montrer `report.json`, CSV et Parquet.
- 1:00–2:15 : depuis Vue, lancer fixture, suivre l’état et ouvrir les séries/provenance. Les températures ne sont pas implicitement le capteur de niveau Gouin.
- 2:15–3:00 : montrer les ressources et un journal dans Aspire. Python CLI/FastAPI partagent le même cœur, .NET/Quartz coordonne, SQLite conserve les runs.
- 3:00–4:30 : exécuter le scénario d’échec préparé et relancer. Montrer l’erreur lisible et la trace conservée.
- 4:30–5:00 : exécuter la commande Docker CLI ci-dessous, déjà préremplie et avec une image chaude. Montrer le résumé produit. Les valeurs calculées doivent correspondre au natif, hors métadonnées d’exécution.

```powershell
docker run --rm --network none hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /tmp/result
```

Si le moteur autorisé utilise un contexte explicite, ajouter `--context <nom-validé>` juste après `docker`, par exemple `docker --context podman-machine-default run ...` sur le poste de construction. Ne pas changer le contexte global pour la démonstration. Le fichier dans `/tmp/result` disparaît avec ce conteneur : la démonstration montre le résumé console; les exports persistants ont déjà été montrés côté natif. Préparer et valider la pile Compose complète avant la séance; ne pas basculer toute la pile pendant ces cinq minutes.

Ne pas dépendre d’un appel live : le snapshot local assure la démonstration. Ne pas activer une planification permanente pour créer du mouvement. Montrer le contrôle, puis revenir à désactivé.

## Démo 3 — 4 minutes, seuil de bascule à 2:10

- **0:00–0:40** : montrer « Couverture : Non calculée » sur le parcours `06-specification`, puis ouvrir `etl/hydrodatalab/coverage.py`, `specs/test_coverage_acceptance.py` et le contrat. Les tests de couverture rouges sont volontaires à cette étape.
- **0:40–2:10** : coller le prompt live. L’agent modifie Python, sans recréer l’API ni Vue. Attendre une sortie de tests, pas seulement « terminé ».
- **À 2:10** : si ce n’est pas prêt, dire « Je passe à la solution préparée pour garder le temps de vérifier. » Préserver les modifications, arrêter les services du live et utiliser le dossier de secours `07-solution` avec runtime séparé.
- **2:10–3:00** : montrer le diff et les tests, dont jours de 23/25 heures.
- **3:00–4:00** : revenir au même écran, lancer scénario complet puis manquant. Déplier **Qualité par série** : le niveau horaire est à 24/24 puis 23/24 = 95,83 %. La couverture globale des quatre séries devient 167/168, soit 99,4 % dans la synthèse : ce n’est pas le même dénominateur. Le retrait est en mémoire; les données originales sont intactes.

Commande de vérification à partir de la racine : `uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live`. La fonction reçoit des timestamps déjà filtrés sur valeur valide; la validité de zéro se vérifie aussi au niveau du pipeline. Une valeur absente n’est pas transformée en zéro.

## Reprise après incident

Consulter `PROGRESS.md`, l’état Git et `./scripts/resume.ps1`. Parcours : `00-socle`, `01-instructions`, `02-extraction`, `03-transformation-bi`, `04-pilotage`, `05-docker`, `06-specification`, `07-solution`. Préserver les changements; ne pas utiliser `git reset --hard` pour gagner du temps.

Si le PC redémarre : rouvrir le PDF, annoncer l’étape, lancer le mode fixture. Si Docker échoue : utiliser le chemin natif déjà répété. Si Copilot est indisponible : ouvrir spec, diff préparé et tests; ne pas présenter le code préparé comme une génération live. Si tout service échoue : montrer les exports locaux vérifiés et les annexes. Les captures réelles du projet sont dans `docs/screenshots/` : poste de contrôle, séries complètes, qualité avec mesure absente, échec contrôlé et relance/exports. Les ouvrir en secours en annonçant qu’il s’agit de captures préparées.

## Index des captures de secours

Ces fichiers montrent l’application HydroDataLab réelle, pas VS Code ni une génération Copilot. Les heures ci-dessous sont les dates de dernière modification des fichiers, relevées le 18 octobre 2026 en UTC; elles ne certifient pas l’instant précis de capture. Zoomer sur la zone utile pendant la présentation.

| Fichier | Ce qui est effectivement visible | Date du fichier (UTC) |
|---|---|---|
| [01 — Poste de contrôle](../docs/screenshots/01-poste-controle.png) | Courbes, commandes et historique; dernier rapport avec une mesure absente | 2026-10-18 19:58:24 |
| [02 — Collecte en cours](../docs/screenshots/02-observatoire-complet.png) | Une collecte en cours; le dernier rapport réussi reste visible | 2026-10-18 19:58:25 |
| [03 — Mesure absente](../docs/screenshots/03-qualite-mesure-absente.png) | 167 observations, avertissement pédagogique, couverture globale de 99,4 % et tableau des mesures | 2026-10-18 19:58:30 |
| [04 — Échec contrôlé](../docs/screenshots/04-echec-controle.png) | Erreur HTTP 503 simulée et action de relance; rapport précédent conservé | 2026-10-18 19:58:30 |
| [05 — Relance et exports](../docs/screenshots/05-relance-et-exports.png) | Exécution réussie après l’échec, 168 observations, exports et planification désactivée | 2026-10-18 19:58:34 |
| [06 — Écran étroit](../docs/screenshots/06-mobile.png) | Même application disposée sur un écran étroit | 2026-10-18 19:58:34 |

La capture 03 garde le détail de qualité par série replié : elle ne montre donc pas directement la couverture du niveau à 95,83 %. Pour cette preuve, déplier la section dans l’application ou montrer le résultat de test et le rapport JSON préparé.

## Réponses courtes aux questions fréquentes

- **Peut-on tout mettre dans un gros fichier?** Le socle doit rester pertinent; détails par composant et procédure à la demande.
- **Quel modèle est le meilleur?** Commencer avec Auto Balance; comparer preuves, reprises et coût de tâches représentatives. La matrice n’est pas un classement absolu.
- **Les règles garantissent-elles la conformité?** Non; elles orientent. Les tests, permissions, revue et outils déterministes apportent d’autres contrôles.
- **Ce niveau nous dit-il les réserves?** Non. Le cas affiche un niveau en mètres, pas une conversion en énergie ni une prévision de gestion.
- **Pourquoi une spec pour 20 lignes?** Parce qu’un doublon, null ou changement d’heure peut invalider un indicateur convaincant.
