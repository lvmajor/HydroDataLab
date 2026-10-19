# Aide-mémoire — des agents IA aux données fiables

Vérifié le 18 octobre 2026. Un bon résultat est un comportement attendu accompagné de preuves reproductibles.

La [fiche des personnalisations VS Code](personnalisations-vscode.md) explique les formats, portées, déclenchements, la capture du panneau Instructions et Canvas. La [matrice des modèles](matrice-modeles.md) détaille les hypothèses du graphique de coûts.

## Votre contexte de développement

Développer et tester localement des ETL Python et des visualisations, puis préparer leur exécution dans un environnement conteneurisé. Utiliser la marge d’expérimentation pour comparer des approches et conserver des résultats reproductibles. Au quotidien : données autorisées pour l’outil utilisé, secrets protégés, accès limités, changements relus et testés. Les politiques HQ continuent de s’appliquer aux prototypes. [Référence Microsoft sur la sécurité](https://code.visualstudio.com/docs/agents/run/security).

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
| Harnais / harness | Logiciel qui relie modèle, outils, contexte et autorisations; Copilot utilise le Copilot SDK dans notre parcours. |
| Contexte / token | Le contexte est l'information disponible pour la tâche; un token est un morceau de texte traité par le modèle. |
| Instruction / skill | Une instruction est une consigne; une skill est une recette réutilisable avec ses étapes. |
| Tool / outil | Une action disponible, comme lire un fichier ou lancer pytest. |
| Serveur MCP | Un service qui expose notamment des outils par le Model Context Protocol. |
| Hook | Du code déclenché à un événement du cycle de l’agent, selon le harnais. |
| Plugin | Un paquet regroupant des capacités comme skills et serveurs MCP. |
| SDD | Spec-Driven Development : besoin → spécification → plan → tâches → code → preuves. |
| Canvas | Surface interactive partagée avec l’agent; préversion desktop Copilot, fournie par une extension. |
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

## Context layers — les couches du contexte

| Couche | Exemple dans HydroDataLab | Quand elle contribue |
|---|---|---|
| **Project instructions — Consignes du projet** | `.github/copilot-instructions.md` : préserver les données brutes et savoir valider le projet | Règles communes découvertes par le harnais |
| **File-specific instructions — Consignes ciblées** | `.github/instructions/python.instructions.md` : zéro est valide, une absence reste absente | Selon fichier, description ou ajout manuel |
| **Reference documents — Documents de référence** | `docs/DATA_DICTIONARY.md` : champs, unités, fréquence, fuseau et provenance | Document joint à la demande ou effectivement lu |
| **Task prompt — Demande courante** | « Ajoute la couverture décrite dans `specs/coverage.md`. » | Pour préciser le résultat attendu maintenant |
| **Tool results — Résultats des outils** | Sortie pytest et `report.json` : 23/24 donne 95,83 % | Pendant le travail, à partir des résultats réellement observés |

Toutes alimentent le **Model context — Contexte du modèle**. Ces noms organisent notre démarche; ils ne constituent pas cinq niveaux officiels de VS Code ni un ordre de priorité. Un fichier mentionné n’est pas automatiquement chargé. Les catégories remplacent les anciens raccourcis du support : socle, composant, contrat, tâche et preuves.

## Économiser sans sacrifier la qualité

Commencer par Auto Balance. Employer Efficiency pour une tâche simple, Intelligence ou un modèle capable pour une analyse difficile. Fournir les fichiers pertinents et les critères; éviter les sorties de terminal géantes. Mesurer le coût total des tentatives. `/compact` résume mais peut perdre des détails. Pour un sujet sans rapport, ouvrir une nouvelle session. Stocker les décisions durables dans Git, pas seulement dans le chat.

Les tarifs actuels sont en AI credits : 1 crédit = 0,01 $ US. Le coût dépend du modèle, des tokens et du cache. La remise Auto de 10 % des plans payants ne garantit pas une économie sur toute tâche. HydraFusion/Max choisissent un ou plusieurs modèles : somme des usages, sans supplément propre et sans remise Auto. Ils restent en research preview. [Documentation HydraFusion](https://docs.github.com/en/early-access/copilot/hydrafusion).

## Vérifier une transformation

- Zéro est valide; null signifie absent.
- Un doublon ne compte pas deux fois.
- Fréquence et journée locale définissent l’attendu, pas le nombre de lignes reçu.
- Instants UTC, période locale début inclusif/fin exclusive.
- Revoir unités, provenance et avertissements, puis le diff.
- Montrer une sortie de tests; ne pas se satisfaire d’une affirmation de l’agent.

Niveau en mètres ≠ réserve en GWh. MW ≠ MWh. Les capteurs et fréquences des séries doivent rester explicites.

Sources : [instructions VS Code](https://code.visualstudio.com/docs/agent-customization/custom-instructions), [migration Copilot](https://code.visualstudio.com/docs/agent-customization/migrate-customizations), [contexte](https://code.visualstudio.com/docs/agents/concepts/context), [Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection), [tarifs](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing). Registre complet : `docs/SOURCES.md`.
