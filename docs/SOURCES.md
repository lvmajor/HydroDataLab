# Registre des sources — HydroDataLab

Vérification documentaire initiale : **18 octobre 2026**; ajouts S32–S39 vérifiés le **19 octobre**. Prestation : **20 octobre 2026**. Sources primaires sauf les ressources communautaires identifiées. La date de consultation ne garantit pas que chaque fonctionnalité est activée dans le tenant HQ ou la version installée. Recommandations de modèles et organisation du contexte = choix pédagogiques, pas benchmarks fournisseurs.

| ID | Source | Fait utilisé / date de publication connue |
|---|---|---|
| S01 | [VS Code — Agents window](https://code.visualstudio.com/docs/agents/run/agents-window) | Fenêtre dédiée, sessions, Changes, `code --agents`; doc mise à jour 14/10/2026. |
| S02 | [VS Code — Configure Agents window](https://code.visualstudio.com/docs/agents/run/agents-window-configuration) | Paramètres partagés, portée, wordWrap, panneaux et extensions; 14/10/2026. |
| S03 | [VS Code — Custom instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions) | Copilot/Local, racine, applyTo, description, References, règles additives; 14/10/2026. |
| S04 | [VS Code — AI settings](https://code.visualstudio.com/docs/agents/reference/ai-settings) | Noms et statuts des paramètres; 14/10/2026. |
| S05 | [VS Code — Context](https://code.visualstudio.com/docs/agents/concepts/context) | Historique, index, sorties d’outils, compaction, sous-agents; 14/10/2026. |
| S06 | [VS Code — Best practices](https://code.visualstudio.com/docs/agents/best-practices) | Tâches bornées, critères, revue; limites des checkpoints; 14/10/2026. |
| S07 | [VS Code — Agent skills](https://code.visualstudio.com/docs/agent-customization/agent-skills) | Chargement progressif et déclenchement manuel; consultation 18/10/2026. |
| S08 | [VS Code — Custom agents](https://code.visualstudio.com/docs/agent-customization/custom-agents) | Rôles, outils, modèle optionnel; 14/10/2026. |
| S09 | [VS Code — Migrate customizations](https://code.visualstudio.com/docs/agent-customization/migrate-customizations) | Copilot ne charge pas prompt.md; migration vers skills; 14/10/2026. |
| S10 | [VS Code — Context engineering](https://code.visualstudio.com/docs/agents/guides/context-engineering-guide) | Contexte concis, plan puis implémentation; 14/10/2026. |
| S11 | [HQ — Demande électrique](https://donnees.hydroquebec.com/explore/dataset/demande-electricite-quebec/) et [HQ — Données hydrométéorologiques](https://donnees.hydroquebec.com/explore/dataset/donnees-hydrometeorologiques/) | Sources effectivement utilisées; URLs de requête, SHA-256 et licence dans `data/fixtures/manifest.json`. |
| S12 | [Contrat local](CONTRACT.md) | Rapport schemaVersion1, fréquence par série, period fin exclusive America/Toronto, points UTC, scénario missing en mémoire. |
| S13 | [GitHub — Models and pricing](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing) | Tarifs USD/million tokens, cache, paliers longs; 1 crédit = 0,01 USD; consulté 18/10/2026. |
| S14 | [GitHub — Auto model selection](https://docs.github.com/en/copilot/concepts/models/auto-model-selection) | Efficiency/Balance/Intelligence, politiques, remise 10% plans payants; consulté 18/10/2026. |
| S15 | [GitHub — HydraFusion updates and Max](https://github.blog/changelog/2026-10-14-hydrafusion-updates-and-new-max-option/) | Research preview, orchestration, Max; publié 14/10/2026. |
| S16 | [GitHub Spec Kit](https://github.github.com/spec-kit/) et [VS Code — Planning](https://code.visualstudio.com/docs/agents/run/planning) | Intention/spec/plan/tâches/implémentation; conserver un plan relu. |
| S17 | [Microsoft Learn — Agent mode](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/) | Module annoncé 50 minutes, six unités; consulté 18/10/2026. |
| S18 | [Awesome Copilot](https://awesome-copilot.github.com/), [dépôt](https://github.com/github/awesome-copilot), [GitHub Community](https://github.com/community/community/discussions/categories/copilot?locale=en-US) | Exemples et échanges communautaires à relire et adapter. |
| S19 | [Slidev — Export](https://sli.dev/guide/exporting) et [polices](https://sli.dev/custom/config-fonts) | Export CLI Playwright, PDF statique, polices locales; consulté 18/10/2026. |
| S20 | [Hydro-Québec — Réservoirs](https://www.hydroquebec.com/comprendre/hydroelectricite/gestion-eau.html) | Distingue volumes total/utilisable; un niveau n’est pas un stock énergétique. |
| S21 | [CEHQ — Données instantanées](https://www.cehq.gouv.qc.ca/hydrometrie/historique_donnees/fiche_instantanee.asp) | Source complémentaire non utilisée dans le snapshot : données instantanées préliminaires, anomalies/corrections possibles. |
| S23 | [VS Code 1.142](https://code.visualstudio.com/updates/v1_142) | 14/10/2026 : harnais Copilot déjà disponible, déploiement progressif par défaut; Canvas en Preview. Ne date pas l’apparition des chemins MCP. |
| S24 | [Concept de harnais](https://code.visualstudio.com/docs/agents/concepts/agent-harnesses) et [choix et Canvas](https://code.visualstudio.com/docs/agents/run/agent-harnesses) | Séparer harnais/modèle; Canvas desktop local Copilot Agent Host, extension et activation. |
| S25 | [Concepts de personnalisation](https://code.visualstudio.com/docs/agents/concepts/customization) | Instructions, skills, agents, tools, MCP, hooks, plugins : responsabilités et composition. |
| S26 | [Panneau Customizations](https://code.visualstudio.com/docs/agent-customization/overview) | Portées User/Workspace/Organization, fichier découvert vs comportement vérifié. |
| S27 | [Hooks](https://code.visualstudio.com/docs/agent-customization/hooks) et [MCP](https://code.visualstudio.com/docs/agent-customization/mcp-servers#choose-a-configuration-destination) | Hooks Copilot SDK différents de Local; destinations MCP personnelles/projet, format mcpServers/servers, destination projet expérimentale. Relu le 19/10. |
| S28 | [Plugins](https://code.visualstudio.com/docs/agent-customization/agent-plugins) | Paquets de capacités; chaque composant conserve son activation. |
| S29 | [HydraFusion, documentation](https://docs.github.com/en/early-access/copilot/hydrafusion) | Single/Cascade/Critique; somme des usages par modèle, aucun supplément propre, aucune remise Auto. |
| S30 | [GitHub — Comparaison des modèles](https://docs.github.com/en/copilot/reference/ai-models/model-comparison) | Complément à la grille de prix; les recommandations et volumes du cours restent pédagogiques. |

S22 — [Git : worktree](https://git-scm.com/docs/git-worktree), consulté le 18 octobre 2026. Un dépôt peut posséder plusieurs dossiers de travail liés, ce qui permet d'ouvrir plusieurs branches simultanément. La diapositive 6 explique dépôt, commit, branche et worktree avant leur emploi; un worktree supplémentaire reste facultatif dans ce parcours.

Précision S01/S16, revérifiée le 18 octobre : la fenêtre Agents ne propose pas l'agent Plan dans son menu; `/plan` fonctionne dans une session Copilot. Les notes et le guide distinguent ce chemin des sélecteurs de la vue Chat.

## Instantané réellement utilisé

Manifeste lu le 18 octobre 2026 : récupération **2026-10-18T19:44:01Z**, période **17 octobre 2026**, début inclusif et fin exclusive au 18 octobre dans America/Toronto. **168 observations** : 96 de demande, 72 hydrométéorologiques. Identifiants source filtrés : `1-15412` pour Niveau et `1-10622` pour Température Minimum/Maximum. Les noms de station, unités, fréquences et définitions proviennent des données/metadata; ne pas attribuer implicitement les températures au même capteur que le niveau. Licence déclarée par le manifeste : **CC BY-NC 4.0**. Conserver les fichiers bruts, l’attribution et leur empreinte.

## Ajouts du 19 octobre : personnalisation, skills et veille

S32 — [Microsoft : migration vers le harnais Copilot](https://code.visualstudio.com/docs/agent-customization/migrate-customizations). Des formats et emplacements peuvent nécessiter une migration. Fonde l’exemple d’entretien de la diapositive 13; la recommandation « vanille » est notre analyse, pas une conclusion chiffrée de Microsoft.

S33 — [Microsoft : Agent Skills](https://code.visualstudio.com/docs/agent-customization/agent-skills). Chargement progressif, format ouvert, différences de capacités entre harnais et revue des skills partagés.

S34 — [Microsoft : playwright-cli](https://github.com/microsoft/playwright-cli). CLI accompagnée de skills, pilotage de navigateur et captures; distinction avec le serveur MCP. Le catalogue est proposé, sans installation par cette révision.

S35 — [Aspire : agents IA](https://aspire.dev/get-started/ai-coding-agents/) et [aspire agent init](https://aspire.dev/reference/cli/commands/aspire-agent-init/). Skills pour la CLI, santé et journaux; MCP facultatif.

S36 — [Équipe .NET : dotnet/skills](https://github.com/dotnet/skills). Collections dotnet-aspnetcore et dotnet-test; ne pas confondre skill portable, agent propre au harnais et plugin de distribution.

S37 — [Matt Pocock : skills](https://github.com/mattpocock/skills). grill-me et tdd sont présentés comme pistes en exploration, non validées dans notre démo ni dans le compte HQ.

S38 — [skills.sh : répertoire](https://skills.sh/) et [fiche playwright-cli](https://skills.sh/microsoft/playwright-cli/playwright-cli). Indices d’adoption datés dans la fiche participant; ils ne constituent ni un consensus « must have », ni une preuve de qualité. Les descriptions techniques sont vérifiées dans les dépôts d’origine.

S39 — [Nouveautés VS Code](https://code.visualstudio.com/updates), [changelog Copilot](https://github.blog/changelog/label/copilot/), [Microsoft Learn](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/), [Awesome Copilot](https://github.com/github/awesome-copilot), [GitHub Community](https://github.com/community/community/discussions/categories/copilot), [Aspire](https://aspire.dev/) et [Spec Kit](https://github.github.com/spec-kit/). Liens de la page finale, consultés le 19 octobre. Les trois ressources HQ sont des emplacements à compléter, sans adresse inventée.

## Limites éditoriales à préserver

- Pas d’estimation des réserves en GWh ni de prévision opérationnelle; le niveau Gouin reste exprimé en mètres.
- Les exemples 24/24, 23/24 et les jours de 23/25 heures sont des cas de tests, pas des données hydrologiques inventées.
- Les coûts du graphique supposent quatre scénarios de volume, égal entre modèles pour un scénario, sans cache ni remise. Ce sont des illustrations, pas des mesures de performance ou une prévision de facture.
- HydraFusion peut choisir un ou plusieurs modèles autorisés et disponibles; la capsule n’exige pas son activation. Aucune politique HQ sur un modèle n’est présumée.
- Aucune capture d’interface inventée. Les schémas décrivent des concepts et sont étiquetés comme tels.
- Les références web sont consultables sur action; le build de la présentation conserve localement scripts et polices.

## Capture fournie et distinction des démonstrations

La capture `presentation/public/vscode-customizations-overview.png` est l’image fournie le 19 octobre, conservée sans modification des pixels. Le cadrage CSS de l’annexe 24 montre le menu et Customize Your Agent pour Copilot / Formation Copilot - HQ; l’image complète est liée dans la fiche des personnalisations. Les chemins expliqués sous l’image décrivent les portées accessibles dans Instructions, sans prétendre être visibles dans cette vue Overview. L’ancienne image `vscode-customizations-user.png` reste conservée mais n’est plus affichée. Le schéma de contexte est pédagogique; les exemples SQL, hooks et plugins sont illustratifs et ne permettent pas d’identifier les composants installés derrière les compteurs de la capture. Le schéma Canvas décrit une fonctionnalité et ne prétend pas être relié à Copilot.

S40 — [Microsoft : panneau Customizations et création assistée](https://code.visualstudio.com/docs/agent-customization/overview), relu le 19 octobre. Overview, génération de personnalisations à relire, portée et validation. La slide 11 reprend les libellés exacts de la nouvelle capture; les cartes Explore Customizations répètent les catégories du menu.

S41 — [Microsoft : utiliser et gérer les outils](https://code.visualstudio.com/docs/agents/run/tools), consulté le 19 octobre. Origine des outils, sélection des groupes et outils configurables, compteur Tools côté client pour Copilot, disponibilité distincte des autorisations.

S42 — [Microsoft : outils dans la boucle agentique](https://code.visualstudio.com/docs/agents/concepts/tools) et [Language Model Tool API](https://code.visualstudio.com/api/extension-guides/ai/tools), consultés le 19 octobre. Sélection par le modèle selon demande/contexte et définitions des outils, rôle de modelDescription et du schéma d’entrée, paramètres proposés, exécution contrôlée et retour du résultat. La slide 11 présente le catalogue Tools; la slide 12 et la fiche développent le choix d’un appel. Terminal/pytest constitue un exemple pédagogique, pas une séquence imposée par Copilot.

S43 — [Microsoft : Agent Skills](https://code.visualstudio.com/docs/agent-customization/agent-skills), relu le 19 octobre : sections SKILL.md file format, Use skills as slash commands et How Copilot uses skills. Nom/description, corps chargé à la demande, références consultées au besoin; valeurs par défaut et effets distincts de disable-model-invocation et user-invocable. Le schéma en slide 12 explique le fonctionnement courant dans Copilot, sans activer le mode expérimental fork. La skill du dépôt reste volontaire. [GitHub : rôle des skills](https://docs.github.com/en/copilot/concepts/agents/about-agent-skills).

Pour S03, la page officielle sans paramètres a été intermittente pendant la consultation; la même page indexée avec un paramètre de suivi a permis de lire les sections sur le format, AGENTS.md, la vérification et les instructions d’organisation. Les supports utilisent son URL canonique. Ne pas confondre une organisation des dossiers avec un ordre de priorité universel, ni transférer les règles GitHub.com à tous les harnais VS Code.

## Contexte du public et sécurité

S31 — [Microsoft : sécurité du développement avec les agents](https://code.visualstudio.com/docs/agents/run/security), consulté le 18 octobre 2026. Référence de la diapositive 2 : contrôle des permissions, protection des secrets et revue des changements. Le contexte d’expérimentation plus souple vient du présentateur; il ne constitue pas une politique HQ vérifiée.
