---
description: API .NET et orchestration des tâches
applyTo: "src/**/*.cs"
---

L'API ne réimplémente aucun calcul BI Python. Respecter `docs/CONTRACT.md`.
SQLite conserve l'historique et les paramètres; ne pas présenter une exécution interrompue comme réussie.
Une seule exécution de pipeline à la fois. La planification est désactivée par défaut.
Utiliser des paramètres SQL et des chemins validés. Propager l'identifiant d'exécution dans les logs.
