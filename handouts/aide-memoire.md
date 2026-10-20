# Aide-mémoire — des agents IA aux données fiables

Vérifié les 18–19 octobre 2026. Repères MCP actualisés pour VS Code 1.142. Un bon résultat est un comportement attendu accompagné de preuves reproductibles.

La [fiche des personnalisations VS Code](personnalisations-vscode.md) explique les formats, portées, déclenchements, la capture du panneau Instructions et Canvas. La [matrice des modèles](matrice-modeles.md) détaille les hypothèses du graphique de coûts.

## Votre contexte de développement

Développer et tester localement des ETL Python et des visualisations, puis préparer leur exécution dans un environnement conteneurisé. Utiliser la marge d’expérimentation pour comparer des approches et conserver des résultats reproductibles. Au quotidien : données autorisées pour l’outil utilisé, secrets protégés, accès limités, changements relus et testés. Les politiques HQ continuent de s’appliquer aux prototypes. [Référence Microsoft sur la sécurité](https://code.visualstudio.com/docs/agents/run/security).

## Modèle, effort et coût du résultat

- **Auto** choisit le modèle; **Thinking Effort** règle le raisonnement pour les modèles compatibles. Commencer par les réglages recommandés, puis augmenter l’effort si la difficulté le justifie. Davantage d’effort consomme plus de tokens. [Réglage dans VS Code](https://code.visualstudio.com/docs/agent-customization/language-models#configure-thinking-effort).
- Comparer le coût de **tous les appels, reprises comprises**, avec les tests réussis et le temps passé. Un premier essai moins cher ne garantit pas une tâche moins chère.
- [Artificial Analysis : code et coût par tâche](https://artificialanalysis.ai/agents/coding-agents) aide à comparer des configurations agent/modèle/réglages. Son coût API par tâche n’est pas la facture Copilot ni un coût par réussite garantie. La slide 18 montre une vraie capture et les exemples Sol/Sonnet en Low, High et Max. Détails et valeurs datées dans la [matrice des modèles](matrice-modeles.md#thinking-effort--effort-de-raisonnement).

## Avant de lancer

Définir objectif, entrée, sortie, contraintes et critères de réussite. Ouvrir le bon dossier du projet et choisir sa branche d'essai. Vérifier le modèle et les actions autorisées. Un worktree supplémentaire est facultatif. Pour HydroDataLab, Python réalise les transformations et les données brutes restent intactes.

Pour formuler une première tâche : **« Je veux… à partir de… Ce sera réussi si… »** Exemple : produire une moyenne horaire à partir de quatre mesures aux 15 minutes, uniquement si les quatre sont valides. Prévoir un test complet et un test avec une mesure absente, puis relire le calcul et ses résultats. Ce gabarit sert à l’exercice de la slide 24.

## Les mots utiles

| Terme | Sens dans cette formation |
|---|---|
| Dépôt Git | Le projet avec l'historique de ses versions enregistrées. |
| Commit | Une version enregistrée dans cet historique. |
| Branche | Un chemin d'évolution nommé, par exemple `codex/repetition-01` pour notre essai. |
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

La **slide 13** détaille ces deux mécanismes. Une skill charge une méthode dans le contexte : nom/description repérés, puis `SKILL.md` sélectionné selon la tâche ou invoqué avec `/nom`, puis références consultées au besoin. Un outil est appelé avec des paramètres : le harnais contrôle l’exécution et le résultat revient au modèle. Charger une skill ne lance pas automatiquement ses scripts.

Notre `/validate-etl` est volontaire (`disable-model-invocation: true`). Sans ce réglage, le modèle peut choisir une skill pertinente. `user-invocable: false` masque la commande du menu `/`, sans interdire à lui seul le choix automatique. Voir les [conditions complètes et les références](personnalisations-vscode.md#quand-une-skill-est-elle-chargée).

## Où ranger quoi?

| Besoin | Support |
|---|---|
| Décisions du projet | `.github/copilot-instructions.md` |
| Conventions d’un composant | `.github/instructions/*.instructions.md`, `applyTo` |
| Règles partagées entre agents compatibles | `AGENTS.md` |
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

## Tiered-context architecture — architecture du contexte par paliers

Les slides 10–11 séparent deux questions. **Scope — portée : à quel travail cela s’applique? Progressive loading — chargement progressif : quand et comment le contenu entre-t-il dans le contexte?** Une portée ne définit ni un moment de chargement ni une priorité entre consignes.

### 1. Portée : projet, fichiers ou tâche

| Portée | Exemple HydroDataLab | Support |
|---|---|---|
| **Project-wide — tout le projet** | Préserver les données brutes et la provenance | `.github/copilot-instructions.md` ou `AGENTS.md` compatible |
| **File-specific — fichiers ciblés** | Ne pas remplacer une absence par zéro dans l’ETL Python | `.github/instructions/python.instructions.md`, motif `etl/**/*.py` |
| **Task scoped — tâche ciblée** | Ajouter la couverture, selon la spécification, puis la valider | Demande courante, spécification, rôle choisi et skill pertinente |

Les préférences utilisateur et les directives d’organisation applicables peuvent couvrir plusieurs projets. Ces sources sont complémentaires; résoudre les contradictions explicitement. Une skill ou une référence peut être réutilisée pour plusieurs tâches.

### 2. Chargement : automatique, conditionnel, puis au fil du travail

- **Au départ :** les instructions communes reconnues par le harnais accompagnent la demande. « Always on » désigne ces consignes permanentes applicables, pas tous les documents du dépôt ni une garantie d’obéissance.
- **Quand c’est pertinent :** une instruction ciblée peut s’activer par correspondance `applyTo`, par pertinence de sa `description` ou par ajout explicite. Une skill peut être sélectionnée par l’agent ou invoquée par l’utilisateur, selon sa configuration.
- **Pendant le travail :** joindre ou faire lire le dictionnaire, la spécification et le code utiles; recevoir les résultats des outils. Une lecture peut aussi avoir lieu dès le début. Ce sont des mécanismes qui se combinent, pas trois cases exclusives.

**Une skill est à la fois Task scoped (portée) et On demand (chargement).** « Au besoin » n’implique pas « manuel seulement ». Le fonctionnement détaillé des skills et des appels d’outils est présenté **uniquement en slide 13**, et résumé dans la [section Skills et outils](#skills-et-outils--quand-interviennent-ils). La slide 11 illustre le contexte avec les règles communes, la règle Python applicable et le dictionnaire lu; la 12 situe les rubriques de configuration dans l’interface.

**Architecture et standards :** mettre les quelques exigences essentielles dans les règles communes, puis référencer les documents détaillés et préciser quand les lire. Un `ARCHITECTURE.md` ou un lien ne garantit pas l’inclusion intégrale du document. Une instruction ciblée par chemin est automatique **sous condition**, pas permanente pour toutes les tâches. [Microsoft : instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions), [organiser le contexte](https://code.visualstudio.com/docs/agents/guides/context-engineering-guide).

**IDE/CLI :** les fichiers ouverts ne sont pas tous envoyés. Le contexte implicite dépend de la vue, du harnais et des réglages; dans la fenêtre Agents, il est désactivé par défaut. Joindre explicitement le code ou les fichiers indispensables. En CLI, ne pas supposer que les onglets de VS Code sont disponibles. Une sortie d’outil revient au contexte; une sortie d’un autre terminal doit être fournie ou lue. [Microsoft : contexte implicite](https://code.visualstudio.com/docs/agents/concepts/context#implicit-context).

**Rôle du harnais :** MCP configure les connexions et outils disponibles; les permissions autorisent ou bloquent les actions. Ces paramètres peuvent être persistants au niveau utilisateur/projet. Ils ne sont ni une consigne métier ni un chargement de toutes les données du serveur. Les définitions d’outils et leurs résultats peuvent contribuer au contexte. [Microsoft : utiliser les outils](https://code.visualstudio.com/docs/agents/run/tools).

**Prompt et workflow :** le prompt est la demande courante. Les fichiers `.prompt.md` s’exécutent dans **Local / Chat**; avec le harnais **Copilot**, employer une skill compatible pour un workflow réutilisable. Un agent personnalisé fournit son rôle quand il est sélectionné. [Microsoft : prompt files](https://code.visualstudio.com/docs/agent-customization/prompt-files).

**Composition HydroDataLab (slide 11) :** règles communes + règle Python applicable + demande de couverture + dictionnaire/code lus. Un résultat d’outil peut ensuite enrichir le contexte; la slide 13 explique cet appel et son retour. S’y ajoutent les instructions système et l’historique utile de cette session, dans une fenêtre de contexte limitée. Vérifier References, activité de lecture et résultats; « disponible » ne signifie pas « utilisé ». [Microsoft : assemblage du contexte](https://code.visualstudio.com/docs/agents/concepts/context#how-vs-code-assembles-context).

## Économiser sans sacrifier la qualité

Commencer par Auto Balance. Employer Efficiency pour une tâche simple, Intelligence ou un modèle capable pour une analyse difficile. Fournir les fichiers pertinents et les critères; éviter les sorties de terminal géantes. Mesurer le coût total des tentatives. `/compact` résume mais peut perdre des détails. Pour un sujet sans rapport, ouvrir une nouvelle session. Stocker les décisions durables dans Git, pas seulement dans le chat.

Les tarifs actuels sont en AI credits : 1 crédit = 0,01 $ US. Le coût dépend du modèle, des tokens et du cache. La remise Auto de 10 % des plans payants ne garantit pas une économie sur toute tâche. HydraFusion/Max choisissent un ou plusieurs modèles : somme des usages, sans supplément propre et sans remise Auto. Ils restent en research preview. [Documentation HydraFusion](https://docs.github.com/en/early-access/copilot/hydrafusion).

## Garder une configuration facile à entretenir

Partir des fonctions natives, des consignes du projet et des tests. Ajouter un skill pour une tâche répétée, puis vérifier son utilité. La sélection Python, Playwright, Aspire, .NET et les pistes de Matt Pocock est détaillée dans [Skills et veille](skills-et-veille.md). Matt Pocock reste en exploration dans cette formation. Les ressources pour suivre les nouveautés et les emplacements HQ sont en diapositive 25.

## Vérifier une transformation

- Zéro est valide; null signifie absent.
- Un doublon ne compte pas deux fois.
- Fréquence et journée locale définissent l’attendu, pas le nombre de lignes reçu.
- Instants UTC, période locale début inclusif/fin exclusive.
- Revoir unités, provenance et avertissements, puis le diff.
- Montrer une sortie de tests; ne pas se satisfaire d’une affirmation de l’agent.

Niveau en mètres ≠ réserve en GWh. MW ≠ MWh. Les capteurs et fréquences des séries doivent rester explicites.

Sources : [instructions VS Code](https://code.visualstudio.com/docs/agent-customization/custom-instructions), [migration Copilot](https://code.visualstudio.com/docs/agent-customization/migrate-customizations), [contexte](https://code.visualstudio.com/docs/agents/concepts/context), [Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection), [tarifs](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing). Registre complet : `docs/SOURCES.md`.
