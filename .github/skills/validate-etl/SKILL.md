---
name: validate-etl
description: Vérifier une modification du moteur ETL HydroDataLab, ses contrats et ses résultats métier.
disable-model-invocation: true
---

1. Lire la spécification concernée et le dictionnaire des données.
2. Examiner le diff pour les unités, dates, valeurs absentes et doublons.
3. Exécuter `uv run --project etl pytest etl/tests`.
4. Si le calcul de couverture change, exécuter `uv run --project etl pytest specs/test_coverage_acceptance.py`.
5. Exécuter un traitement fixture puis vérifier le rapport et les exports.
6. Rapporter les preuves et les limites sans affirmer que Docker ou le réseau ont été testés si ce n'est pas le cas.
