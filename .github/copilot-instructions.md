# Carte du projet

HydroDataLab illustre un ETL Python pour des développeurs citoyens BI.
`etl/` extrait, valide, transforme et exporte. `src/Api/` pilote les tâches; `web/` présente les résultats.
`src/AppHost/` décrit les composants Aspire. `presentation/` contient la formation locale.
Lire `docs/DATA_DICTIONARY.md` avant de modifier un calcul et `docs/CONTRACT.md` avant de modifier une interface.
Conserver les données brutes et la provenance; les transformations sont rejouables et déterministes.
Une valeur absente ne devient jamais zéro. Un zéro numérique peut être une observation valide.
Un niveau d'eau en mètres n'est ni une profondeur, ni un pourcentage de remplissage, ni un stock énergétique.
Conserver le type des températures : minimum et maximum horaires ne sont pas des moyennes.
Les horodatages sont des instants UTC; les périodes métier indiquent leur fuseau et leur fin exclusive.
Tester le résultat métier avec de petits exemples, y compris données manquantes et changements d'heure.
Avant de conclure, examiner le diff et exécuter la validation du composant touché.
Pour toute interface créée ou modifiée, lire [les critères de typographie et de lisibilité](../docs/UI_TYPOGRAPHY.md) et fournir les résultats des vérifications visuelles; une compilation réussie ne suffit pas. Pour Vue, lire aussi [les instructions ciblées](instructions/vue.instructions.md).
Python : `uv run --project etl pytest etl/tests`. API : `dotnet test tests/Api.Tests`.
Vue : `npm --prefix web run build`. Formation : `npm --prefix presentation run build`.
Ne pas déployer vers un cloud ou publier le dépôt dans le cadre de cette formation locale.
