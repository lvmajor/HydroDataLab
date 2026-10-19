# Vérifications effectuées — 18 octobre 2026

## Dernière révision du support

Ajout de la mise en contexte en diapositive 2 : 25 pages au total, durée de 45 minutes conservée, liens internes renumérotés. Site et PDF reconstruits; 25 routes sans débordement ni erreur JavaScript, accès externes bloqués; PDF et nouvelle ouverture inspectés. Voir `validation/audience-context.json`. Les validations à 24 pages ci-dessous sont les preuves des révisions précédentes.

## Application

- Python : 35 tests, dont deux acceptations; 0 échec. Zéros, absences, unités, doublons, types de température, jours de 23/25 heures, réseau, publication atomique, rejouabilité et verrou HTTP couverts.
- API : 8 tests .NET; SQLite conserve les résultats et n'admet qu'un lancement actif concurrent.
- Vue : 11 tests; compilation TypeScript/Vite réussie; audit npm application sans alerte.
- Aspire 13.6.1 : trois ressources natives démarrées et saines, arrêt ciblé réussi.
- `scripts/prepare.ps1` et `scripts/start-demo.ps1 -NoBuild` exécutés avec succès.
- `scripts/verify-http.ps1` passé en natif et via nginx/Compose : 168 observations, 4 séries, 100 %, scénario niveau à 95,83 %, échec contrôlé, relance réussie, CSV/Parquet/JSON disponibles.
- Redémarrage réel de l'API : rapport et réglage périodique 60 min conservés; planification ensuite désactivée.
- Les tests navigateur couvrent le pilotage, le clavier, les graphiques, les scénarios, la relance, les exports et le mobile. Voir `docs/screenshots/`.

## Conteneurs et limite du poste

Les quatre images sont préparées : `hydrodatalab-etl:formation`, `hydrodatalab-api:formation`, `hydrodatalab-web:formation`, `mcr.microsoft.com/dotnet/aspire-dashboard:13.6.1`. Le modèle Compose est généré par Aspire, puis démarré avec `--pull never --no-build --wait`.

La CLI Python a été exécutée avec **`--network none`** : 168 observations; CSV identique octet pour octet, Parquet identique comme table typée, rapport métier identique hors instant de génération. Commande réexécutable : `uv run --project etl python etl/tests/verify_container.py --engine podman` (ou moteur Docker par défaut).

Sur ce poste, Docker Desktop ne démarre pas. Le moteur Podman 6.1.2 fonctionne, mais sa liaison Windows par named pipe s'est bloquée pendant un import d'image. La construction finale utilise `podman build --format docker`. Compose a été validé par une API **temporaire, liée à 127.0.0.1:55375**, lancée dans le namespace systemd de cette VM existante. Le namespace a été découvert, son PID n'est pas une configuration portable. Aucun service global, distribution ou application étrangère n'a été redémarré.

Ce contournement de diagnostic ne remplace pas la répétition du moteur autorisé sur le portable HQ. Les scripts acceptent `-Engine podman`, `-DockerContext` ou `-DockerHost`. Le parcours natif reste disponible sans moteur de conteneurs.

## Présentation

Révision pédagogique après retour du présentateur : définitions avant emploi, objectifs d'observation sur les pages de démonstration et manipulations déplacées vers les notes. Site et PDF de 24 pages régénérés et inspectés. Les liens profonds `/presenter/17` chargent maintenant les ressources et affichent réellement les notes; le minutage de secours reste absent de la vue audience. Preuve ciblée : `validation/pedagogy.json`.

24 routes Slidev et deux interactions validées avec les requêtes externes bloquées; 0 erreur JavaScript et 0 débordement. PDF de 24 pages, texte et rendu inspectés. Preuves détaillées : `presentation/exports/qa/VALIDATION.md`. Les alertes transitives restantes de l'outillage Slidev sont décrites sans masquage dans `presentation/DEPENDENCY-AUDIT.md`; la projection utilise le build statique par défaut.

## Sauvegarde et reprise Git

Les huit tags sont annotés et l'histoire est linéaire. Une restauration réelle depuis un bundle indépendant a exécuté les CLI des jalons 02 à 07 avec `uv --offline --frozen`. Les empreintes des sources sont exactes à chaque étape. Les deux acceptations échouent comme prévu sur 06; les 35 tests Python réussissent sur 07.

Cette vérification a détecté puis permis de corriger une normalisation des fins de ligne par Git dans les anciens commits. La réparation locale a été explicitement autorisée; aucune donnée métier ni empreinte source n'a été remplacée. L'ancienne sauvegarde reste conservée pour traçabilité. Preuves : `validation/git-restoration.json` et `validation/git-history-repair.json`.

L'archive hors ligne des quatre images mesure 1 205 382 656 octets. Son manifest contient les quatre identifiants attendus; son SHA-256 est `3c3c775373607b6f6cec5f2bdea7cc9506bfe4c5182e9df04bfce8ea4690ebfe`.

`scripts/package-kit.ps1` a été exécuté : copie vérifiée de l'archive d'images, bundle copié vérifié par Git, inventaire de chaque fichier. Le serveur Python standard livré dans le kit a passé un contrôle navigateur des 24 diapositives, des deux interactions et du mode présentateur `/presenter/1`, avec les requêtes externes bloquées. Aucune erreur JavaScript ni ressource externe; preuve `validation/kit-browser.json`.

## Synchronisation présentateur / audience — 19 octobre

Le build statique a été contrôlé dans deux pages d’un même contexte Chromium, avec les requêtes externes bloquées. Les contrôles de la vraie vue présentateur sont utilisés : cinq couches (sélection, texte et flèches), 20 coûts et légende, essais de 1 à 8, changements dans les deux sens, navigation native Slidev, rechargement et ouverture tardive de l’audience. Les deux réglages restent indépendants. Aucune erreur JavaScript ni ressource externe. Captures des deux vues inspectées; contrôle général des 25 diapositives réussi, sans débordement. Aucune version de dépendance existante n’a changé; VueUse 14.4.0 devient une dépendance explicite.

Test reproductible : `presentation/scripts/qa_presenter_sync.mjs`. Preuve : `validation/presenter-sync.json`; captures : `presentation/exports/qa/sync-*.png`. Le guide précise le mode Étendre, le même navigateur/profil/origine et la remise à l’état initial avant la séance. Le PDF reste le secours statique de 25 pages déjà validé.

## À confirmer le 19 octobre

Compte Copilot HQ, modèles réellement autorisés, politiques des outils, accès réseau public, moteur de conteneurs approuvé, projection, alimentation et répétition chronométrée. Aucune de ces vérifications spécifiques au portable HQ n'est déclarée effectuée ici.
