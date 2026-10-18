# Aide-mémoire — des agents IA aux données fiables

Vérifié le 18 octobre 2026. Un bon résultat est un comportement attendu accompagné de preuves reproductibles.

## Avant de lancer

Définir objectif, entrée, sortie, contraintes et critères de réussite. Ouvrir le bon dossier du projet et choisir sa branche d'essai. Vérifier le modèle et les actions autorisées. Un worktree supplémentaire est facultatif. Pour HydroDataLab, Python réalise les transformations et les données brutes restent intactes.

## Les mots utiles

| Terme | Sens dans cette formation |
|---|---|
| Dépôt Git | Le projet avec l'historique de ses versions enregistrées. |
| Commit | Une version enregistrée dans cet historique. |
| Branche | Un chemin d'évolution nommé, par exemple `demo-live` pour notre essai. |
| Worktree | Un dossier de travail supplémentaire lié au même dépôt, utile pour ouvrir une autre branche à côté. |
| Session | Une conversation consacrée à une tâche. |
| Modèle / agent | Le modèle produit une proposition; l'agent l'utilise avec des outils pour avancer dans la tâche. |
| Moteur d'exécution | Le logiciel qui fournit à l'agent ses outils et ses consignes; Copilot dans notre démonstration. |
| Contexte / token | Le contexte est l'information disponible pour la tâche; un token est un morceau de texte traité par le modèle. |
| Instruction / skill | Une instruction est une consigne; une skill est une recette réutilisable avec ses étapes. |
| Spécification / test | La spécification décrit le résultat attendu; le test compare le résultat obtenu à cette attente. |
| Diff | La comparaison des lignes de code avant et après une modification. |
| ETL / fixture | ETL = extraire, transformer, charger ou exporter; fixture = copie préparée des données pour un résultat reproductible. |

Une branche ou un worktree ne sauvegarde pas automatiquement les changements non enregistrés. Un worktree partage l'historique du dépôt et n'isole pas les services externes. [Référence Git](https://git-scm.com/docs/git-worktree).

## Où ranger quoi?

| Besoin | Support |
|---|---|
| Décisions du projet | `.github/copilot-instructions.md` |
| Conventions d’un composant | `.github/instructions/*.instructions.md`, `applyTo` |
| Socle partagé entre agents compatibles | `AGENTS.md` |
| Procédure répétable | `.github/skills/<nom>/SKILL.md` |
| Rôle et outils | `.github/agents/<nom>.agent.md` |
| Tâche et critères actuels | Spec et tests ciblés |

Les règles applicables s’ajoutent; résoudre les contradictions à leur source. Copilot et Local ont des règles de découverte différentes. Les fichiers `*.prompt.md` restent utilisables dans Local; le moteur Copilot utilise les skills pour ces workflows. Pour une skill manuelle : `disable-model-invocation: true`.

## Les cinq paliers ETL

1. Socle : architecture, commandes et invariants non évidents.
2. Composant : conventions Python et transformations.
3. Contrat : schéma, unité, fréquence, fuseau, provenance, qualité.
4. Tâche : spec courte, fichier concerné, exemples attendus.
5. Preuves : tests, rapport et logs ciblés.

C’est une méthode d’organisation, pas une hiérarchie officielle de priorité. Un index du dépôt n’implique pas l’envoi de tous les fichiers à chaque requête.

## Économiser sans sacrifier la qualité

Commencer par Auto Balance. Employer Efficiency pour une tâche simple, Intelligence ou un modèle capable pour une analyse difficile. Fournir les fichiers pertinents et les critères; éviter les sorties de terminal géantes. Mesurer le coût total des tentatives. `/compact` résume mais peut perdre des détails. Pour un sujet sans rapport, ouvrir une nouvelle session. Stocker les décisions durables dans Git, pas seulement dans le chat.

Les tarifs actuels sont en AI credits : 1 crédit = 0,01 $ US. Le coût dépend du modèle, des tokens et du cache. La remise Auto de 10 % des plans payants ne garantit pas une économie sur toute tâche. HydraFusion/Max restent en research preview; leur orchestration n’est pas une économie garantie.

## Vérifier une transformation

- Zéro est valide; null signifie absent.
- Un doublon ne compte pas deux fois.
- Fréquence et journée locale définissent l’attendu, pas le nombre de lignes reçu.
- Instants UTC, période locale début inclusif/fin exclusive.
- Revoir unités, provenance et avertissements, puis le diff.
- Montrer une sortie de tests; ne pas se satisfaire d’une affirmation de l’agent.

Niveau en mètres ≠ réserve en GWh. MW ≠ MWh. Les capteurs et fréquences des séries doivent rester explicites.

Sources : [instructions VS Code](https://code.visualstudio.com/docs/agent-customization/custom-instructions), [migration Copilot](https://code.visualstudio.com/docs/agent-customization/migrate-customizations), [contexte](https://code.visualstudio.com/docs/agents/concepts/context), [Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection), [tarifs](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing). Registre complet : `docs/SOURCES.md`.
