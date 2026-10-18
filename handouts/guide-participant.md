# Guide participant — pratiquer sur une analyse Python

Formation du 20 octobre 2026; sources vérifiées le 18 octobre. À la fin, vous devriez pouvoir cadrer une petite tâche, fournir un contrat de données et vérifier le résultat de l’agent.

## Le projet en une minute

HydroDataLab lit un instantané public Hydro-Québec, le transforme en Python et produit CSV, Parquet et JSON. FastAPI expose le même moteur que la CLI. Une API ASP.NET Core 10 pilote les tâches avec Quartz et garde l’historique dans SQLite. Vue affiche séries, qualité et provenance. Aspire 13.6.1 aide à démarrer et observer les services; Compose sert au chemin conteneur préparé.

Les données de démonstration couvrent le 17 octobre 2026 : 96 observations de demande et 72 observations hydrométéorologiques. Le manifeste conserve URL, date de récupération et SHA-256. Les identifiants de station des températures sont distincts de celui du niveau Gouin. Le niveau est une hauteur en mètres, pas une réserve d’énergie.

## Exercice A — comprendre avant de modifier (10 minutes)

1. Ouvrir une session du dépôt, moteur Copilot. Vérifier dossier et branche.
2. Utiliser le prompt de découverte de `prompts.md`.
3. Retrouver dans le contrat l’unité, la fréquence et le fuseau d’une série.
4. Demander quel test distingue zéro d’une absence.
5. Vérifier une affirmation dans le fichier source ou la sortie de test.

Réussite : vous pouvez nommer le fichier de transformation et expliquer pourquoi « 24 lignes » ne prouve pas « une journée complète ». Variante sans code : comparer le contrat et le rapport, puis écrire une question que l’agent doit résoudre avant de calculer.

## Exercice B — modifier une fonction avec une spec (15 à 20 minutes)

Utiliser une copie de travail depuis `06-specification`. Le stub de couverture est volontaire. Préserver le travail personnel et suivre les scripts de préparation/reprise du dépôt. Ne pas réinitialiser destructivement votre dossier principal.

Fonction : `etl/hydrodatalab/coverage.py`. Contrat : `docs/CONTRACT.md`. Tests d’acceptation : `specs/test_coverage_acceptance.py`.

Critères à donner avant le code :

| Entrée / situation | Attente |
|---|---|
| Journée ordinaire horaire, 24 instants valides distincts | observed=24, expected=24, percent=100 |
| Une observation de niveau retirée | 23, 24, 95.83 |
| Valeur de mesure égale à 0 | Elle reste valide dans le pipeline |
| Même instant reçu deux fois | Il ne compte qu’une fois |
| Valeur null | Elle ne compte pas comme mesure observée |
| 8 mars 2026, pas de 15 minutes, America/Toronto | expected=92 |
| 1 novembre 2026, pas horaire, America/Toronto | expected=25 |
| 01:00 à -04:00 puis 01:00 à -05:00 le 1 novembre | Deux instants distincts |
| Timestamp hors période ou hors créneau | Ne gonfle pas observed |

Demander un plan bref, relire les décisions, puis implémenter. La fonction reçoit les timestamps des valeurs déjà valides; les tests pipeline couvrent aussi ce filtrage. Exécuter depuis la racine :

```powershell
uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live
```

Dans Vue, déplier **Qualité par série** pour observer le niveau à 23/24 = 95,83 %. La synthèse globale affiche 167/168 = 99,4 % : les dénominateurs diffèrent.

Réussite : les tests passent, vous comprenez le calcul d’expected et pouvez expliquer le diff. Un message « cela devrait fonctionner » ne satisfait pas ce critère.

## Exercice C — capitaliser sans grossir le contexte (10 minutes)

Écrire une règle concise dans un fichier d’instructions ciblé Python : expliquer le traitement de zéro/null avec sa raison métier. Vérifier `applyTo`, ouvrir une nouvelle session et demander une petite modification qui utilise cette règle. Examiner References et résultat. Si elle est ignorée, vérifier d’abord moteur et découverte.

Ensuite, proposer une skill de validation qui exécute les commandes du dépôt et résume les preuves. Pour rester volontaire, utiliser `disable-model-invocation: true`. Ne pas recopier l’ensemble du contrat dans chaque instruction.

## Mesurer votre pratique

Pour trois tâches comparables, noter le résultat des tests, le temps, le nombre de reprises et les AI credits visibles. Comparer Auto Balance à un modèle manuel autorisé. Changer une seule variable à la fois. La matrice jointe sert de point de départ, pas de classement universel.

## Continuer

- [Microsoft Learn — module agent mode, 50 minutes](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/) : prolonger par un exercice guidé.
- [VS Code — bonnes pratiques](https://code.visualstudio.com/docs/agents/best-practices) : contrat, revue et contexte.
- [Awesome Copilot](https://awesome-copilot.github.com/) : lire un exemple, l’adapter, le tester avant adoption.
- [GitHub Community](https://github.com/community/community/discussions/categories/copilot?locale=en-US) : partager une difficulté reproductible sans données internes.
- [GitHub Spec Kit](https://github.github.com/spec-kit/) : prochaine étape pour approfondir le développement piloté par spécifications.

Les six sujets de la formation sont reliés par la même règle : donner assez de contexte pour décider, puis des preuves pour vérifier.
