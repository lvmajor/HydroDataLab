---
description: API .NET et orchestration des tâches
applyTo: "src/**/*.cs"
---

L'API `src/Api` ne réimplémente aucun calcul BI : elle pilote le moteur sélectionné. Les calculs restent dans `etl/` (Python) ou `src/Etl` (.NET), avec le même contrat et des tests de parité. Respecter `docs/CONTRACT.md`.
SQLite conserve l'historique et les paramètres; ne pas présenter une exécution interrompue comme réussie.
Une seule exécution de pipeline à la fois. La planification est désactivée par défaut.
Utiliser des paramètres SQL et des chemins validés. Propager l'identifiant d'exécution dans les logs.
