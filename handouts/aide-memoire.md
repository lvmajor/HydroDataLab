# Aide-mémoire — des agents IA aux données fiables

Vérifié les 18–19 octobre 2026. Repères MCP actualisés pour VS Code 1.142. Un bon résultat est un comportement attendu accompagné de preuves reproductibles.

La [fiche des personnalisations VS Code](personnalisations-vscode.md) explique les formats, portées, déclenchements, la capture du panneau Instructions et Canvas. La [matrice des modèles](matrice-modeles.md) détaille les hypothèses du graphique de coûts.

## Votre contexte de développement

Développer et tester localement des ETL Python et des visualisations, puis préparer leur exécution dans un environnement conteneurisé. Utiliser la marge d’expérimentation pour comparer des approches et conserver des résultats reproductibles. Au quotidien : données autorisées pour l’outil utilisé, secrets protégés, accès limités, changements relus et testés. Les politiques HQ continuent de s’appliquer aux prototypes. [Référence Microsoft sur la sécurité](https://code.visualstudio.com/docs/agents/run/security).

## Modèle, effort et coût du résultat

- **Auto** choisit le modèle; **Thinking Effort** règle le raisonnement pour les modèles compatibles. Commencer par les réglages recommandés, puis augmenter l’effort si la difficulté le justifie. Davantage d’effort consomme plus de tokens. [Réglage dans VS Code](https://code.visualstudio.com/docs/agent-customization/language-models#configure-thinking-effort).
- Comparer le coût de **tous les appels, reprises comprises**, avec les tests réussis et le temps passé. Un premier essai moins cher ne garantit pas une tâche moins chère.
- [Artificial Analysis : code et coût par tâche](https://artificialanalysis.ai/agents/coding-agents) aide à comparer des configurations agent/modèle/réglages. Son coût API par tâche n’est pas la facture Copilot ni un coût par réussite garantie. Détails et exemple dans la [matrice des modèles](matrice-modeles.md#thinking-effort--effort-de-raisonnement).

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

## Skills et outils : quand interviennent-ils?

Une skill charge une méthode dans le contexte : nom/description repérés, puis `SKILL.md` sélectionné selon la tâche ou invoqué avec `/nom`, puis références consultées au besoin. Un outil est appelé avec des paramètres : le harnais contrôle l’exécution et le résultat revient au modèle. Charger une skill ne lance pas automatiquement ses scripts.

Notre `/validate-etl` est volontaire (`disable-model-invocation: true`). Sans ce réglage, le modèle peut choisir une skill pertinente. `user-invocable: false` masque la commande du menu `/`, sans interdire à lui seul le choix automatique. Voir les [conditions complètes et les références](personnalisations-vscode.md#quand-une-skill-est-elle-chargée).

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

## MCP : emplacements selon le harnais, repères VS Code 1.142

**Transition récente :** la version **1.142, publiée le 14 octobre 2026**, déploie progressivement **Copilot comme harnais par défaut**. Il était déjà disponible dans le sélecteur. Les notes de version ne présentent pas les chemins ci-dessous comme apparus en 1.142 : ils correspondent à la configuration documentée lors du passage à Copilot. [Notes de version](https://code.visualstudio.com/updates/v1_142#copilot-harness).

| Harnais actif | Configuration personnelle | Configuration du projet | Clé JSON principale |
|---|---|---|---|
| **Copilot** | `%USERPROFILE%\.copilot\mcp-config.json` par défaut sous Windows | `.mcp.json` à la racine | `mcpServers` |
| **Local** | `mcp.json` dans les données du **profil VS Code** | `.vscode/mcp.json` | `servers` |

Le personnel Copilot concerne plusieurs projets et reste distinct d’un profil VS Code. Si `COPILOT_HOME` est défini, le fichier se trouve dans ce dossier. Les emplacements Local restent disponibles pour compatibilité, marqués dépréciés dans l’assistant. Celui-ci propose la destination projet `.mcp.json` lorsque **`chat.mcp.workspaceRootConfig.enabled`** est activé; cette option est encore **expérimentale**. [Microsoft : destinations MCP](https://code.visualstudio.com/docs/agent-customization/mcp-servers#choose-a-configuration-destination).

**Migration :** sélectionner **Copilot → Chat: Open Customizations → Migrations**. Relire les conversions, notamment les variables et propriétés propres à VS Code, puis vérifier un appel d’outil. Changer uniquement le nom du fichier ne suffit pas. Les chemins distants/Dev Container appartiennent à l’environnement qui exécute l’agent. [Microsoft : migration MCP](https://code.visualstudio.com/docs/agent-customization/migrate-customizations#migrate-mcp-servers).

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

## Garder une configuration facile à entretenir

Partir des fonctions natives, des consignes du projet et des tests. Ajouter un skill pour une tâche répétée, puis vérifier son utilité. La sélection Python, Playwright, Aspire, .NET et les pistes de Matt Pocock est détaillée dans [Skills et veille](skills-et-veille.md). Matt Pocock reste en exploration dans cette formation. Les ressources pour suivre les nouveautés et les emplacements HQ sont en diapositive 24.

## Vérifier une transformation

- Zéro est valide; null signifie absent.
- Un doublon ne compte pas deux fois.
- Fréquence et journée locale définissent l’attendu, pas le nombre de lignes reçu.
- Instants UTC, période locale début inclusif/fin exclusive.
- Revoir unités, provenance et avertissements, puis le diff.
- Montrer une sortie de tests; ne pas se satisfaire d’une affirmation de l’agent.

Niveau en mètres ≠ réserve en GWh. MW ≠ MWh. Les capteurs et fréquences des séries doivent rester explicites.

Sources : [instructions VS Code](https://code.visualstudio.com/docs/agent-customization/custom-instructions), [migration Copilot](https://code.visualstudio.com/docs/agent-customization/migrate-customizations), [contexte](https://code.visualstudio.com/docs/agents/concepts/context), [Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection), [tarifs](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing). Registre complet : `docs/SOURCES.md`.
