# Jalon 07 — solution validée

Le pipeline et l'application fonctionnent avec l'indicateur de couverture implémenté. Les observations valides sont dédupliquées par créneau; le calcul respecte les jours de 23/25 heures à Toronto.

`uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py` valide les règles, dont 23/24 = 95,83 %. Les zéros valides restent comptés et les données originales sont conservées.

Le tag historique `06-specification` laisse cette fonction volontairement absente. Pour la répétition actuelle, créer `codex/repetition-01` depuis `codex/preparation-demo-06` (9e1ecde), qui conserve l’exercice et ajoute les améliorations récentes. Suivre `handouts/conducteur-demos.html` pour les commandes et le retour au dépôt principal `main` en secours. Cette version finale constitue le résultat préparé, pas une trace de génération en direct par Copilot.

Sur main, les analyses ETL, la synthèse horaire et les détails d’exécution ont été ajoutés après les tags historiques. Pour la nouvelle répétition, suivre le conducteur : `codex/preparation-demo-06` intègre ces améliorations tout en laissant la couverture à implémenter. Ne pas basculer simplement sur un tag ancien en espérant y retrouver la dernière interface.
