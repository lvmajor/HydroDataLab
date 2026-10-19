# Guide présentateur — HydroDataLab

Formation du **mardi 20 octobre 2026**. Public principal : citoyens développeurs, analystes BI/Python; gestionnaires et développeurs débutants/intermédiaires. Version documentaire : 19 octobre. Sujet central : utiliser GitHub Copilot dans Visual Studio Code avec des règles, du contexte, des skills et des outils adaptés, puis vérifier ses résultats. HydroDataLab illustre ces pratiques pour le public BI/Python.

**Pour répéter et piloter les trois démos :** ouvrir le [conducteur avec commandes à copier](conducteur-demos.html), utilisable directement dans un navigateur, ou sa [source Markdown](conducteur-demos.md). Le parcours recommandé conserve `main` pour les démos 1 et 2 ainsi que le secours, et utilise une seule copie d’exercice sur `codex/repetition-01`, issue de `codex/preparation-demo-06` (9e1ecde) pour la démo 3. La bascule se prépare pendant la slide 18; le serveur des slides reste sur `main`.

## Minutage strict

| Horloge | Diapositive | Explication | Démo |
|---|---|---:|---:|
| 00:00–00:30 | 1 — Copilot dans VS Code | 30 s |  |
| 00:30–01:30 | 2 — Votre terrain de développement | 1 min |  |
| 01:30–02:00 | 3 — Bien déléguer avec Copilot | 30 s |  |
| 02:00–04:00 | 4 — Les données du fil conducteur | 2 min |  |
| 04:00–06:00 | 5 — Modèle, harnais, outils et agent | 2 min |  |
| 06:00–08:00 | 6 — Repères Git | 2 min |  |
| 08:00–13:00 | 7 — Première mission dans Agents | 1 min | 4 min |
| 13:00–15:00 | 8 — Une instruction vérifiable | 2 min |  |
| 15:00–17:00 | 9 — Format, portée et activation | 2 min |  |
| 17:00–19:00 | 10 — Couches bilingues du contexte | 2 min |  |
| 19:00–21:00 | 11 — Mécanismes de personnalisation | 2 min |  |
| 21:00–22:00 | 12 — Base vanille et ajouts ciblés | 1 min |  |
| 22:00–24:00 | 13 — Tokens et reprises | 2 min |  |
| 24:00–25:30 | 14 — Auto | 1 min 30 |  |
| 25:30–27:30 | 15 — Coûts : quatre tâches × cinq modèles | 2 min |  |
| 27:30–28:30 | 16 — Capsule HydraFusion | 1 min |  |
| 28:30–34:30 | 17 — Vérifier un résultat | 1 min | 5 min |
| 34:30–36:30 | 18 — Survol SDD et préparation de l’exercice | 2 min |  |
| 36:30–40:30 | 19 — Déléguer et vérifier la modification |  | 4 min |
| 40:30–42:00 | 20 — Des skills selon votre tâche | 1 min 30 |  |
| 42:00–43:30 | 21 — Votre prochaine tâche avec Copilot | 1 min 30 |  |
| 43:30–45:00 | 22 — Ressources et liens internes HQ | 1 min 30 |  |
| 45:00–55:00 | Questions | 10 min |  |
| 55:00–60:00 | Marge | 5 min |  |

Total contenu : **32 minutes d’explications + 13 minutes de démonstrations**. Questions : 10 minutes; marge : 5. Les 22 diapositives principales occupent exactement 45 minutes sur le conducteur. Les annexes 23–28 sont réservées aux questions; la répétition humaine doit confirmer le rythme réel.

Le parcours relie les fichiers d’instructions (8–9) aux couches de contexte (10), puis aux mécanismes de personnalisation (11) et à la base « vanille » (12). Le sujet reste Copilot dans VS Code; Python et l’ETL fournissent les exemples vérifiables. Aucun aperçu ETL supplémentaire n’est ajouté à l’ouverture.

La slide 11 garde les sept concepts : suivre un exemple instruction → skill → outil, puis situer agent personnalisé, MCP, hook et plugin sans entrer dans sept configurations. La fiche [personnalisations-vscode.md](personnalisations-vscode.md) conserve les formats. La sélection de skills rejoint la fin (20), après leur mise en contexte. Matt Pocock reste en exploration; aucune liste universelle de « must have » n’est affirmée.

La 10 montre les couches en anglais/français, leurs fichiers et leurs conditions d’utilisation. Elles composent le contexte sans définir une priorité officielle. La 15 compare quatre tâches et cinq modèles avec des volumes supposés; les losanges sont des choix à essayer, pas un benchmark. La 18 nomme le SDD et annonce la formation de l’autre équipe. **Canvas reste en annexe 25**, accessible depuis les ressources (22), sans nouveau concept dans la conclusion.

Pour zéro/absence, [10, 0, None] donne deux mesures sur trois, soit 66,67 %. Les contrôles de plausibilité restent propres à chaque série. La capture Instructions en annexe 23 provient d’un autre projet, avec des listes vides : ce n’est pas une preuve de chargement HydroDataLab. Les sources au bas des slides restent cliquables dans le site et le PDF.

La page 22 reste visible pendant les questions. Compléter les trois entrées HQ dans presentation/resources/links.json; la fiche [skills-et-veille.md](skills-et-veille.md) donne les sources et les modalités de mise à jour.

## Ce que voit le public

### Piloter depuis le portable

Utiliser deux écrans en mode **Étendre** (`Win + P`) et deux fenêtres du même navigateur/profil. Garder http://127.0.0.1:53173/presenter/1 sur le portable; déplacer http://127.0.0.1:53173/1 sur le projecteur puis passer en plein écran (`F11`). Utiliser exactement le même hôte et le même port dans les deux fenêtres.

Dans le panneau **Current → Slides** de la vue présentateur, sélectionner les couches de contexte (10) et actionner les boutons `−`/`+` des coûts (15). Le public voit les changements automatiquement. Les réglages restent conservés après navigation ou rechargement; avant la séance, sélectionner la première couche et 1 essai. Cette synchronisation fonctionne hors ligne, y compris dans le kit. Recharger les deux fenêtres après une mise à jour du support. Pour une démo, afficher VS Code ou HydroDataLab sur l’écran de projection, puis revenir à la fenêtre audience.

### Contenu projeté

Les diapositives expliquent les notions et annoncent ce que les participants pourront observer ou vérifier. Les raccourcis clavier, commandes, minutages et procédures de secours se trouvent dans ce guide et dans les notes du mode présentateur. Projeter la fenêtre de présentation, pas la vue des notes.

Ne pas supposer de connaissances Git ou Docker. Définir les termes avant de les employer : modèle et agent (5), dépôt/commit/branche/worktree (6), session/planification/autorisations (7), instruction (8), contexte (10), skill, outils et MCP (11), token (13), API et image Docker (17), spécification et critère de réussite (18). Le worktree est une option expliquée, pas un prérequis ni une manipulation demandée. Le lexique est repris dans l'aide-mémoire.

À chaque démonstration, donner une question au public : « Quelles sources vérifient cette réponse? », « Quelles preuves faut-il avant d’accepter le résultat de l’agent? », « Une mesure manque : le contrôle le détecte-t-il? ». Les repères de la fenêtre Agents et ses réglages se montrent dans la première démo, avec leur utilité : lisibilité, portée du réglage, confirmation des actions et comparaison avant/après.

## L’ouverture et le moment fort

La nouvelle diapositive 2 occupe une minute. Le contexte est celui communiqué par le présentateur : des analystes BI/Python qui peuvent expérimenter avec un cadre de développement plus souple, tout en conservant les règles de sécurité de leur organisation. Décrire le trajet : développer ETL et visualisations, tester localement, préparer code et dépendances pour un déploiement conteneurisé. Définir ETL et conteneur brièvement. Les accès, données et environnements doivent être autorisés pour l’usage. Ne pas présenter cette liberté comme une exception aux politiques HQ; elles ne sont pas documentées dans ce support. Réduire la couverture et l’annonce du parcours à 30 secondes chacune préserve les 45 minutes.

**Le moment fort principal est la démo 3, de 36:30 à 40:30** : passer d’une règle en français à un calcul Python testé, puis voir son résultat dans la même interface. Avant le prompt, montrer « Couverture : Non calculée » sur la copie préparée issue de 06. Après les tests, revenir à la qualité par série : le niveau horaire affiche 100 % pour les données complètes, puis 95,83 % quand une mesure est retirée en mémoire. Marquer une pause : « Vous voyez maintenant ce que cette règle permet de détecter. » La couverture globale de 99,4 % reste un indicateur différent.

Le passage du même cœur Python de la commande locale au conteneur, pendant la démo 2, renforce la pertinence pour ce public. Les images, l’interface, l’API et le suivi Aspire sont préparés; seule la petite évolution Python est demandée à Copilot en direct. Ce moment dépend du résultat réel de la génération; à 2:10, la solution de secours est annoncée clairement. HydraFusion occupe une minute explicative; Canvas reste en annexe pour les questions. Aucun des deux ne demande une activation en séance. La répétition du 19 octobre doit confirmer que l’avant/après est lisible et tient dans les quatre minutes.

## Préparer lundi, sur le portable HQ

Depuis la racine de la version finale (`main`; les tags restent historiques), exécuter `./scripts/doctor.ps1`, `./scripts/prepare.ps1`, puis `./scripts/verify.ps1`. Lire leur aide et le README avant d’ajouter des options. Le pipeline ne nécessite aucune donnée interne. Docker/Compose et Aspire doivent être vérifiés sur ce portable avant la séance; ne pas installer ni télécharger d’image devant le public.

- Vérifier connexion Copilot, modèle autorisé, version VS Code et moteur Copilot. Confirmer Auto Balance et GPT-6.1 Sol; choisir un remplacement autorisé avant la séance si nécessaire.
- Les paramètres expérimentaux ne sont pas requis pour réussir la formation. HydraFusion reste une capsule facultative; pas d’exécution si le pool/politiques HQ ne le permettent pas.
- Instantané confirmé : 168 observations du 17 octobre 2026, extraites le 18 octobre à 19:44:01 UTC. Vérifier empreintes et manifeste, sans remplacer les fixtures pour obtenir un résultat plus spectaculaire.
- Suivre le [conducteur de répétition](conducteur-demos.html) : le dépôt principal reste sur `main` pour les démos 1 et 2, les slides et le secours. Une copie Git locale reçoit `codex/repetition-01` depuis la branche préparée `codex/preparation-demo-06` (9e1ecde), descendante de `06-specification` pour la démo 3. Préparer les dépendances et les deux démarrages avant la séance; basculer pendant la slide SDD. Aucun téléchargement ou changement de branche du dépôt principal n’est nécessaire en direct.
- Lancer `./scripts/start-demo.ps1` et tester le parcours. Ports fixes : Vue 53170 et slides 53173. Aspire attribue les ports API/Python et fournit leurs adresses aux composants; les retrouver dans son dashboard. La copie d’exercice part de la branche préparée issue de 06, indiquée dans le conducteur; elle contient aussi les nouvelles analyses ETL, mais garde la couverture à implémenter.
- Ouvrir le support : `./scripts/start-presentation.ps1` (ou `cd presentation`, `npm ci`, `npm run dev`); http://localhost:53173. Pour le build autonome : `npm run build` puis `npm run preview`. L’installation réseau se fait avant la séance.
- Ouvrir aussi `presentation/exports/formation-hydrodatalab.pdf`. Il contient 28 pages; les interactions sont figées mais leur explication reste lisible.
- Pour régénérer ce PDF : `npm --prefix presentation run export` utilise Chrome/Edge/Chromium et Python avec `pypdf`. Au besoin, définir `SLIDEV_PYTHON_PATH` vers cet interpréteur. Le post-traitement convertit les liens d’annexes en destinations PDF sans serveur; projeter le PDF déjà préparé ne demande aucune de ces dépendances.
- Tester projection 16:9, zoom du navigateur, notifications désactivées et alimentation branchée. Faire une répétition chronométrée sans dépendre du réseau.

### Préparer les jalons et les conteneurs

- `00` à `03` : suivre les commandes propres à chaque jalon dans [CHECKPOINTS](../docs/CHECKPOINTS.md); ne pas appeler les scripts absents d’un jalon.
- `04-pilotage` : exécuter `./scripts/prepare.ps1 -SkipPresentation`, puis `./scripts/start-demo.ps1`. Le support Slidev n’est livré qu’à partir de `05-docker`.
- `05-docker`, `06-specification`, `07-solution` : le script `prepare.ps1` complet est disponible. Au jalon 06, les échecs des tests de couverture sont volontaires avant l’exercice; ils ne signifient pas que l’installation a échoué.
- Préparer et tester les images avec [DOCKER](../docs/DOCKER.md) avant la séance. `prepare-docker.ps1` arrête l’application native. Après une répétition Compose, appeler `./scripts/docker-demo.ps1 -Action Stop` avec le même contexte, puis relancer le natif. Ne lancer qu’une pile à la fois sur 53170.
- La génération Python live s’effectue en mode natif au jalon 06 : les images Docker préparées contiennent la solution finale. Le démarrage sélectionne `.runtime/<branche-ou-tag>`; la copie de secours a aussi son dossier distinct.
- Conserver le PDF et le conducteur dans le dépôt principal, qui ne change pas de tag. `main` contient la solution métier du jalon 07 avec les améliorations de préparation, dont les ports dynamiques, et sert de secours. Les tags conservent leur configuration historique. Le bundle Git ne transporte pas les dépendances ni les images : les préparer sur place ou les transférer comme indiqué dans DOCKER.

## Démo 1 — 4 minutes

1. **0:00–0:45** : ouvrir **Chat: Open Agents Window** dans le dépôt principal, branche `main`. Session = conversation pour une tâche; repérer `/plan` pour une future implémentation, sans lancer une deuxième demande pendant cette mission en lecture seule. Le menu de cette fenêtre ne contient pas actuellement l'agent Plan : utiliser `/plan` dans la session Copilot. Copilot est l'environnement choisi et Auto sélectionne un modèle; ses niveaux seront expliqués ensuite. Présenter les confirmations d'actions.
2. **0:45–1:30** : ouvrir `Ctrl+,`, ajuster la lisibilité et expliquer la portée : Utilisateur pour ses projets, Espace de travail pour le projet ouvert. Repérer **Changes**; ce panneau peut être vide pendant la mission en lecture seule. Le vrai diff sera montré à la démo 3.
3. **1:30–2:20** : ouvrir **Chat: Open Customizations**. Instruction = consigne écrite pour l'agent. Retrouver la règle Python qui distingue zéro d'une absence.
4. **2:20–4:00** : lancer le prompt de découverte du conducteur des démos. Traduire **References** par « sources consultées », ouvrir un fichier cité et vérifier une affirmation avec le public.

Résultat attendu : le public sait où l'agent travaille, quelles actions il peut faire et comment vérifier une affirmation. Si l'authentification est indisponible, montrer les fichiers et les règles locaux en annonçant que la réponse ne sera pas générée en direct. Ne pas créer de worktree pendant cette démonstration.

## Démo 2 — 5 minutes

Objectif Copilot : vérifier par l’exécution, le code et les traces le résultat d’un développement assisté par agent. L’ETL est notre cas concret. Dépôt principal sur main; une collecte complète a déjà réussi avant la séance.

- **0:00–0:45** : commande Python préparée; résumé et hourly.csv. Données locales reproductibles, 168 observations et quatre séries.
- **0:45–2:15** : Vue → Parcours ETL → Extraire, Transformer, Charger / exporter. Lire les champs bruts, le schéma normalisé et les 24 moyennes horaires. Développer un seul indicateur : pointe horaire moyenne 22 009,75 MW à 18 h Toronto. Les autres restent disponibles aux questions.
- **2:15–3:15** : dans VS Code, ouvrir analysis.py et test_analysis.py. Relier la règle des quatre quarts d’heure valides à un test. Retrouver la preuve citée par Copilot à la démo 1, sans parcourir toute l’implémentation.
- **3:15–4:15** : Voir les détails d’un succès : paramètres, étapes Python, durées, exports propres à cette exécution. Un seul journal Aspire si le créneau le permet. Quartz déclenche, SQLite conserve l’historique; les étapes Python arrivent en fin de traitement.
- **4:15–5:00** : commande Docker préremplie, image déjà chargée; comparer le résumé au natif. En cas d’indisponibilité du moteur, annoncer le résultat préparé et revenir aux slides à 5:00.

**Variante choisie avant la séance :** remplacer 3:15–5:00 par échec contrôlé, détails de l’extraction échouée et relance liée à sa première tentative. Dans cette variante, omettre Aspire et Docker. Ne pas cumuler les deux parcours. Le [conducteur détaillé](conducteur-demos.html) conserve les commandes et les valeurs pour la répétition et les questions.

```powershell
docker run --rm --pull never --network none hydrodatalab-etl:formation python -m hydrodatalab run --source fixture --date 2026-10-17 --output /tmp/result
```

Employer le moteur et le contexte validés avant la séance (option --context juste après docker si nécessaire). Les fichiers du conteneur disparaissent avec --rm; les exports persistants sont montrés côté natif. L’égalité complète des résultats se valide avant la séance, pas à partir du seul résumé console. Aucune construction, installation ou bascule de toute la pile Compose en direct. La planification reste désactivée.

## Démo 3 — 4 minutes, seuil de bascule à 2:10

- **0:00–0:40** : montrer « Couverture : Non calculée » dans la copie d’exercice `codex/repetition-01`, préparée depuis `codex/preparation-demo-06`, puis ouvrir `specs/coverage.md`, `etl/hydrodatalab/coverage.py` et `specs/test_coverage_acceptance.py`. Les tests de couverture rouges sont volontaires à cette étape.
- **0:40–2:10** : coller le prompt live. L’agent modifie Python, sans recréer l’API ni Vue. Attendre une sortie de tests, pas seulement « terminé ».
- **À 2:10** : si ce n’est pas prêt, dire « Je passe à la solution préparée pour garder le temps de vérifier. » Arrêter la génération, conserver les modifications dans la copie d’exercice, arrêter ses services et redémarrer le dépôt principal `main`, qui contient la solution préparée avec son runtime séparé.
- **2:10–3:00** : montrer le diff et les tests, dont jours de 23/25 heures.
- **3:00–4:00** : revenir au même écran, lancer scénario complet puis manquant. Déplier **Qualité par série** : le niveau horaire est à 24/24 puis 23/24 = 95,83 %. La couverture globale des quatre séries devient 167/168, soit 99,4 % dans la synthèse : ce n’est pas le même dénominateur. Le retrait est en mémoire; les données originales sont intactes. Revenir à la slide 20 (sélection de skills).

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

Nouvelles captures ETL du 19 octobre : [avant](../docs/screenshots/etl-avant.png), [après](../docs/screenshots/etl-apres.png), [synthèse](../docs/screenshots/etl-synthese.png), [exécution](../docs/screenshots/etl-execution.png).
