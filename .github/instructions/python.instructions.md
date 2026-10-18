---
description: Règles des transformations et contrats de données Python HydroDataLab
applyTo: "etl/**/*.py,specs/**/*.py"
---

Séparer entrées/sorties et calculs purs. Garder la CLI et FastAPI comme adaptateurs du même cœur.
Consulter `docs/DATA_DICTIONARY.md` pour unités, fréquence, type de mesure et fuseau.
Éviter les accès réseau dans les tests de calcul. Utiliser les fixtures attribuées ou des données synthétiques explicitement identifiées.
Dédupliquer par série et instant, pas par valeur. Valider les nombres finis.
Calculer la couverture à partir des créneaux attendus de la période; une journée locale peut durer 23, 24 ou 25 heures.
Écrire les exports dans un répertoire temporaire puis publier atomiquement le résultat terminé.
