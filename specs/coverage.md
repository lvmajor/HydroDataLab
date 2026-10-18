# Mini-spécification : couverture des observations

## Besoin
Comme analyste BI, je veux voir si chaque série contient toutes les mesures attendues sur la journée sélectionnée avant d'utiliser les résultats. Une valeur absente ne doit pas se transformer en zéro.

## Contrat
La fonction indépendante `calculate_coverage(timestamps, start, end, interval_minutes, timezone_name="America/Toronto")` renvoie `{observed, expected, percent}`. Elle reçoit uniquement les horodatages des valeurs numériques valides; zéro est une valeur valide. Le pipeline filtre les nulls avant cet appel.

- `start` et `end` sont des dates civiles; début inclus, fin exclue.
- Le nombre attendu correspond à la durée réelle entre les deux minuits locaux, divisée par la cadence. Toronto connaît des journées de 23 et 25 heures.
- Une observation compte si son instant est dans la période et exactement sur un créneau attendu.
- Deux décalages UTC de la même heure locale à l'automne représentent deux instants distincts. Un doublon du même instant compte une seule fois.
- `percent` vaut `100 × observed / expected`, arrondi à deux décimales, entre 0 et 100.
- Une fin antérieure ou égale au début, un intervalle non positif/non diviseur de 60 ou un horodatage sans décalage UTC provoquent `ValueError`.

## Critères d'acceptation
Le snapshot du 17 octobre 2026 contient 96 créneaux de demande et 24 créneaux de niveau : 100 %. Le scénario pédagogique qui retire un point d'eau donne 23/24 = 95,83 %. Le 8 mars 2026 comporte 23 créneaux horaires, le 1er novembre en comporte 25.

L'API et les exports gardent le même contrat. Le checkpoint 06 présente `coverage: null` avec le stub `etl/demo/coverage_stub.py`; le checkpoint 07 contient la fonction terminée. Les tests d'acceptation sont volontairement séparés de la collecte normale : `uv run --project etl pytest specs/test_coverage_acceptance.py`. Le moteur et les données brutes ne sont pas modifiés pendant l'exercice.
