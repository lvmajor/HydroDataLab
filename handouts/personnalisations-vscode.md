# Personnalisations de VS Code et du harnais Copilot

Vérifié le **19 octobre 2026**, pour VS Code **1.142**. Cette fiche accompagne les diapositives 8 à 13, 21, 24 et 26. Les formats cités sont liés à leur documentation officielle.

## Choix conseillé pour ce public

Pour des analystes Python qui travaillent aujourd’hui dans VS Code + GitHub Copilot, commencer avec **un socle court dans `.github/copilot-instructions.md`**, des règles ciblées dans `.github/instructions/python.instructions.md`, et des tests. Ajouter une skill quand une procédure revient souvent. Le harnais est le logiciel qui relie le modèle aux outils et au contexte. Dans 1.142, Copilot utilise le Copilot SDK et son adoption comme défaut progresse; il était déjà disponible. Changer le modèle n’en change pas le harnais. [Harnais et prérequis](https://code.visualstudio.com/docs/agents/run/agent-harnesses), [nouveautés 1.142](https://code.visualstudio.com/updates/v1_142#copilot-harness).

**AGENTS.md est un bon choix pour le socle d’une équipe multiharnais**, lorsque tous les outils choisis le découvrent. Le nom exact est `AGENTS.md`, à la racine; le corps est du Markdown. Conserver les détails propres à Copilot dans ses fichiers ciblés. Dans HydroDataLab, le socle existant reste dans `copilot-instructions.md`; le petit `AGENTS.md` demande sa lecture. Ce renvoi doit être vérifié, pas supposé équivalent à une inclusion automatique. Ne pas maintenir deux copies longues des mêmes règles. [Formats pris en charge](https://code.visualstudio.com/docs/agent-customization/custom-instructions#choose-a-format).

## Portée, activation, composition : trois questions différentes

| Question | Réponse pratique |
|---|---|
| **Où et pour qui?** | Workspace = ce projet, versionné dans Git. User = préférences personnelles sur la machine du harnais. Organisation = règles gérées centralement dans les sessions compatibles. |
| **Quand?** | Socle commun; motif des fichiers; pertinence de la tâche; ou ajout manuel. |
| **Comment combiner?** | Les éléments applicables contribuent au contexte. Supprimer les contradictions; ne pas compter sur « le plus proche gagne ». |

Il ne faut pas présenter la hiérarchie de dossiers comme une hiérarchie universelle d’autorité. GitHub documente un ordre de priorité pour ses expériences, notamment GitHub.com; il ne suffit pas à prouver celui de chaque harnais VS Code. Les instructions ne remplacent pas les autorisations appliquées par le logiciel. [Portées dans VS Code](https://code.visualstudio.com/docs/agent-customization/overview#choose-a-customization-scope), [contexte GitHub.com et priorités](https://docs.github.com/en/copilot/concepts/prompting/response-customization).

| Fichier / source pour Copilot | Format | Quand l’utiliser |
|---|---|---|
| `.github/copilot-instructions.md` | Markdown | Carte du projet et règles communes. |
| `AGENTS.md` à la racine | Markdown | Socle partagé entre harnais compatibles. |
| `.github/instructions/*.instructions.md` | En-tête YAML facultatif + corps Markdown | Détails d’un composant ou d’une tâche. |
| `~/.copilot/copilot-instructions.md` | Markdown | Préférences personnelles permanentes. |
| `~/.copilot/instructions/*.instructions.md` | YAML + Markdown | Préférences ciblées personnelles. |
| Instructions d’organisation | Configuration GitHub | Selon accès, prise en charge et réglage `github.copilot.chat.organizationInstructions.enabled`. |

`~` signifie le dossier personnel sur la machine où l’agent s’exécute. Les fichiers de profil VS Code et les dossiers `~/.copilot` sont distincts; les dossiers du harnais ne suivent pas automatiquement Settings Sync. [Migration officielle](https://code.visualstudio.com/docs/agent-customization/migrate-customizations).

### Exemple ciblé

Dans `.github/instructions/python.instructions.md` :

```md
---
description: Qualité des transformations Python
applyTo: "etl/**/*.py"
---
Conserver les valeurs manquantes.
Compter 0 comme une mesure numérique valide.
```

- `applyTo` : motif relatif à la racine, correspondant aux fichiers créés ou modifiés. `**` traverse les sous-dossiers.
- `description` : aide à sélectionner la règle pour une tâche pertinente.
- Sans ces champs : joindre le fichier manuellement.
- `name` facultatif : libellé dans l’interface.

Ce bloc est simplifié : le fichier réel couvre aussi `specs/**/*.py` et d’autres règles Python. [Format Microsoft](https://code.visualstudio.com/docs/agent-customization/custom-instructions#instructions-file-format).

Exemple chiffré : trois températures attendues `[10, 0, None]` donnent **deux mesures numériques sur trois**, soit **66,67 %**. Remplacer `None` par zéro ferait croire à trois mesures. Dans pandas, une absence peut être `NaN` ou `NA`; dans JSON, `null`. Les règles de plausibilité d’une série restent distinctes de cette distinction entre absence et zéro.

## Context layers — les couches du contexte

La diapositive 10 relie cinq sources au **Model context — Contexte du modèle** :

1. **Project instructions — Consignes du projet** : les règles communes de `copilot-instructions.md`.
2. **File-specific instructions — Consignes ciblées** : les règles Python dans `python.instructions.md`, activées selon les mécanismes décrits plus haut.
3. **Reference documents — Documents de référence** : le dictionnaire des données joint à la demande ou lu par l’agent.
4. **Task prompt — Demande courante** : le résultat voulu et la spécification à suivre pour cette tâche.
5. **Tool results — Résultats des outils** : les sorties réelles de pytest, du terminal ou du rapport de qualité.

Les deux premières sources apportent les consignes durables; les suivantes cadrent le travail et permettent de vérifier son résultat. C’est notre organisation pédagogique, pas une liste de niveaux officiels de VS Code. La position dans le dessin ne donne pas une priorité. L’historique et d’autres instructions peuvent également contribuer au contexte. [Guide Microsoft pour structurer le contexte](https://code.visualstudio.com/docs/agents/guides/context-engineering-guide).

## Lire le panneau Agent Customizations

La diapositive 11 relie les libellés du menu à trois familles pédagogiques, à partir de la [capture complète fournie le 19 octobre](../presentation/public/vscode-customizations-overview.png), cadrée en annexe 25. Ce menu classe des capacités; sa disposition ne définit pas leur priorité.

- **Overview — Vue d’ensemble :** point d’entrée pour retrouver les personnalisations du harnais et du projet indiqués dans l’en-tête.
- **Customize Your Agent — Personnaliser votre agent :** décrire ses préférences pour faire préparer des agents, skills ou instructions. Par exemple : « Prépare une instruction de projet demandant des tests pour toute transformation Python. » Examiner les fichiers proposés, leur portée et leur compatibilité avant de les enregistrer. Ce champ aide à créer une personnalisation; il ne garantit pas son application à toutes les sessions.
- **Explore Customizations — Explorer les personnalisations :** les cartes ouvrent les mêmes catégories que le menu de gauche. Elles ne constituent pas d’autres mécanismes.

[Microsoft : panneau et création assistée](https://code.visualstudio.com/docs/agent-customization/overview).

| Rubrique affichée | Ce que c’est et à quoi elle sert | Exemple à raconter |
|---|---|---|
| **Plugins — Extensions de l’agent** | Installer un paquet regroupant plusieurs capacités et faciliter leur distribution. Un plugin d’agent n’est pas synonyme d’extension VS Code. | Distribuer une skill de validation et la connexion MCP utile à l’équipe. |
| **MCP Servers — Serveurs MCP** | Connecter des outils et des données externes par le Model Context Protocol. | Exposer un outil de lecture SQL; l’agent peut demander cet outil. |
| **Skills — Procédures réutilisables** | Fournir une méthode, éventuellement accompagnée de scripts et ressources, chargée quand elle est pertinente ou invoquée. | `/validate-etl` guide les tests, puis le bilan qualité. |
| **Instructions — Consignes** | Donner des règles durables, selon leur portée et leurs conditions d’activation. | Conserver une valeur absente au lieu de la remplacer par zéro. |
| **Agents — Agents personnalisés** | Préparer un rôle avec ses consignes et ses outils. | Le réviseur BI examine les calculs avec des outils de lecture. |
| **Hooks — Déclencheurs** | Exécuter une commande à un événement du cycle de vie, sans attendre que le modèle la choisisse. | Enregistrer un appel d’outil dans un journal local. |
| **Tools — Outils** | Savoir quels outils sont disponibles pour la tâche et gérer les outils configurables : lecture, recherche, terminal, etc. | L’agent dispose du terminal; il peut l’appeler pour lancer pytest. |

**Même tâche, trois fonctions :** l’instruction demande de préserver les absences; le skill décrit la vérification; l’outil exécute le test. Un serveur MCP peut fournir un outil; un plugin peut distribuer ce serveur et la skill. Ni le skill, ni le plugin ne sont des modèles. [Microsoft : comparer et combiner les mécanismes](https://code.visualstudio.com/docs/agents/concepts/customization).

Les compteurs de la capture ne mesurent ni le coût ni l’utilisation dans une réponse. Plus précisément, pour le harnais Copilot, le nombre à côté de **Tools** compte les outils côté client activés et exclut les outils intégrés au harnais, affichés en lecture seule. Rendre un outil disponible ne supprime pas ses contrôles d’autorisation. Vérifier les appels et leurs résultats dans la session. [Microsoft : gérer les outils](https://code.visualstudio.com/docs/agents/run/tools).

## Quand une skill est-elle chargée?

La diapositive 12 compare deux parcours. Une **skill** apporte une méthode dans le contexte; un **outil** est appelé avec des paramètres pour agir ou obtenir un résultat. L’agent peut sélectionner les deux, mais leur activation n’a pas le même effet.

Dans Copilot, le chargement normal d’une skill est progressif :

1. **Découvrir :** le nom et la description rendent la skill repérable, sans charger d’avance toutes ses consignes et ressources.
2. **Sélectionner et charger :** si la tâche correspond, le modèle peut choisir la skill; le corps de `SKILL.md` rejoint alors le contexte. Une invocation explicite `/nom` peut aussi déclencher ce chargement.
3. **Consulter au besoin :** les fichiers associés et référencés sont utilisés pendant le travail. Charger la procédure ne lance pas automatiquement ses scripts : leur exécution passe par des outils et les contrôles applicables.

Une description précise indique ce que la skill apporte **et quand l’utiliser**. Une skill découverte n’est pas forcément choisie pour chaque tâche pertinente. Le chargement progressif limite le contenu ajouté; ce n’est pas une garantie de coût nul. [Microsoft : chargement des skills](https://code.visualstudio.com/docs/agent-customization/agent-skills#how-copilot-uses-skills).

### Deux réglages distincts dans le YAML de SKILL.md

| Configuration Copilot | Invocation avec `/nom` | Sélection automatique |
|---|---|---|
| Champs omis (valeurs par défaut) | Oui | Possible selon la pertinence |
| `disable-model-invocation: true` | Oui | Non |
| `user-invocable: false` | Non, commande masquée | Possible selon la pertinence |
| Les deux réglages ci-dessus | Non | Non |

**Dans HydroDataLab**, `.github/skills/validate-etl/SKILL.md` contient `disable-model-invocation: true`. Le lancement est donc volontaire avec `/validate-etl`; ce n’est pas le comportement par défaut de toutes les skills. Ces contrôles sont documentés pour Copilot; vérifier leur prise en charge dans un autre harnais. [Microsoft : commandes et activation](https://code.visualstudio.com/docs/agent-customization/agent-skills#use-skills-as-slash-commands).

**Une skill peut guider plusieurs appels d’outils** : lire la spécification, exécuter pytest, lire le rapport, résumer les preuves. Inversement, un outil peut servir sans aucune skill. Une skill ne crée pas un outil absent et ne lui accorde pas de permissions supplémentaires. Dans le parcours présenté, son contenu rejoint le contexte courant; le mode expérimental `context: fork` n’est pas utilisé.

## Comment l’agent choisit-il un outil?

Le panneau **Tools** répond à « Quels outils l’agent peut-il utiliser pour accomplir cette tâche? ». Le choix d’un appel précis intervient ensuite dans la boucle de travail. **Disponible**, **choisi** et **autorisé pour cet appel** sont trois choses distinctes.

1. **Le harnais détermine les outils accessibles.** Leur origine, la configuration et les capacités de la session comptent. Le panneau Tools gère les outils configurables; un agent personnalisé peut aussi définir sa sélection.
2. **Le modèle choisit un appel si nécessaire.** Il reçoit la demande, le contexte et les définitions des outils disponibles. Leur description indique leur utilité; le schéma des paramètres indique les entrées attendues. Le modèle propose un outil et ses arguments, ou peut répondre sans outil. Les instructions et les résultats précédents influencent cette décision.
3. **Le harnais applique les contrôles.** Selon les autorisations, il peut demander une confirmation ou refuser l’appel. Activer un outil ne dispense pas de ces contrôles.
4. **L’outil s’exécute, puis sa sortie revient au modèle.** Le résultat ou l’erreur enrichit le contexte. Le modèle peut appeler un autre outil, corriger sa démarche ou terminer.

[Microsoft : boucle et choix des outils](https://code.visualstudio.com/docs/agents/concepts/tools), [définitions, paramètres et déroulement d’un appel](https://code.visualstudio.com/api/extension-guides/ai/tools#tool-calling-flow).

**Exemple HydroDataLab :** « Vérifie que la transformation préserve les valeurs absentes. » L’agent peut lire le code et le test, puis appeler l’outil terminal avec `uv run --project etl pytest etl/tests/test_pipeline.py`. Après les contrôles applicables, le terminal retourne la sortie de pytest; l’agent s’en sert pour son bilan. C’est un parcours possible, pas un ordre garanti. Dans cet exemple, **terminal** désigne l’outil de l’agent, et **pytest** le programme qu’il lui demande d’exécuter. Un autre environnement pourrait exposer un outil de test spécialisé.

**Pour orienter le choix :** préciser le résultat et la vérification attendus; utiliser `#` pour choisir une référence d’outil proposée par la session; limiter les outils configurables à ceux utiles à la tâche. Le menu `#` propose aussi des fichiers et d’autres éléments de contexte : ce ne sont pas tous des outils. Une référence explicite ne crée pas une capacité absente et ne contourne pas les permissions. Examiner les appels, leurs paramètres et leurs résultats : le modèle peut choisir un outil inadéquat. [Microsoft : sélectionner et référencer les outils](https://code.visualstudio.com/docs/agents/run/tools).

## Les sept concepts et leur déclenchement

| Concept | Exemple | Format ou point d’entrée | Déclenchement |
|---|---|---|---|
| Instruction | Préserver les absences | Fichiers ci-dessus | Selon portée et activation. |
| Skill | Valider l’ETL | `.github/skills/validate-etl/SKILL.md` | Pertinence ou commande; notre skill est volontaire. |
| Agent personnalisé | Réviseur BI | `.github/agents/reviseur-bi.agent.md` | Rôle sélectionné / invocation prise en charge. |
| Tool / outil | Terminal pour lancer pytest | Tools : capacités disponibles | Choix du modèle selon la tâche et le contexte, puis contrôles du harnais. |
| Serveur MCP | Proposer une lecture SQL | MCP Servers; [emplacements Copilot et Local](aide-memoire.md#mcp--emplacements-selon-le-harnais-repères-vs-code-1142) | Un outil exposé par le serveur est appelé. |
| Hook | Journaliser un appel | Hooks; configuration du harnais | Événement de cycle de vie, sans dépendre d’une décision du modèle. |
| Plugin | Distribuer skill et MCP ensemble | Plugins; paquet et manifeste | Installation; chaque composant garde ses règles. |

MCP signifie **Model Context Protocol**. Un outil n’exige pas nécessairement MCP. Un plugin regroupe des capacités; il ne devient pas une nouvelle priorité des instructions. SQL, hook et plugin ci-dessus sont des exemples illustratifs; la capture ne permet pas d’identifier les composants précis installés derrière ses compteurs. [Comparaison Microsoft](https://code.visualstudio.com/docs/agents/concepts/customization), [serveurs MCP](https://code.visualstudio.com/docs/agent-customization/mcp-servers), [plugins](https://code.visualstudio.com/docs/agent-customization/agent-plugins).

### MCP : choisir le fichier du harnais actif

L’encadré de la slide 11 compare **Copilot** (`%USERPROFILE%\.copilot\mcp-config.json` personnel, `.mcp.json` projet) et **Local** (`mcp.json` du profil VS Code, `.vscode/mcp.json` projet). La clé JSON est respectivement `mcpServers` ou `servers`.

**Repère de version :** la 1.142 déploie progressivement Copilot par défaut, déjà accessible auparavant. Elle ne date pas l’apparition de ces chemins. L’[aide-mémoire](aide-memoire.md#mcp--emplacements-selon-le-harnais-repères-vs-code-1142) précise le réglage expérimental pour la destination projet, `COPILOT_HOME`, la compatibilité Local et la migration. [Notes 1.142](https://code.visualstudio.com/updates/v1_142#copilot-harness), [guide Microsoft](https://code.visualstudio.com/docs/agent-customization/migrate-customizations#migrate-mcp-servers).

### Une recette : skill

```md
---
name: validate-etl
description: Vérifier une transformation Python et son rapport.
disable-model-invocation: true
---
1. Lire la spécification et le dictionnaire concernés.
2. Exécuter les tests ciblés.
3. Vérifier unités, couverture et provenance.
4. Résumer les preuves et les limites.
```

Dans notre exemple, `disable-model-invocation: true` réserve le lancement à l’utilisateur : `/validate-etl`. Une skill est un dossier qui peut contenir scripts et ressources. [Skills : format et activation](https://code.visualstudio.com/docs/agent-customization/agent-skills).

### Un rôle : agent personnalisé

Le fichier `reviseur-bi.agent.md` contient un en-tête YAML (description, outils selon le harnais) puis les consignes Markdown. Notre rôle relit les calculs et rapporte les problèmes; sa configuration limite les outils. Utiliser le sélecteur réel pour choisir les noms d’outils disponibles. [Agents personnalisés](https://code.visualstudio.com/docs/agent-customization/custom-agents).

### Une action déterministe : hook

Pour Copilot, suivre le **schéma GitHub Copilot / SDK**, pas un exemple Local copié tel quel. La présence du même chemin `.github/hooks/` ne garantit pas des événements ou charges utiles identiques. Tester un hook isolément avant de l’adopter. [Compatibilité des hooks](https://code.visualstudio.com/docs/agent-customization/hooks#choose-the-hook-implementation-for-your-session).

## Relier les fichiers au panneau Customizations

1. Ouvrir la racine HydroDataLab et sélectionner le harnais **Copilot**.
2. Ouvrir **Customizations → Instructions**, ou **Chat: Open Customizations**.
3. Repérer **Workspace** pour les fichiers du dépôt, **User** pour les préférences personnelles.
4. Ouvrir l’élément, examiner son fichier et son en-tête.
5. Faire une petite tâche représentative; examiner **References**, les actions et les tests.

La nouvelle capture fournie par le présentateur, intégrée en annexe 25, montre **Overview** pour **Copilot · Formation Copilot - HQ**. Les portées User et Workspace se consultent en ouvrant Instructions; elles ne sont pas visibles dans cette vue d’ensemble. Les chemins sous la capture expliquent ces portées, sans prétendre être des entrées de l’image. [Gestion et vérification des personnalisations](https://code.visualstudio.com/docs/agent-customization/overview#verify-a-customization).

Si un fichier manque : vérifier harnais, dossier ouvert, emplacement et activation. Le réglage des dossiers parents et les fichiers AGENTS imbriqués demandent une vérification spécifique. Pour Local, `chat.useNestedAgentsMdFiles` est expérimental; ce n’est pas un commutateur universel pour Copilot. Les anciens `chat.*FilesLocations` peuvent nécessiter une migration.

## Canvas : survol de la nouveauté 1.142

Un **Canvas** est une surface interactive associée au chat, fournie par une extension. L’utilisateur et l’agent peuvent y agir sur le même objet. Exemple BI proposé : un tableau des étapes extraire, valider, transformer, exporter, avec critères de réussite.

Pour l’essayer après la formation : activer `chat.canvases.enabled`, ouvrir une nouvelle session Copilot locale dans l’application de bureau; utiliser `/create-canvas` si la skill est disponible, ou une extension Canvas installée. **+ → Canvas** ouvre une surface disponible. La préversion n’est pas disponible dans le harnais Local, Claude, Codex, une session distante ou le navigateur. Ce diaporama Slidev et l’application Vue restent des applications distinctes; ils ne sont pas connectés à Canvas. [Documentation Canvas](https://code.visualstudio.com/docs/agents/run/agent-harnesses#use-canvases-with-the-copilot-harness).

## Réglages pour présenter

`Ctrl+,` ouvre les paramètres. Vérifier d’abord leur portée User/Workspace, puis le zoom et le retour à la ligne. `sessions.editor.wordWrap` est expérimental; contrôler sa présence dans la version installée. La fenêtre Agents permet aussi d’aligner son panneau inférieur. Les réglages avancés sont des options, pas une liste à copier. [Configuration de la fenêtre Agents](https://code.visualstudio.com/docs/agents/run/agents-window-configuration), [référence des paramètres](https://code.visualstudio.com/docs/agents/reference/ai-settings).
