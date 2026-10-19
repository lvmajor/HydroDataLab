# Personnalisations de VS Code et du harnais Copilot

Vérifié le **18 octobre 2026**, pour VS Code **1.142**. Cette fiche accompagne les diapositives 8 à 13, 23 et 25. Les formats cités sont liés à leur documentation officielle.

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

La diapositive 13 relie cinq sources au **Model context — Contexte du modèle** :

1. **Project instructions — Consignes du projet** : les règles communes de `copilot-instructions.md`.
2. **File-specific instructions — Consignes ciblées** : les règles Python dans `python.instructions.md`, activées selon les mécanismes décrits plus haut.
3. **Reference documents — Documents de référence** : le dictionnaire des données joint à la demande ou lu par l’agent.
4. **Task prompt — Demande courante** : le résultat voulu et la spécification à suivre pour cette tâche.
5. **Tool results — Résultats des outils** : les sorties réelles de pytest, du terminal ou du rapport de qualité.

Les deux premières sources apportent les consignes durables; les suivantes cadrent le travail et permettent de vérifier son résultat. C’est notre organisation pédagogique, pas une liste de niveaux officiels de VS Code. La position dans le dessin ne donne pas une priorité. L’historique et d’autres instructions peuvent également contribuer au contexte. [Guide Microsoft pour structurer le contexte](https://code.visualstudio.com/docs/agents/guides/context-engineering-guide).

## Les sept concepts et leur déclenchement

| Concept | Exemple | Format ou point d’entrée | Déclenchement |
|---|---|---|---|
| Instruction | Préserver les absences | Fichiers ci-dessus | Selon portée et activation. |
| Skill | Valider l’ETL | `.github/skills/validate-etl/SKILL.md` | Pertinence ou commande; notre skill est volontaire. |
| Agent personnalisé | Réviseur BI | `.github/agents/reviseur-bi.agent.md` | Rôle sélectionné / invocation prise en charge. |
| Tool / outil | Lire CSV, lancer pytest | Catalogue Tools et outils autorisés | Appel demandé par l’agent, avec les contrôles du harnais. |
| Serveur MCP | Proposer une lecture SQL | MCP Servers; configuration du harnais | Un outil exposé par le serveur est appelé. |
| Hook | Journaliser un appel | Hooks; configuration du harnais | Événement de cycle de vie, sans dépendre d’une décision du modèle. |
| Plugin | Distribuer skill et MCP ensemble | Plugins; paquet et manifeste | Installation; chaque composant garde ses règles. |

MCP signifie **Model Context Protocol**. Un outil n’exige pas nécessairement MCP. Un plugin regroupe des capacités; il ne devient pas une nouvelle priorité des instructions. SQL, hook et plugin ci-dessus sont des exemples conceptuels; ils ne sont pas installés dans HydroDataLab. [Comparaison Microsoft](https://code.visualstudio.com/docs/agents/concepts/customization), [serveurs MCP](https://code.visualstudio.com/docs/agent-customization/mcp-servers), [plugins](https://code.visualstudio.com/docs/agent-customization/agent-plugins).

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

La capture fournie par le présentateur, intégrée en annexe 23, provient d’un autre projet et montre **User 0 / Workspace 0**. Elle illustre le panneau; elle ne certifie pas que les fichiers HydroDataLab y sont chargés. [Gestion et vérification des personnalisations](https://code.visualstudio.com/docs/agent-customization/overview#verify-a-customization).

Si un fichier manque : vérifier harnais, dossier ouvert, emplacement et activation. Le réglage des dossiers parents et les fichiers AGENTS imbriqués demandent une vérification spécifique. Pour Local, `chat.useNestedAgentsMdFiles` est expérimental; ce n’est pas un commutateur universel pour Copilot. Les anciens `chat.*FilesLocations` peuvent nécessiter une migration.

## Canvas : survol de la nouveauté 1.142

Un **Canvas** est une surface interactive associée au chat, fournie par une extension. L’utilisateur et l’agent peuvent y agir sur le même objet. Exemple BI proposé : un tableau des étapes extraire, valider, transformer, exporter, avec critères de réussite.

Pour l’essayer après la formation : activer `chat.canvases.enabled`, ouvrir une nouvelle session Copilot locale dans l’application de bureau; utiliser `/create-canvas` si la skill est disponible, ou une extension Canvas installée. **+ → Canvas** ouvre une surface disponible. La préversion n’est pas disponible dans le harnais Local, Claude, Codex, une session distante ou le navigateur. Ce diaporama Slidev et l’application Vue restent des applications distinctes; ils ne sont pas connectés à Canvas. [Documentation Canvas](https://code.visualstudio.com/docs/agents/run/agent-harnesses#use-canvases-with-the-copilot-harness).

## Réglages pour présenter

`Ctrl+,` ouvre les paramètres. Vérifier d’abord leur portée User/Workspace, puis le zoom et le retour à la ligne. `sessions.editor.wordWrap` est expérimental; contrôler sa présence dans la version installée. La fenêtre Agents permet aussi d’aligner son panneau inférieur. Les réglages avancés sont des options, pas une liste à copier. [Configuration de la fenêtre Agents](https://code.visualstudio.com/docs/agents/run/agents-window-configuration), [référence des paramètres](https://code.visualstudio.com/docs/agents/reference/ai-settings).
