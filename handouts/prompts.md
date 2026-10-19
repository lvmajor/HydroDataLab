# Prompts de la formation

Version du 19 octobre 2026. Coller dans une session du dépôt et du moteur attendus. Remplacer les références si la structure évolue. Ces prompts sont du texte à copier; ils ne supposent pas le chargement de fichiers `*.prompt.md` par le moteur Copilot.

## 1 — Retrouver le contexte (lecture seule)

```text
Repère le calcul des indicateurs et explique comment il est testé.
Ne modifie aucun fichier. Suis le chemin des données locales préparées
dans data/fixtures jusqu’au rapport. Lis docs/CONTRACT.md et les instructions pertinentes.
Identifie le fichier Python de transformation et les tests à utiliser.
Cite les fichiers consultés et signale toute information manquante.
Réponse attendue : un schéma en quatre étapes et les commandes de vérification.
```

Observer References et les lectures d’outils. Une citation seule ne prouve pas que la règle a été respectée; vérifier une affirmation dans le code.

## 2 — Préparer une modification

Dans la fenêtre Agents avec Copilot, utiliser `/plan` pour demander une démarche. Le sélecteur de cette fenêtre ne contient pas actuellement l'agent Plan.

```text
Prépare un plan pour ajouter la couverture des observations à une série du rapport.
Lis docs/CONTRACT.md, etl/hydrodatalab/coverage.py et
specs/test_coverage_acceptance.py. Ne modifie pas le code.
Clarifie observed, expected, percent, les bornes de la journée locale,
les valeurs absentes, les doublons et le changement d’heure.
Propose la plus petite modification et les tests qui prouvent le résultat.
```

## 3 — Mini-démo live Python (copie préparée issue de 06)

```text
Implémente calculate_coverage dans etl/hydrodatalab/coverage.py.
Respecte specs/coverage.md, la signature existante et docs/CONTRACT.md.
Les timestamps reçus correspondent déjà aux observations dont la valeur est valide.
Compte des créneaux distincts alignés et inclus dans [début, fin).
Début et fin sont des dates civiles America/Toronto; fin exclusive.
Calcule expected à partir des bornes converties en UTC et de interval_minutes,
pas à partir des lignes reçues. Gère les journées de 23 et 25 heures.
Retourne observed, expected, percent arrondi à deux décimales.
Ne recrée pas l’API ni l’interface. Ne modifie pas les données brutes.
Exécute depuis la racine de cette copie :
uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live
Lis les échecs et corrige la fonction. Termine par le diff et les résultats
vérifiés; n’affirme pas avoir lancé un test sans résultat d’exécution.
```

## 4 — Relire après génération

```text
Relis la modification de couverture et les tests, sans changer les fichiers.
Cherche une erreur sur les bornes temporelles, la déduplication, zéro/null
et les journées de changement d’heure. Distingue un défaut démontré d’une
question ou d’une amélioration facultative. Pour chaque défaut, donne un cas
reproductible et le résultat attendu. Si tu n’en trouves pas, dis-le sans
présenter cette relecture comme une garantie d’absence de bug.
```

## 5 — Créer une instruction à partir d’une erreur récurrente

```text
Nous avons plusieurs fois confondu une valeur nulle avec zéro dans les ETL.
Propose une instruction courte et ciblée pour les fichiers etl/**/*.py,
avec la raison métier et un exemple. Vérifie les instructions existantes
pour éviter les doublons ou contradictions. Ne généralise pas aux autres
langages si cette règle n’y est pas nécessaire.
```

## 6 — Reprendre après interruption

```text
Lis PROGRESS.md, docs/CONTRACT.md et l’état Git. Identifie le dernier jalon
vérifié et la prochaine action encore nécessaire. Préserve les changements
existants. Distingue ce qui est écrit de ce qui a réellement été testé.
Résume le point de reprise avant de continuer la tâche autorisée.
```

La démo live possède un seuil de bascule à 2:10. Si elle dépasse ce seuil, utiliser la solution préparée en l’annonçant, puis montrer le diff et les tests. Le but est d’enseigner la vérification, pas de masquer le temps de génération.
