# Jalon 07 — solution validée

Le pipeline et l'application fonctionnent avec l'indicateur de couverture implémenté. Les observations valides sont dédupliquées par créneau; le calcul respecte les jours de 23/25 heures à Toronto.

`uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py` valide les règles, dont 23/24 = 95,83 %. Les zéros valides restent comptés et les données originales sont conservées.

La branche `demo-live` part du tag `06-specification`, où cette fonction est volontairement absente. Voir `docs/CHECKPOINTS.md` pour l'exercice et la bascule de secours. Cette version finale constitue le résultat préparé, pas une trace de génération en direct par Copilot.
