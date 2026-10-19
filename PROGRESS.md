# HydroDataLab — journal de reprise

## Progression Copilot et synchronisation du support — 19 octobre

- Départ : main `9629f7c`, dépôt propre. Demande confirmée : appliquer les recommandations 2 à 5; sujet central = GitHub Copilot dans VS Code et bonnes pratiques, HydroDataLab servant d’exemple. Aucun aperçu ETL supplémentaire au début.
- Réalisé : ouverture/objectifs/conclusion centrés sur Copilot; instructions 8–9 suivies du contexte 10, mécanismes 11 et approche vanille 12. Choix des skills en 20; Canvas en annexe 25. Les sept concepts restent couverts avec un exemple commun instruction/skill/outil.
- Notes, conducteur HTML/Markdown, guide et prompt live synchronisés. Démo 2 : résultat exécuté, avant/après, un indicateur, code/test, détails et conteneur; l’échec/relance devient une variante de 3:15–5:00, sans cumuler Aspire et Docker. Démo 3 sur la copie préparée, secours sur main; aucune bascule historique en séance.
- Minutage vérifié : les 22 plages des notes correspondent au guide, sans trou; 32 min d’explications + 13 min de démos, questions 10 min, marge 5 min. Démo 1 en slide 7, démo 2 en 17, SDD en 18, démo 3 en 19. L’application, les branches d’exercice et les tags sont inchangés.
- Validé : build, 28 routes sans débordement détecté ni erreur de page, interactions et synchronisation des deux vues (contexte 10/coûts 15). PDF de 28 pages, liens officiels et quatre destinations internes; rendus des pages modifiées et planches inspectés. L’export signale un avertissement FloatingVue de l’outillage, sans effet constaté sur le PDF ou le site compilé. Conducteur : 8 rubriques, 21 copies, 4 largeurs, liens et impression; 19 blocs PowerShell et 2 scripts analysés sans erreur syntaxique. Preuves : docs/validation/presentation-flow.json et presentation-guide.json, presentation/exports/qa/.
- Prochaine action : enregistrer la révision et actualiser le kit existant avec sauvegarde Git, puis reprendre la répétition humaine sur le portable HQ. Aucun nouveau kit complet ni archive Docker supplémentaire.

## ETL visible et détails des exécutions — 19 octobre

- Départ : main `101de0f`, dépôt propre. Demande : avant/après ETL, synthèse horaire, indicateurs Python, détails des exécutions et conducteur actualisé; Quartz conservé.
- Décisions : agrégation de la demande par heure complète (4 créneaux valides), pointe horaire moyenne, estimation rectangulaire de l’énergie demandée et variation du niveau observé. Aucune prévision ni conversion du niveau en réserve. Les calculs restent indépendants du calcul de couverture à compléter en démo 3.
- Préserver les anciens rapports et l’historique SQLite. Ajouter des diagnostics d’étapes Python, les paramètres et le lien entre une relance et sa tentative précédente.
- Réalisé : calculs purs dans analysis.py, avant/après et synthèse Vue, hourly.csv, étapes mesurées Python et détails persistants API, relances liées. Compatibilité anciens rapports/SQLite.
- Validé : 43 tests Python, 10 .NET, 11 Vue et build. Navigateur réel : 24 heures, export, erreur/relance, anciens rapports, 320 px et texte 200 %. Audit typographique : 17 états/formats et contraste >=4,5:1. Captures inspectées.
- Enregistré : fonctionnalité `66d8bd8`. Trois images reconstruites; parité Python locale/conteneur hors réseau (deux CSV, Parquet, rapport métier) et parcours complet Vue/erreur/relance sur Compose réussis. Nouvelle archive complète vérifiée : SHA-256 `7e5a7b8d6258cd2ee2bf62c0b6e3bfebe7d2cbef2880981e61ee923e4d312046`. Elle remplace l’archive source et celle du kit; ne plus charger l’ancien complément Vue.
- Préparation pédagogique : un conflit documentaire constaté lors du cherry-pick a été résolu dans une copie dédiée. Branche `codex/preparation-demo-06`, commit `9e1ecde478f4f5d70634a42c720fa10fd224eab7`, descendant de 06 avec trois améliorations. Couverture identique au tag 06, 8 tests d’analyse réussis et les 2 tests d’acceptation de couverture rouges attendus. Clone via bundle et création de branche testés. Les tags historiques sont intacts; aucune copie d’exercice de l’utilisateur n’a été modifiée.
- Conducteur actualisé (4/5/4 min), guide du présentateur, dictionnaire/contrat, documentation Docker et quatre captures de secours. Lecteur HTML : 8 rubriques, 21 copies, 4 largeurs, impression et liens vérifiés; 19 blocs PowerShell et scripts de kit syntaxiquement valides. Preuves dans `docs/validation/etl*.json`.
- Compose arrêté sans supprimer les volumes; application native main redémarrée et saine sur http://localhost:53170. Dernière validation .NET faite dans un répertoire de compilation isolé pour éviter les verrous Windows.
- Livraison : kit existant actualisé et inventaire SHA-256 vérifié; archive consolidée, guides, captures et bundle contenant la branche de préparation. Copies temporaires de validation retirées après vérification; aucune copie utilisateur modifiée. Le reçu final et le bundle figurent dans `artifacts/LIVRAISON.txt`.
- Prochaine action : rafraîchir Vue, explorer « Parcours ETL » puis « Voir les détails », et reprendre le conducteur actualisé sur le portable HQ. La répétition humaine reste à effectuer. Aucun nouveau kit complet ni réécriture des tags.

## Lisibilité de l’interface Vue — 19 octobre

- Départ : main `18c7615`, dépôt propre. Réviser toute la typographie Vue et inscrire des critères vérifiables dans les instructions permanentes.
- Audit : textes de 9 à 12 px, couleurs secondaires trop pâles et SVG qui réduisent leurs libellés avec la largeur. Conserver la palette et les parcours; adopter 18 px pour la lecture, 16 px pour les informations secondaires et des graphiques dont les textes ne rétrécissent plus.
- Références : WCAG 2.2 (contraste, agrandissement, redistribution, espacement) et documentation officielle des instructions VS Code. Les tailles choisies sont une convention du projet, pas un minimum WCAG universel.
- Réalisé : Segoe UI/Aptos, corps 18 px, secondaire 16 px, tailles en rem, titres 20/24/32–48 px; panneau élargi, contrastes renforcés, redistribution des colonnes et tableaux défilables au clavier. SVG mesurés avec ResizeObserver : aucune réduction des lettres; graduation assez précise pour distinguer les petits écarts de niveau d’eau.
- Instructions permanentes : renvoi depuis le socle commun et règles `web/**` dans `vue.instructions.md`. Critères, sources, portée User et distinction convention/WCAG dans `docs/UI_TYPOGRAPHY.md`; contrôle reproductible `web/qa/typography.mjs`.
- Validé : 11 tests Vue et build réussis; 17 états/formats, texte >=16 px, contrastes >=4,5:1, pas de débordement de page ni de texte SVG coupé, tableaux ouverts, clavier et filtres, texte 200 %, redistribution 320 CSS px, espacement personnalisé. Pas d’erreur JavaScript ni de requête navigateur externe. États supplémentaires simulés dans le navigateur uniquement; aucune collecte ni modification des données/planification. Captures examinées. Preuve : `docs/validation/typography.json`. Le vrai projecteur et le zoom manuel restent à vérifier à la répétition HQ; pas de certification WCAG revendiquée.
- Commit de typographie `30d9b5e7e06d9fe0dbbff3cc8aafc47bb3464f18`, appliqué sans conflit après le commit réseau sur une copie temporaire de 06. Python et spécification restent strictement identiques au tag 06. Conducteur et checkpoints mis à jour; aucun tag historique réécrit.
- Conteneur Vue reconstruit depuis le Dockerfile existant. CSS/JS identiques au build natif par SHA-256; archive séparée de 49 800 192 octets, chargement et configuration nginx vérifiés sans réseau ni port publié. `docs/DOCKER.md` précise son chargement après la grande archive conservée. Ne pas dupliquer le kit complet. L’AppHost du présentateur, déjà actif au début, reste lancé; Compose n’est pas redémarré pendant sa répétition.
- Prochaine action : rafraîchir Vue et le conducteur; utiliser les deux commits de préparation pour une nouvelle tentative. Le commit documentaire et le bundle final sont indiqués dans `artifacts/LIVRAISON.txt` après actualisation du kit existant.

## Ports API et Python dynamiques — 19 octobre

- Départ : main `f38def6`, dépôt propre; aucun AppHost actif. Demande explicite : laisser Aspire choisir les ports API/Python et garder Vue sur 53170.
- Réalisé : ports API/Python attribués par Aspire, références d’endpoints conservées, replis fixes supprimés de .NET et Vite. Vue reste sur 53170 et sert aussi de point d’entrée aux validations HTTP. Commit de configuration `036bb193c778be0ff5a8f1c9681ee2b40f666397`, applicable par cherry-pick sur le jalon 06. Aucun tag historique réécrit.
- Validé : démarrage natif avec les anciens ports 53171/53172 volontairement réservés; trois ressources saines, adresses transmises automatiquement, parcours HTTP complet via Vue (168 observations, couverture, panne, relance, trois exports). Tests : 8 .NET et 11 Vue réussis, compilation Vue réussie. Navigateur : trois graphiques, couverture 100 % puis 95,83 %, requêtes limitées à l’origine Vue, aucune erreur JavaScript.
- Exercice : clone de contrôle issu de 06 puis cherry-pick sans conflit; préparation et démarrage réussis, nouveaux ports API/Python, Vue sur 53170. Couverture « Non calculée » visible, deux échecs d’acceptation attendus et aucun échec d’installation. Preuve sans variables sensibles : `docs/validation/dynamic-ports.json`.
- Le clonage direct du dossier échouait à cause de la propriété Windows du dépôt source. Le conducteur utilise maintenant un bundle Git local, testé avec succès, sans modification de permissions ou de configuration globale. Une tentative déjà créée peut conserver son travail puis créer une nouvelle branche avec cette correction.
- Guides, README et contrat actualisés; les 19 blocs PowerShell sont syntaxiquement valides. Lecteur HTML : huit rubriques, 21 boutons, copie et repli Ctrl+C, quatre largeurs et impression contrôlés. Les slides et le PDF ne changent pas.
- Les deux applications de contrôle sont arrêtées. Copie temporaire `.runtime/ports-exercise` retirée après vérification du chemin, de la branche et de l’absence de changements : 365 991 242 octets libérés. Bundle de contrôle et description brute Aspire retirés aussi. Actualiser le kit existant; le reçu `artifacts/LIVRAISON.txt` identifiera le commit final et le bundle. Prochaine action : rafraîchir le conducteur et reprendre la préparation de l’exercice sur le portable.

## Prérequis du portable et rôle des ports — 19 octobre

- Retour du présentateur : diagnostic exécuté sur son poste personnel; outils présents, accents mal interprétés. La répétition sur le portable HQ reste à effectuer.
- Explication vérifiée dans les sources du projet et les documentations officielles : uv local pour CLI/tests/Aspire natif; uv déjà embarqué dans l’image Python, sans besoin local pour exécuter les conteneurs préparés. Les ports hôtes ont été fixés pour les URL de démo; Aspire peut les attribuer dynamiquement si cette contrainte est retirée.
- Correction : le conducteur précise PowerShell 7, déjà prévu par le README. `doctor.ps1` affiche la version du shell et refuse une version antérieure avec un message ASCII lisible, avant les messages accentués. Pas de modification des ports ni des services.
- Validé : diagnostic complet sous PowerShell 7.6.7, accents lisibles; arrêt explicite et lisible sous Windows PowerShell 5.1. Guide HTML régénéré avec les mêmes 8 rubriques. SHA de livraison dans `artifacts/LIVRAISON.txt`. Prochaine action : relancer le diagnostic dans `pwsh`, puis préparer la copie d’exercice selon le conducteur.

## Conducteur de répétition — 19 octobre

- Départ : main `f9ad107`, dépôt propre. Demande : guider les trois démos pas à pas et fournir un document lisible pendant la présentation.
- Parcours : main pour les démos 1 et 2 et le secours, une seule copie Git d’exercice sur une nouvelle branche issue de `06-specification` pour la démo 3. L’application main et celle de `07-solution` sont identiques (`etl`, `src`, `web`, `specs`). Aucun changement de branche du dépôt principal, aucun clone d’exercice créé automatiquement, aucun nouveau kit complet.
- Réalisé : `handouts/conducteur-demos.md`, lecteur HTML hors ligne avec navigation et copie, générateur dans `presentation/scripts/build-demo-guide.mjs`. Préparation, commandes, prompts, libellés Vue, preuves, bascule pendant SDD, secours à 2:10, conservation de la tentative et reprise après incident.
- Contrôlé : prérequis présents; CLI fixture exécutée, 168 observations et 4 séries; 35 tests Python réussis. Node trouvé dans le terminal sans profil : 25.2.1; Node 24 reste la version recommandée pour le portable. Aucun lancement Docker ou répétition humaine HQ revendiqué.
- Validé : 8 rubriques, 21 boutons de copie, liens locaux, affichage à 1440/1280/900/390 pixels, zéro requête externe ou erreur JavaScript. Repli Ctrl+C et affichage de toutes les sections à l’impression contrôlés; captures inspectées. Les 19 blocs PowerShell passent l’analyse syntaxique.
- Contrôle du jalon 06 dans une extraction temporaire : deux échecs d’acceptation dus à `coverage = None`, comme prévu. Ajout de la création de `etl/.test-output` au guide, nécessaire sur une copie neuve. Aucun basculement du dépôt principal ni modification de la fonction de solution.
- Enregistrement : commit « Ajoute le conducteur des trois démos et la répétition sans perte ». Actualiser le kit existant avec `refresh-kit.ps1`; le reçu `artifacts/LIVRAISON.txt` identifiera le SHA et la sauvegarde livrés.
- Prochaine action : suivre « Avant la séance » sur le portable, préparer la copie d’exercice et chronométrer la transition ainsi que les démos 4/5/4 minutes. Le guide est prêt; cette répétition humaine reste à faire.

Mis à jour : 19 octobre 2026. Formation cible : mardi 20 octobre 2026.

## État
- [x] Plan accepté : public BI/Python, formation française, web local + PDF.
- [x] Données publiques capturées et provenance conservée (17 octobre : 168 observations).
- [x] ETL Python, tests, CLI et adaptateur HTTP.
- [x] API .NET/Quartz/SQLite, Vue et Aspire, parcours natif intégré.
- [x] Parcours Docker Compose et comparaison avec le mode natif.
- [x] 25 diapositives (19 principales + 6 annexes), PDF, notes et aide-mémoire contrôlés.
- [x] Huit tags annotés, branche demo-live, scripts de reprise et restauration Git réelle.
- [x] Quatre images archivées avec manifest et empreinte SHA-256.
- [x] Kit de transfert hors ligne assemblé; images, bundle et inventaire SHA-256 vérifiés.
- [x] Vérification navigateur et répétition technique sur le poste de construction.

## Décisions
- Python porte les transformations. .NET pilote et conserve l'historique; Vue présente les résultats.
- Aspire 13.6.1, .NET 10, Quartz avec stockage SQLite.
- Les niveaux en mètres ne sont pas des volumes ou des stocks énergétiques.
- Aucun envoi vers un dépôt distant ni déploiement cloud.
- Données locales par défaut; accès direct aux données publiques explicite.

## Reprise
Lire ce journal, puis `git status --short --branch` et `git log -5 --oneline`.
Ne pas écraser les modifications existantes. Le travail des agents est réparti entre `etl/`, `web/`, `presentation/` et le reste du dépôt.

## Prochaine action
Révision pédagogique terminée et contrôlée : notions expliquées avant emploi, consignes d'animation dans les notes, site et PDF reconstruits. Copier le dossier `artifacts/kit-hq-<date-heure>/` le plus récent sur le portable HQ; `artifacts/LIVRAISON.txt` identifie le kit de référence. Répéter le 19 octobre et confirmer les accès Copilot. La version technique de l'application reste validée.

Dernier jalon technique validé : `07-solution` = `4a462eec09f6f2cc62b4ec0ca36fa4a6a033bd78`. `demo-live` pointe sur `06-specification` = `114d57d137c4064ab9617bb736e187421c83afdc`. Les commits de livraison suivants portent sur les scripts de transport et la documentation.

Dernier commit d'outillage validé : `5e5b476` (assemblage du kit). Le commit de clôture suivant ajoute ce bilan et la preuve navigateur. `git log -1` donne la version exacte de livraison.

## Vérifications
- Python : 35 tests réussis; lecture réelle de la source publique vérifiée; date CLI, idempotence, changements d'heure et interruptions couverts.
- API : compilation .NET 10 et 8 tests SQLite réussis.
- Vue : compilation et 11 tests réussis; parcours Edge complet et mobile validé, captures dans docs/screenshots.
- HTTP natif : collecte, scénario manquant 95,83 %, échec/relance, trois exports vérifiés.
- Slidev : 24 diapositives, aucun débordement ni requête externe; interactions vérifiées, PDF produit.
- Git : huit jalons annotés de 00 à 07, histoire linéaire, branche `demo-live` au jalon 06. Aucun dépôt distant.
- Image ETL finale testée sans réseau : CSV identique, Parquet et JSON métier équivalents au mode natif.
- Aspire : publication Compose réussie; images API, Vue, ETL et dashboard téléchargées/construites.
- 16:16 : Compose sain et parcours HTTP complet validé (collecte, couverture, panne, relance, exports). Le blocage venait de la liaison named pipe Docker; construction Podman directe puis API temporaire localhost dans le namespace systemd. Aucun service global redémarré. Voir docs/VALIDATION.md.
- Persistance réelle : restart API conserve dernier rapport et planification; planification désactivée après vérification.
- Préparation native exécutée avec succès; audit résiduel des outils Slidev documenté dans presentation/DEPENDENCY-AUDIT.md.
- Scripts de reprise : runtime séparé par branche/tag; changements Git refusés si modifications locales.
- Jalon 06 vérifié : deux acceptations volontairement rouges, CLI fonctionnelle avec 168 observations et couverture absente. Solution restaurée : 35 tests Python verts.
- Compose : 14 contrôles navigateur réussis, aucune requête externe du navigateur. Conteneurs arrêtés proprement après les tests; volumes conservés.
- Docker Desktop : backend indisponible; client Podman direct et API temporaire locale utilisés pour les validations. Ne pas dépendre de la liaison named pipe sur ce poste.
- Ports réservés à cette démo : Vue 53170, API 53171, Python 53172, présentation 53173.
- Réparation autorisée explicitement le 18 octobre : Git avait normalisé les fins de ligne des données brutes. Les jalons 02 à 07 conservent maintenant leurs octets exacts et leurs empreintes; tags et branche live mis à jour atomiquement. Ancienne sauvegarde conservée, mais ne pas l'utiliser pour présenter.
- Restauration réelle : six CLI réussies, intégrité des sources vérifiée à chaque jalon; 06 présente deux échecs d'acceptation attendus; 07 passe les 35 tests Python. Preuves dans `docs/validation/`.
- Archive d'images : 1 205 382 656 octets, quatre IDs vérifiés; SHA-256 `3c3c775373607b6f6cec5f2bdea7cc9506bfe4c5182e9df04bfce8ea4690ebfe`.
- Kit : code et histoire Git, images, Compose complet, données/licences, présentation statique, PDF, notes et captures; chaque fichier est inventorié dans `SHA256SUMS.json`. Aucune dépendance npm n'est requise pour sa projection.
- Serveur Python du kit : 24 routes de diapositives, deux interactions et `/presenter/1` vérifiés avec les accès externes bloqués; aucune erreur JavaScript. Preuve `docs/validation/kit-browser.json`.

## Révision pédagogique du 18 octobre

- Introduction de modèle/outils/agent puis dépôt/commit/branche/worktree; worktree explicitement facultatif.
- Démonstrations orientées vers une question et un résultat pour l'audience. Minutages, raccourcis et secours conservés dans les notes et le guide.
- Définitions de session, instructions, skill, contexte, token, API, image Docker et spécification; lexique dans l'aide-mémoire.
- Guides et requêtes alignés, notamment `/plan` dans la fenêtre Agents. Les huit tags historiques sont conservés; utiliser les supports récents de main/du kit pendant les démos.
- Site, PDF de 24 pages et interactions contrôlés. Lien direct `/presenter/17` réparé et notes réellement vérifiées; preuve `docs/validation/pedagogy.json`.

## Préparation sur le portable HQ
La répétition du 19 octobre et la confirmation des politiques/modèles HQ nécessiteront le portable et le compte du présentateur. Elles ne sont pas encore effectuées.

## Révision des personnalisations et nouveautés 1.142

- Point de reprise : main `3e58862`, dépôt propre avant cette révision; huit tags pédagogiques conservés.
- Sources officielles revérifiées le 18 octobre : harnais Copilot/SDK, instructions, migration, plugins, hooks, Canvas, tarifs et HydraFusion.
- Décisions : conserver 18 diapositives + 6 annexes et 45 minutes; expliquer composition et activation sans inventer un ordre universel de priorité. Parcours Copilot en priorité; AGENTS.md pour le socle partagé entre harnais compatibles.
- Réalisé : exemple numérique zéro/absence, sources cliquables, composition animée Python/Vue, sept concepts, capture utilisateur reliée aux portées, graphique avec quatre hypothèses de tâches, Canvas et SDD. Notes, minutage et trois fiches synchronisés; nouvelle fiche `handouts/personnalisations-vscode.md`.
- Site validé : 24 routes, aucune erreur ni ressource externe, aucun débordement; sélection de règles, quatre scénarios de coût, tentatives, réduction des animations et notes présentateur contrôlés.
- PDF : 24 pages inspectées visuellement; liens officiels vérifiés. Les liens d’annexes sont convertis en destinations PDF locales par `fix_pdf_links.py`; export reproductible avec Python/pypdf.
- Révision validée enregistrée : `434d66a`. Le commit de clôture ajoute ce bilan. Assemblage du kit confirmé : 216 fichiers, archive d’images et bundle Git vérifiés, quatre renvois PDF internes fonctionnels sans serveur.
- Prochaine action : copier le kit indiqué dans `artifacts/LIVRAISON.txt` sur le portable et un support distinct; effectuer la répétition et vérifier le compte HQ le 19 octobre. Les supports les plus récents restent sur main; les huit tags pédagogiques sont conservés.

## Révision ciblée : couches bilingues et comparaison simultanée

- Départ : main `359f7f1`, dépôt propre. Demande : clarifier les termes et la place visuelle des couches; comparer les tâches et modèles sans devoir choisir une seule tâche.
- Réalisé : diapositive 10 avec cinq couches bilingues, exemples de fichiers, conditions d’utilisation et flèches vers le contexte du modèle; diapositive 13 avec quatre tâches et cinq modèles visibles simultanément, valeurs, barres comparables et recommandations. Tarifs officiels revérifiés le 18 octobre, hypothèses inchangées.
- Correction du chevauchement signalé : en-têtes raccourcis, volumes dans leur cellule, ligne de conseil redondante retirée, espace réservé aux hypothèses et aux sources. Les cinq états de la diapositive 10 sont également vérifiés.
- Validations réussies : build Slidev; 24 routes sans débordement ni erreur JavaScript, accès externes bloqués; 20 coûts et multiplicateur contrôlés; PDF de 24 pages avec liens officiels et renvois internes, pages 10/13 inspectées. Notes, aide-mémoire et fiches synchronisés. Preuve : `docs/validation/context-cost-comparison.json`.
- Point de reprise : cette révision est enregistrée dans le commit portant « Clarifie les couches de contexte et compare les coûts sans chevauchement ». Le script `package-kit.ps1` assemble ensuite la sauvegarde transportable; son résultat et le SHA du commit sont consignés dans `artifacts/LIVRAISON.txt`. Les huit tags pédagogiques restent inchangés.
- Prochaine action : transférer le kit indiqué par ce reçu, puis répéter sur le portable HQ le 19 octobre.

## Mise en contexte du public et moment fort

- Départ : main `aa97bf4`, dépôt propre. Ajout demandé d’une ouverture sur le développement BI/Python local, la préparation au déploiement en conteneur, la marge d’expérimentation et la sécurité quotidienne.
- Réalisé : nouvelle diapositive 2 « Votre terrain de développement », avec trajet développer/tester localement/préparer le déploiement conteneurisé, marge d’expérimentation et réflexes de sécurité. Source Microsoft S31 liée; la souplesse du cadre reste le contexte fourni par le présentateur, sans inventer de politique HQ.
- Durée conservée : couverture 30 s, contexte 1 min, parcours 30 s; 32 min d’explications + 13 min de démos. Support de 25 pages, renvois et fiches renumérotés. La démo Python devient la diapositive 18.
- Moment fort précisé dans les notes et le conducteur : « Non calculée » → code Python et tests → niveau horaire à 100 %, puis 95,83 % dans le scénario manquant. Infrastructure préparée et solution de secours annoncées clairement; leur présentation en direct reste à répéter sur le portable HQ.
- Validé : build; 25 routes sans erreur, débordement ou ressource externe; interactions et notes; PDF de 25 pages, liens et destinations internes, inspection des planches et de la nouvelle page. Preuve : `docs/validation/audience-context.json`. Le serveur de présentation arrêté a été relancé sur 127.0.0.1:53173.
- Enregistrement : commit « Ajoute le contexte du public et précise le moment fort ». Le kit et le SHA exacts seront consignés par le reçu `artifacts/LIVRAISON.txt` après assemblage; tags pédagogiques conservés.
- Prochaine action : transférer ce kit et répéter l’avant/après le 19 octobre sur le portable HQ.

## Révision « vanille », skills et ressources — 19 octobre

- Départ : main `b40c95f`, dépôt propre. Ajouter une position nuancée sur les personnalisations, une sélection de skills et une page finale de ressources avec emplacements HQ modifiables. Conserver 45 minutes de contenu et les interactions entre les deux vues.
- Audit : `artifacts` contient 11 131 997 052 octets, dont huit kits avec la même archive Docker. Suppression des sept kits du 18 octobre explicitement autorisée par le présentateur; conserver le kit du 19 octobre, la source Docker et tous les bundles/ZIP Git.
- Ménage effectué : 8 571 725 646 octets supprimés, 2 560 271 406 octets conservés avant la nouvelle sauvegarde. Chemins des sept dossiers vérifiés, aucune jonction suivie, archive et bundle conservés contrôlés. Preuve : `docs/validation/storage-audit.json`.
- Réalisé : pages 11 (approche vanille), 12 (skills selon la tâche; Matt Pocock en exploration) et 22 (veille et trois emplacements HQ). Liens éditables dans `presentation/resources/links.json`; fiche `handouts/skills-et-veille.md`. Aucun lien interne fourni à ce stade, aucune adresse inventée, aucun skill tiers installé.
- Sources primaires vérifiées le 19 octobre; aucun consensus « must have » affirmé. Minutage conservé : 32 minutes d’explications, 13 minutes de démos, 10 minutes de questions, 5 minutes de marge. Renvois et notes actualisés : contexte 13, coûts 16, Python live 20, annexes 23–28.
- Validé : 28 routes sans débordement, erreur de page ou accès externe; ressources et emplacements HQ contrôlés; synchronisation des deux vues réussie. PDF de 28 pages, liens officiels et quatre renvois internes vérifiés; planches et nouvelles pages inspectées.
- Livraison : nouveau script `refresh-kit.ps1` pour actualiser le support et la sauvegarde du kit existant sans recopier Docker. Après enregistrement Git, l’exécuter sur le kit du 19 octobre puis contrôler son serveur Python; le reçu `artifacts/LIVRAISON.txt` identifiera le commit livré.
- Prochaine action : compléter les liens HQ si disponibles, puis répéter sur le portable avec le kit indiqué dans le reçu.

## Synchronisation des interactions entre les deux écrans — 19 octobre

- Départ : main `d42ba5c`, dépôt propre. Demande : piloter les couches de contexte et les coûts depuis la vue présentateur.
- Réalisé : partage des couches de contexte (11) et du nombre d’essais (14), avec sauvegarde locale, valeurs validées et mise à jour entre fenêtres du même navigateur/profil/origine. Les deux contrôles restent indépendants. VueUse 14.4.0, déjà installé, est déclaré explicitement; aucune autre version modifiée.
- Validé : build réussi; cinq couches, 20 coûts, bornes 1/8, navigation native, rechargement et ouverture tardive, dans deux pages dont la vraie vue présentateur, sans requête externe ni erreur JavaScript. Les 25 diapositives passent le contrôle général sans débordement. Captures inspectées; PDF statique inchangé.
- Guides mis à jour pour piloter depuis le portable. Test : `presentation/scripts/qa_presenter_sync.mjs`; preuve : `docs/validation/presenter-sync.json`.
- Enregistrement : commit « Synchronise les interactions entre présentateur et audience ». Le kit, son SHA et sa validation seront consignés dans `artifacts/LIVRAISON.txt` après assemblage. Les tags pédagogiques restent inchangés.
- Prochaine action : utiliser le kit indiqué dans ce reçu, recharger les deux fenêtres et répéter sur les deux écrans du portable HQ.
