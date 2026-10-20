# HydroDataLab — journal de reprise

## Portée et chargement progressif — 19 octobre

- Départ : main `314dbcd`, propre et synchronisé. Demande : clarifier les slides 10–11, leurs notes et l’aide-mémoire sans allonger la séance.
- Décision : séparer les deux axes. « Task scoped » décrit la portée; « On demand » décrit un chargement qui peut être choisi par l’agent ou demandé explicitement. La skill locale validate-etl reste volontaire (`disable-model-invocation: true`).
- Réalisé : portée projet/fichiers/tâche en slide 10; chargement automatique, conditionnel et lecture/retour en slide 11, avec progression nom/description → SKILL.md → références. Exemples du brut, de coverage.py, de /validate-etl et du test 23/24. Notes en quatre séquences courtes par slide; aide-mémoire, fiche, guide et sources alignés.
- Vérifié : build; audience en 1366×768 et 1920×1080, sans débordement; notes sur portable, détails repliés et ouverture au clavier; trois états synchronisés, retour audience → présentateur, rechargement et navigation conservant la sélection. PDF de 31 pages et six destinations internes contrôlé, pages 10–11 examinées. Message FloatingVue/Popper déjà connu sans effet constaté. Preuves : docs/validation/context-scope-loading.json et presentation/exports/qa/VALIDATION.md.
- Minutage : 16:30–18:00 / 18:00–19:00 conservés; 25 plages notes/guide concordantes, 32 + 13 minutes. Prochaine étape : commit et publication sur main, puis pull/rebuild sur HQ et répétition humaine. Aucun changement aux tags ou à la branche d’exercice.

## Architecture du contexte par paliers — 19 octobre

- Départ : main `dac5d19`, propre et synchronisé. Demande : remplacer « socle » par un libellé concret et expliquer la tiered-context architecture sur deux slides si nécessaire.
- Vérifié : documentation Microsoft actuelle sur les instructions, le contexte implicite/explicite et le context engineering. `applyTo` est conditionnel; les documents ne deviennent pas permanents par leur seule présence; la fenêtre Agents désactive le contexte implicite par défaut. Séparer contexte du modèle et configuration/permissions du harnais.
- Réalisé : règles communes en slide 9; paliers en 10 et composition interactive en 11. MCP/permissions distingués du contenu transmis au modèle; conditions des documents, chemins, rôles, prompts/skills et IDE/CLI détaillées. Notes, guides, conducteur HTML et renvois renumérotés. 31 slides, dont 25 principales; 32 + 13 minutes conservées, démos aux mêmes horaires.
- Vérifié : build; 31 routes sans débordement; rendus 9–11 en 1366×768 et 1920×1080; notes 10–11, repli et clavier; cinq sélections synchronisées, persistance au rechargement et retour audience → présentateur; coûts à 1/2 essais sur la nouvelle slide 17. PDF de 31 pages, textes/liens et six destinations internes contrôlés; pages modifiées examinées. Message console FloatingVue/Popper connu sans effet constaté. Preuves : docs/validation/tiered-context.json, presentation-flow.json et presentation/exports/qa/VALIDATION.md.
- Livraison : commit validé `7b34dde`, publié sur origin/main. Sur le portable HQ, pull, rebuild et rechargement des deux vues; répéter les slides 9–11 en quatre minutes. Aucun changement de tags, de branche d’exercice, de données ou d’images Docker.

## Capture réelle Artificial Analysis — 19 octobre

- Départ : main 0c4492c, propre et synchronisé. Demande : remplacer le schéma par une vraie capture et des comparaisons modèle/effort/coût.
- Preuve enregistrée : six infobulles publiques Sol/Codex et Sonnet/Claude Code, Low/High/Max; lectures et réglages dans docs/screenshots/artificial-analysis/. Capture réelle locale dans presentation/public/. Aucun export de données payant utilisé.
- Réalisé : vraie capture sur la slide 17, tableau des six valeurs, comparaisons Sol High/Max (+74 %, indice affiché 60) et Sonnet High/Max (×11,4, indice 55 → 68). Indices arrondis, axe logarithmique, harnais et distinction coût API/facture Copilot explicites. Notes de 90 secondes, guide, matrice, aide-mémoire et sources alignés. Compteur du diaporama corrigé pour utiliser le total réel.
- Vérifié : build; slide 17 audience/présentateur en 1366×768 et 1920×1080, aucun débordement, marges de 23,6/33,2 pixels; notes ouvertes au clavier, repliées après rechargement. PDF de 30 pages régénéré, textes/liens et six destinations internes validés, page 17 inspectée. Avertissement FloatingVue connu sans défaut constaté. Les 24 plages notes/guide concordent : 32 + 13 minutes, slide 17 à 26:30–28:00. Preuves : docs/validation/thinking-effort.json et presentation/exports/qa/VALIDATION.md.
- Livraison : commit de contenu validé `d646f7c`, publié sur origin/main. Prochaine action côté HQ : faire un pull, reconstruire Slidev et recharger les deux vues; répéter les deux comparaisons dans leur créneau de 90 secondes. Tags, branche d’exercice, Docker et ZIP existant inchangés.

## Thinking effort et coût par tâche — 19 octobre

- Départ : main `e86847f`, propre et synchronisé avec origin/main. Demande : ajouter une slide après la 16 et référencer les benchmarks de code/coût d’Artificial Analysis.
- Réalisé : slide 17 visuelle, niveaux de raisonnement, différence avec Auto, coût des reprises et lecture du benchmark. Sources Microsoft et Artificial Analysis relues le 19 octobre; pas de classement ou de points inventés. Notes ventilées, guide, conducteur HTML, aide-mémoire et matrice actualisés.
- Minutage validé : 30 slides, dont 24 principales; 32 minutes d’explications et 13 de démos. Auto 1 min, matrice 1 min 30, nouvelle slide 1 min 30, HydraFusion 30 s; horaires des démos inchangés. Les 24 plages notes/guide concordent.
- Vérifié : build, 30 routes sans débordement/erreur/requête externe, interactions contexte/coûts, nouvelle slide audience/présentateur en 1366×768 et 1920×1080, notes repliables au clavier. Chevauchement initial corrigé. PDF de 30 pages, textes/liens officiels et six destinations internes validés; page 17 et planches examinées. Avertissement FloatingVue d’export inchangé, sans défaut constaté. Preuves : docs/validation/thinking-effort.json, presentation-flow.json et presentation/exports/qa/.
- Livraison : dernier commit de contenu validé `4145ea3`, publié sur origin/main; référence distante vérifiée. Prochaine action côté HQ : pull en avance rapide, reconstruire le site et recharger les deux vues, puis répéter le créneau 24:00–28:30. Aucun changement des tags, de la branche d’exercice ou des images Docker; ZIP antérieur conservé.

## Synchronisation GitHub privée — 19 octobre

- Départ : main `f34f976`, dépôt propre, aucun distant. L’utilisateur demande explicitement la création d’un dépôt GitHub privé et la publication de main, codex/preparation-demo-06 et des huit tags.
- Cette autorisation remplace la restriction de publication locale pour ce seul dépôt privé; aucun déploiement cloud n’est demandé.
- Créé : https://github.com/lvmajor/HydroDataLab. Visibilité PRIVATE confirmée par l’API avant transfert. Lecture des 635 blobs des références à publier : aucun fichier de plus de 100 Mio ni motif de clé privée/jeton détecté par le contrôle ciblé.
- Préparé : `handouts/synchronisation-github.md`, lien README et guide HQ. Le distant du bundle peut rester sous le nom `kit`; GitHub devient `origin`. Vérification d’un dépôt propre, arrêt sur divergence et séparation de la branche d’exercice. La règle commune de publication est alignée sur l’autorisation explicite.
- Publié : commit documentaire `fdb5c92`, main et codex/preparation-demo-06, huit tags annotés. Origin HTTPS configuré; suivi des deux branches installé. La branche locale demo-live n’est pas publiée. Aucun tag réécrit.
- Validé : dépôt PRIVATE, branche par défaut main, dix références distantes identiques aux références locales. Sept blocs PowerShell parsés. Test depuis le bundle `eba8d19` du ZIP : mise à jour en avance rapide, distant kit préservé, suivi origin/main, branche d’exercice disponible, deuxième exécution sûre; arrêt et conservation d’un fichier local non enregistré. Preuve : `docs/validation/github-sync.json`. Copie temporaire de validation retirée après vérification.
- Prochaine action : sur le portable HQ, ouvrir le guide `handouts/synchronisation-github.md` depuis GitHub et exécuter la connexion dans le dépôt restauré. Le ZIP garde Docker et le site de secours; lancer la version actuelle depuis le dépôt Git. Le portable HQ n’a pas été testé directement. La présente entrée de clôture est publiée après le commit initial contrôlé.

## Repères MCP et VS Code 1.142 — 19 octobre

- Départ : main `0987859`, dépôt propre. Ajouter les emplacements Copilot/Local à la slide 11, aux notes et à l’aide-mémoire.
- Vérifié : les notes 1.142 (14 octobre) annoncent le déploiement progressif de Copilot par défaut; elles précisent que ce harnais était déjà disponible. Les destinations MCP viennent du guide de migration et de la documentation MCP actuelle, pas d’une annonce de création de ces chemins dans 1.142.
- Réalisé : encadré comparatif sur la slide 11, notes minutées avec précisions repliables, aide-mémoire, fiche des personnalisations et conducteur du présentateur. 29 slides et 32 + 13 minutes conservées. Capture accessible en annexe 24. Aucune migration de configuration personnelle ou du projet.
- Validé : build, correspondance des 23 plages notes/guide, encadré et notes au clavier en 1366×768 et 1920×1080 sans débordement, rendu audience/présentateur inspecté. PDF de 29 pages actualisé, page 11 et liens vérifiés. Preuves : `docs/validation/mcp-locations.json`, `presentation-flow.json` et `presentation/exports/qa/`.
- Prochaine action : recharger les deux vues Slidev puis poursuivre la répétition. Le ZIP `eba8d19` reste une archive antérieure à cette retouche; il ne contient pas les repères MCP ajoutés ici.

## ZIP allégé pour le portable HQ — 19 octobre

- Départ : main `1437495`, dépôt propre. Demande : une archive unique pour les slides et les trois démos, Docker inclus, avec guide de préparation du portable.
- Réalisé : packager direct `scripts/package-transfer.py`, un bundle limité à main/exercice/huit tags, une archive de quatre images Linux/amd64, site compilé, PDF, fiches et provenance. Guide HTML/Markdown en 12 étapes avec commandes à copier; vérificateur d’intégrité inclus. Aucune copie intermédiaire du gros TAR.
- Prévalidation : build courant, syntaxe Python, 13 blocs PowerShell, guide testé à cinq largeurs (320–1366), copie/navigation/impression, aucun débordement horizontal ou appel réseau. Rendus examinés. Vérificateur : bonne copie acceptée et fichier altéré détecté. Sources officielles d’installation relues; préparation réseau et répétition HQ restent explicites.
- Livré : `artifacts/HydroDataLab-HQ-20261019-eba8d19.zip`, source Git incluse `eba8d19`, 477 995 420 octets (~478 Mo), 226 fichiers et ~1,24 Go décompressés. SHA-256 `988f22b6ac55d3e31056f91d920314cb81af10da8aab62eb26c6d923bca28fd0`; fichier `.sha256` voisin. Aucun ancien bundle/ZIP de code ni complément Docker typographique.
- Validé après compression : toutes les empreintes relues dans le ZIP; clonage main et exercice propres, huit tags et git fsck; exercice encore incomplet. Serveur Python extrait sur port temporaire, neuf routes, notes, PDF, liens du guide et synchronisation contexte/coûts réussis; Compose config valide. La pile native de répétition et le serveur Slidev existants n’ont pas été arrêtés.
- Preuves : `docs/validation/transfer-hq.json`, reçu `artifacts/TRANSFERT-HQ.txt` et `.zip.validation.json`. Le gros TAR n’a pas été réextrait pour le contrôle; copies Git temporaires retirées après vérification. Cette entrée de clôture est postérieure au commit inclus dans le ZIP; elle ne change pas le code livré.
- Prochaine action : transférer le ZIP et son `.sha256`, extraire, ouvrir `HydroDataLab-HQ/LIRE-MOI-HQ.html`, préparer les dépendances avec réseau et répéter sur le portable HQ. Aucun test sur ce portable ni sur son projecteur n’est revendiqué.

## Notes du présentateur faciles à parcourir — 19 octobre

- Départ : main `43121d0`, dépôt propre. Demande : ventiler les notes pour retrouver facilement le fil pendant la présentation.
- Réalisé : 29 notes structurées avec horloge, fil rouge, blocs courts de parole/manipulation et transition. Précisions et secours repliables; décision de secours à 2:10 de la démo 3 toujours dans le fil visible. Guide : repères de lecture et agrandissement du panneau; renvois de numéros périmés corrigés.
- Validé : build, 23 plages notes/guide et 32 + 13 minutes; 29 routes présentateur avec listes/titres, détails repliés et ouverture au clavier, sans débordement horizontal, erreur JavaScript ni requête externe. Rendus examinés en 1366×768 et 1600×900; contrôles natifs de texte et redimensionnement du panneau préservés. Synchronisation contexte/coûts revalidée.
- Contenu audience strictement identique après retrait des commentaires; PDF conservé, empreinte inchangée. Preuves : docs/validation/presenter-notes.json et presentation-flow.json. La répétition humaine du rythme reste à faire.
- Livraison : enregistrer la révision et actualiser le kit existant; les reçus artifacts/LIVRAISON.txt et REVISION.txt identifient le commit et le bundle. Prochaine action : recharger la vue présentateur et ajuster son panneau de notes sur le portable.

## Skills et tools : deux diapositives visuelles — 19 octobre

- Départ : main `bb479a1`, dépôt propre. Demande : expliquer la sélection automatique des skills, leur chargement progressif et la différence avec les outils; séparer la slide chargée en deux avec capture et schémas.
- Réalisé : slide 11 = capture du menu et trois familles de capacités; nouvelle 12 = deux parcours skill/outil. Cas de déclenchement, formats et matrice des deux réglages dans la fiche. `validate-etl` reste volontaire, sans modification de sa configuration. Renvois, guides et contrôles renumérotés.
- Minutage vérifié : 11 en 19:00–20:15, 12 en 20:15–21:45; vanille en 21:45–22:30 et tokens en 22:30–24:00. Tous les créneaux suivants restent inchangés. Total 29 slides (23 + 6), 32 minutes d’explications et 13 de démos; 23 plages notes/guide concordantes.
- Validé : build, 29 routes, PDF complet et six liens internes; capture et schémas inspectés. Un débordement initial en 12 a été corrigé par les espacements, sans réduire les polices. Synchronisation contexte 10/coûts 16 repassée dans les deux vues. Conducteur : huit rubriques, 21 copies, liens, quatre largeurs et impression. Avertissement FloatingVue d’export inchangé, sans défaut constaté dans les rendus.
- Livraison : enregistrer la révision puis actualiser le kit existant; les reçus artifacts/LIVRAISON.txt et REVISION.txt indiquent le commit et le bundle actifs. Prochaine action : rafraîchir les deux vues et répéter les slides 11–12. Les démos sont maintenant en 7/18/20, SDD en 19. La répétition humaine reste à effectuer.

## Disponibilité et choix des outils — 19 octobre

- Départ : main `4d63407`, dépôt propre. Demande : clarifier le besoin couvert par Tools et comment l’agent choisit ses outils.
- Réalisé : slide 11 distingue outils disponibles, choix du modèle et contrôle de l’exécution. Notes et fiche : description et paramètres d’un outil, choix selon demande/contexte, permissions, retour du résultat et référence avec #. Terminal = outil appelé; pytest = programme exécuté dans cet exemple. Guide aligné sur 15 + 60 + 35 + 10 secondes, sans allonger les deux minutes.
- Validé : build, 28 routes sans débordement détecté, erreur de page ni ressource externe; interactions existantes réussies. PDF de 28 pages, liens et libellés contrôlés, page 11 inspectée; six destinations internes conservées. Notes/guide : mêmes 32 + 13 minutes. Avertissement FloatingVue déjà connu à l’export, sans défaut constaté sur le rendu. Sources S41–S42 relues le 19 octobre.
- Livraison : commit et bundle actifs indiqués dans artifacts/LIVRAISON.txt et REVISION.txt du kit; actualiser le kit existant sans nouvelle archive Docker. Prochaine action : rafraîchir les deux vues et répéter l’explication de Tools. La répétition humaine reste à faire.

## Tour du panneau Agent Customizations — 19 octobre

- Départ : main `2f4e858`, dépôt propre. Demande : conserver Slidev et expliquer chaque élément de la capture fournie, là où les concepts sont introduits.
- Réalisé : slide 11 avec les sept rubriques dans l’ordre du panneau, définitions, utilités, exemples et liens officiels; Overview et Customize Your Agent explicités. Explore Customizations, compteurs et composition dans les notes et la fiche. Nouvelle capture réelle en annexe 23, notes et guides alignés. Toujours 28 slides et créneau 19:00–21:00.
- Validé : build, 28 routes sans débordement détecté, erreurs de page ou ressource externe; interactions préservées. PDF de 28 pages, neuf libellés en page 11 et six destinations internes; rendus inspectés. Notes/guide : 32 + 13 minutes. Conducteur : huit rubriques, 21 copies, liens, quatre largeurs et impression. Avertissement FloatingVue de l’exporteur inchangé, sans défaut constaté sur les rendus. La répétition humaine du rythme reste à faire.
- Livraison : actualiser le kit existant et son inventaire, sans nouvelle archive Docker; conserver les captures sources pour les liens de la fiche. Les reçus artifacts/LIVRAISON.txt et REVISION.txt du kit indiquent le commit et le bundle livrés. En cas d’arrêt avant leur actualisation, relancer scripts/refresh-kit.ps1 depuis un dépôt propre.
- Prochaine action : rafraîchir les deux vues Slidev, lire la slide 11 et sa capture en annexe 23, puis reprendre la répétition. Les services de démonstration restent en place.

## Progression Copilot et synchronisation du support — 19 octobre

- Départ : main `9629f7c`, dépôt propre. Demande confirmée : appliquer les recommandations 2 à 5; sujet central = GitHub Copilot dans VS Code et bonnes pratiques, HydroDataLab servant d’exemple. Aucun aperçu ETL supplémentaire au début.
- Réalisé : ouverture/objectifs/conclusion centrés sur Copilot; instructions 8–9 suivies du contexte 10, mécanismes 11 et approche vanille 12. Choix des skills en 20; Canvas en annexe 25. Les sept concepts restent couverts avec un exemple commun instruction/skill/outil.
- Notes, conducteur HTML/Markdown, guide et prompt live synchronisés. Démo 2 : résultat exécuté, avant/après, un indicateur, code/test, détails et conteneur; l’échec/relance devient une variante de 3:15–5:00, sans cumuler Aspire et Docker. Démo 3 sur la copie préparée, secours sur main; aucune bascule historique en séance.
- Minutage vérifié : les 22 plages des notes correspondent au guide, sans trou; 32 min d’explications + 13 min de démos, questions 10 min, marge 5 min. Démo 1 en slide 7, démo 2 en 17, SDD en 18, démo 3 en 19. L’application, les branches d’exercice et les tags sont inchangés.
- Validé : build, 28 routes sans débordement détecté ni erreur de page, interactions et synchronisation des deux vues (contexte 10/coûts 15). PDF de 28 pages, liens officiels et quatre destinations internes; rendus des pages modifiées et planches inspectés. L’export signale un avertissement FloatingVue de l’outillage, sans effet constaté sur le PDF ou le site compilé. Conducteur : 8 rubriques, 21 copies, 4 largeurs, liens et impression; 19 blocs PowerShell et 2 scripts analysés sans erreur syntaxique. Preuves : docs/validation/presentation-flow.json et presentation-guide.json, presentation/exports/qa/.
- Révision de contenu validée : `0a4b722`. Les reçus `artifacts/LIVRAISON.txt` et `artifacts/kit-hq-20261019-101734/REVISION.txt` identifient le commit livré et le bundle actif. Si un arrêt interrompt la livraison et laisse un reçu plus ancien, relancer `./scripts/refresh-kit.ps1 -KitPath artifacts/kit-hq-20261019-101734` depuis un dépôt propre; l’archive Docker reste conservée.
- Prochaine action : rafraîchir les vues présentateur/audience et le conducteur, puis reprendre la répétition humaine sur le portable HQ. Démo 2 : choisir le parcours standard ou la variante échec/relance avant de chronométrer. Aucun nouveau kit complet ni archive Docker supplémentaire.

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
