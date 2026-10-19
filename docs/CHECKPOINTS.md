# Parcours Git et démonstration de quatre minutes

Les huit tags annotés constituent une histoire linéaire. Les premiers jalons sont volontairement plus petits : seuls leurs composants présents sont exécutables. Les supports et scripts complets sont disponibles à partir du jalon 05. Conserver une copie du PDF et de ce guide hors du dossier que l'on change de jalon.

La révision pédagogique du 18 octobre se trouve sur `main` et dans le kit de transfert le plus récent. Les tags conservent leur état historique : projeter le diaporama du kit récent pendant que le dossier de démonstration change de jalon.

| Tag | État et commande depuis la racine |
|---|---|
| `00-socle` | README, journal et diagnostic `scripts/doctor.ps1`; pas encore d'ETL |
| `01-instructions` | Socle, règles ciblées Python/API/Vue, skill de validation et rôle de revue |
| `02-extraction` | `uv run --project etl python -m hydrodatalab.extract --source fixture --output outputs/raw`; 168 observations |
| `03-transformation-bi` | `uv run --project etl python -m hydrodatalab run --source fixture --output outputs/cli`; tests dans `etl/tests` |
| `04-pilotage` | API, SQLite, Quartz, Vue et Aspire : `scripts/prepare.ps1 -SkipPresentation`, puis `scripts/start-demo.ps1` |
| `05-docker` | Conteneurs, supports, scripts de préparation et validation |
| `06-specification` | Application fonctionnelle, couverture `null`; tests de couverture volontairement rouges; demande adressée à Copilot |
| `07-solution` | Solution de couverture testée et visible dans Vue; sauvegarde complète |

`git log --oneline --decorate --all` montre la progression. Le jalon 06 retire **volontairement** une capacité déjà construite afin d'isoler l'exercice Python; le reste du pipeline est inchangé. Cette préparation n'est pas présentée comme une génération en direct.

## Préparer le live

**Parcours recommandé pour la séance :** suivre le [conducteur des trois démos](../handouts/conducteur-demos.html). Il conserve le dépôt principal sur `main`, crée une copie issue de 06 et y applique le commit réseau `036bb193c778be0ff5a8f1c9681ee2b40f666397`. Ce commit laisse la couverture à implémenter et fait attribuer les ports API/Python par Aspire, avec Vue sur 53170. Les huit tags et la branche historique `demo-live` ne sont pas réécrits. Le parcours direct ci-dessous permet encore de consulter leur état historique, avec les anciens ports fixes.

Arrêter le parcours conteneurisé avant de changer de jalon. Après l'arrêt ciblé d'Aspire, le script refuse aussi la bascule si le port 53170 est encore occupé; il ne tue aucune autre application.

```powershell
./scripts/checkpoint.ps1 -Tag 06-specification -Branch demo-live
./scripts/start-demo.ps1
```

La branche `demo-live` est préparée sur 06. Le script reprend une branche existante sans la réinitialiser; sinon il la crée. Il refuse un dossier contenant des modifications non enregistrées. Le script de démarrage sélectionne `.runtime/<branche-ou-tag>` : le rapport déjà calculé sur main ne masque pas le résultat manquant sur 06. Une planification activée dans un autre jalon ne se propage pas. Pour une nouvelle répétition, utiliser un nouveau nom de branche, par exemple `demo-repetition-19`.

1. Ouvrir `specs/coverage.md`, `specs/test_coverage_acceptance.py` et `etl/hydrodatalab/coverage.py`.
2. Afficher une exécution fixture : aucune valeur de couverture n'est inventée.
3. Coller la requête de `handouts/prompts.md`. Le seul changement nécessaire est Python.
4. Vérifier `uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live`.
5. Relancer un traitement depuis Vue. Le fichier Python est rechargé par Uvicorn; vérifier aussi son journal Aspire. Le scénario manquant donne 23/24, soit 95,83 %.

## Basculer à la solution sans perdre le travail

À 2 min 10, si la génération n'est pas prête, arrêter l'application avec `scripts/stop-demo.ps1`. Conserver les modifications de la branche live (commit explicite, ou stash nommé incluant les nouveaux fichiers). Ne pas utiliser `reset --hard`.

Préparer de préférence **une deuxième copie locale de secours** à l'avance : `git clone <chemin-du-depot> <nouveau-dossier>` puis `git switch --detach 07-solution` dans cette copie, `scripts/prepare.ps1` et une répétition. La copie possède son propre `.runtime` et ses dépendances préparées. Ne lancer qu'une application à la fois, car les ports sont les mêmes. Les captures `docs/screenshots/` et le PDF sont le dernier recours.

Pour revenir à la version finale dans un dépôt propre : `scripts/checkpoint.ps1 -Tag 07-solution`, puis `scripts/start-demo.ps1`. Pour revenir à la branche principale : arrêter Aspire, `git switch main`, puis démarrer. Les scripts n'effacent jamais les résultats ou les historiques.

## Après un plantage

`scripts/resume.ps1` affiche le journal, la branche et les derniers commits sans mutation. `scripts/resume.ps1 -Start` relance ensuite l'application. SQLite garde l'historique; toute ancienne exécution active est marquée interrompue au démarrage. Relancer explicitement dans Vue. Les exports incomplets ne deviennent pas le rapport courant.

`scripts/backup.ps1` exige un dépôt propre et produit un bundle Git vérifié ainsi qu'une archive ZIP du code. Copier ces fichiers sur un autre support. Les images, dépendances et historiques locaux doivent être transportés séparément si on veut reprendre sans réseau.
