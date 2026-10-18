# Vérifications effectuées — 18 octobre 2026

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

24 routes Slidev et deux interactions validées avec les requêtes externes bloquées; 0 erreur JavaScript et 0 débordement. PDF de 24 pages, texte et rendu inspectés. Preuves détaillées : `presentation/exports/qa/VALIDATION.md`. Les alertes transitives restantes de l'outillage Slidev sont décrites sans masquage dans `presentation/DEPENDENCY-AUDIT.md`; la projection utilise le build statique par défaut.

## À confirmer le 19 octobre

Compte Copilot HQ, modèles réellement autorisés, politiques des outils, accès réseau public, moteur de conteneurs approuvé, projection, alimentation et répétition chronométrée. Aucune de ces vérifications spécifiques au portable HQ n'est déclarée effectuée ici.
