# Registre des sources — HydroDataLab

Vérification documentaire : **18 octobre 2026**. Prestation : **20 octobre 2026**. Sources primaires sauf les ressources communautaires identifiées. La date de consultation ne garantit pas que chaque fonctionnalité est activée dans le tenant HQ ou la version installée. Recommandations de modèles et organisation du contexte = choix pédagogiques, pas benchmarks fournisseurs.

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

S22 — [Git : worktree](https://git-scm.com/docs/git-worktree), consulté le 18 octobre 2026. Un dépôt peut posséder plusieurs dossiers de travail liés, ce qui permet d'ouvrir plusieurs branches simultanément. La diapositive 5 explique dépôt, commit, branche et worktree avant leur emploi; un worktree supplémentaire reste facultatif dans ce parcours.

Précision S01/S16, revérifiée le 18 octobre : la fenêtre Agents ne propose pas l'agent Plan dans son menu; `/plan` fonctionne dans une session Copilot. Les notes et le guide distinguent ce chemin des sélecteurs de la vue Chat.

## Instantané réellement utilisé

Manifeste lu le 18 octobre 2026 : récupération **2026-10-18T19:44:01Z**, période **17 octobre 2026**, début inclusif et fin exclusive au 18 octobre dans America/Toronto. **168 observations** : 96 de demande, 72 hydrométéorologiques. Identifiants source filtrés : `1-15412` pour Niveau et `1-10622` pour Température Minimum/Maximum. Les noms de station, unités, fréquences et définitions proviennent des données/metadata; ne pas attribuer implicitement les températures au même capteur que le niveau. Licence déclarée par le manifeste : **CC BY-NC 4.0**. Conserver les fichiers bruts, l’attribution et leur empreinte.

## Limites éditoriales à préserver

- Pas d’estimation des réserves en GWh ni de prévision opérationnelle; le niveau Gouin reste exprimé en mètres.
- Les exemples 24/24, 23/24 et les jours de 23/25 heures sont des cas de tests, pas des données hydrologiques inventées.
- Les coûts du simulateur supposent un volume égal entre modèles, sans cache ni remise; ce sont des illustrations, pas des mesures.
- HydraFusion peut employer un pool comprenant Astra selon l’annonce; le parcours HQ n’en fait pas une démo exécutable ni une recommandation autorisée.
- Aucune capture d’interface inventée. Les schémas décrivent des concepts et sont étiquetés comme tels.
- Les références web sont consultables sur action; le build de la présentation conserve localement scripts et polices.
