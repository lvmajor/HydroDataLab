---
theme: default
favicon: /favicon.png
title: HydroDataLab — Des agents IA aux données fiables
author: Formation Hydro-Québec
info: Support du 20 octobre 2026. Sources vérifiées le 18 octobre 2026.
class: cover
colorSchema: light
aspectRatio: 16/9
canvasWidth: 980
transition: fade
fonts:
  sans: IBM Plex Sans
  serif: Barlow Condensed
  mono: IBM Plex Mono
  provider: none
  local: [IBM Plex Sans, Barlow Condensed, IBM Plex Mono]
drawings:
  enabled: false
mdc: true
exportFilename: formation-hydrodatalab
---

# Des agents IA<br>aux données fiables

HydroDataLab<br>Une pratique concrète pour les analyses Python

<div class="source">20 octobre 2026 • Sources vérifiées le 18 octobre • Démonstration pédagogique, données publiques</div>

<!--
30 secondes, 00:00–00:30. Ouvrir avec une question : « Si l’agent produit un beau graphique, qu’est-ce qui vous permet de croire aux données? » La formation porte sur la délégation d’un travail vérifiable. Le projet est polyglotte, mais le geste principal sera une modification Python accessible aux personnes qui font du BI. L’agent n’est pas une autorité sur l’hydrologie. Ce support n’est pas une publication officielle d’Hydro-Québec. Les chiffres d’exemple sont signalés; les données réelles viennent des sources publiques et de leur instantané documenté.
-->

---
class: audience-context
---

# Votre terrain de développement

<p class="context-audience">Analystes BI et développeurs citoyens : Python, ETL et visualisation de données.</p>

<div class="local-to-container">
<div><b>Développer</b><span>Explorer une idée.<br>Créer un traitement ou un visuel.</span></div>
<span class="route-arrow" aria-hidden="true">→</span>
<div><b>Tester sur le poste</b><span>Vérifier les calculs.<br>Rejouer avec des données d’essai.</span></div>
<span class="route-arrow" aria-hidden="true">→</span>
<div><b>Préparer le déploiement</b><span>Emballer code et dépendances<br>pour un environnement conteneurisé.</span></div>
</div>

<div class="context-practices">
<div><h2>Une marge pour expérimenter</h2><p>Un cadre plus souple : l’occasion<br>d’essayer, de comparer et d’apprendre.</p></div>
<div><h2>La sécurité au quotidien</h2><p>Données autorisées, secrets protégés.<br>Accès limités, code relu et testé.</p></div>
</div>

<div class="context-takeaway">L’IA accélère les essais; vous gardez la maîtrise des données et des actions.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/run/security">Microsoft · sécurité du développement avec les agents</a></div>

<!--
1 minute, 00:30–01:30. Partir du travail réel des participants, avant de présenter HydroDataLab : développer et tester sur leur poste des ETL Python et des visualisations qui pourront être exécutés dans des environnements conteneurisés. ETL signifie extraire, transformer et charger ou exporter des données. Un conteneur exécute une application à partir d’une image qui regroupe son code et ses dépendances; le déploiement demande encore une configuration et un environnement autorisé.
Souligner la chance d’avoir une marge d’expérimentation : essayer une approche, comparer deux résultats, apprendre rapidement. Le cadre plus souple est le contexte décrit par l’organisateur, pas une exemption aux règles de sécurité HQ ni une description vérifiée de ses politiques. Garder quatre réflexes : utiliser des données autorisées pour l’outil et l’usage; protéger les clés et mots de passe; limiter les accès accordés à l’agent; relire et tester ses changements. Un ETL exécuté localement ne signifie pas que les données envoyées à un service IA restent sur le poste. Les droits et règles internes s’appliquent aussi à un prototype. Ici, les données publiques et leurs copies préparées permettent de pratiquer. Transition : « Comment utiliser cette liberté pour produire un résultat dont on peut vérifier la qualité? »
-->

---

# Du besoin BI au résultat vérifié

<div class="lead">Notre défi : détecter les mesures manquantes<br>dans une journée de données.</div>

<div class="columns"><div>

## Ce que vous saurez faire

Donner une tâche précise à l’agent.<br>Retrouver les règles qu’il utilise.<br>Vérifier le calcul qu’il propose.

</div><div>

## Notre exemple Python

Données publiques Hydro-Québec.<br>Un traitement réutilisable.<br>Un indicateur de qualité visible.

</div></div>

<div class="strip">45 minutes de présentation et de démos, puis vos questions.</div>

<!--
30 secondes, 01:30–02:00. Public BI/Python : aucune maîtrise de Git, Docker, .NET ou Vue n’est supposée. Les mots utiles seront expliqués avant leur emploi. Les démonstrations sont réparties dans les 45 minutes; 10 minutes de questions et 5 de marge suivent. Les annexes 20 à 25 servent aux questions. Objectif de sortie : le participant peut décrire une règle, donner les bons éléments à l’agent et examiner une preuve de résultat. Ne pas dérouler une liste de technologies.
-->

---

# Quatre séries, quatre sens différents

| Série | Unité | Ce que l’on observe |
|---|---:|---|
| Demande électrique au Québec | MW | Une puissance demandée |
| Niveau d’eau au réservoir Gouin | m | Une hauteur dans le référentiel source |
| Température minimale | °C | Un minimum sur la période source |
| Température maximale | °C | Un maximum sur la période source |

<div class="small">Notre ETL : extraire les données, les transformer, puis charger ou exporter le résultat.<br>Un niveau en mètres ne donne pas une réserve en GWh.</div>

<div class="source">Sources publiques HQ et contrat [S11, S12]. Instantané : 17 octobre 2026, 168 observations; récupéré le 18 octobre à 19:44 UTC.</div>

<!--
2 minutes, 02:00–04:00. Les fréquences diffèrent : demande à 15 minutes, niveau horaire, minima/maxima selon la définition source. intervalMinutes porte cette information; ne pas attendre 24 valeurs partout. Ne pas convertir un niveau en réserve ni une puissance instantanée en énergie. Le manifeste confirme 168 observations pour le 17 octobre, récupérées le 18 octobre à 19:44:01 UTC. Les températures proviennent de leur station source, distincte du capteur de niveau Gouin. Le cas explore qualité et provenance, pas les opérations hydriques.
-->

---

# Un modèle, un harnais, un agent

| Notion | Dans notre tâche Python |
|---|---|
| **Modèle** | Propose du code et raisonne sur le résultat. |
| **Outil · tool** | Lit un fichier, le modifie ou lance Python. |
| **Harnais · harness** | Relie modèle, contexte, outils et autorisations. |
| **Agent** | Boucle sur ces actions pour accomplir votre tâche. |

<div class="strip compact"><b>Notre choix : harnais Copilot dans VS Code.</b><br>Il utilise le Copilot SDK. Choisir un modèle Claude conserve ce harnais.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/concepts/agent-harnesses">Microsoft · comprendre les harnais</a> · <a href="https://code.visualstudio.com/updates/v1_142#copilot-harness">Nouveautés 1.142</a></div>

<!--
2 minutes, 04:00–06:00. Partir d’un script familier. Le modèle propose, un outil exécute Python, le harnais prépare le contexte et applique les autorisations, l’agent poursuit selon le résultat. Le Copilot SDK est une base commune aux produits Copilot; cela ne garantit pas des outils et réglages identiques partout. En 1.142, le harnais Copilot existant devient progressivement le défaut des nouvelles sessions; il n’est pas né dans cette version. Sélectionner explicitement Copilot pour le parcours. Local est le nom d’un autre harnais, pas un synonyme d’exécution sur le poste. Un modèle Claude dans Copilot ne bascule pas vers le harnais Claude.
-->

---

# Git : garder une trace de ses essais

| Mot rencontré | Ce qu’il signifie |
|---|---|
| **Dépôt Git** | Le projet et l’historique de ses versions enregistrées. |
| **Commit** | Un point enregistré dans cet historique. |
| **Branche** | Une suite de versions pour un essai, par exemple <code>demo-live</code>. |
| **Worktree** · facultatif | Un autre dossier du même dépôt, pour travailler sur une autre branche. |

<div class="strip">Pour débuter, une branche d’essai suffit.</div>

<div class="source"><a href="https://git-scm.com/docs/git-worktree">Documentation Git · worktree</a> [S22]. Les fichiers et résultats non enregistrés demandent leur propre sauvegarde.</div>

<!--
2 minutes, 06:00–08:00. Partir du besoin « essayer un calcul sans mélanger l’essai avec la version de référence ». Dépôt : dossier du projet avec son historique Git. Commit : version enregistrée, pas simple sauvegarde du fichier dans l’éditeur. Branche : chemin d’évolution nommé; main reste la référence, demo-live accueille notre essai jusqu’à sa revue. Un worktree supplémentaire ouvre une autre branche dans un autre dossier, tout en partageant l’historique du même dépôt. Il est facultatif pour cette initiation; nous n’en créons pas en direct. Ce n’est ni un conteneur, ni une sauvegarde, ni une isolation des bases et services externes. La démo préparée utilise la branche demo-live et des dossiers de secours; ne pas la présenter comme une démonstration de création de worktree. Dans la fenêtre Agents, le dossier sélectionné indique où les outils vont agir. Faire reformuler : « Dans quel dossier et sur quelle branche mon essai va-t-il être fait? » Référence : https://git-scm.com/docs/git-worktree
-->

---

# Démo 1 · donner une mission à l’agent

<div class="strip">« Repère le calcul des indicateurs et explique comment il est testé.<br>Ne modifie aucun fichier. »</div>

| Repère dans la fenêtre Agents | À quoi il sert |
|---|---|
| **Session** | Garder une conversation consacrée à cette tâche. |
| **Planification** | Demander une démarche avec <code>/plan</code> avant de modifier. |
| **Autorisations** | Choisir quelles actions doivent être confirmées. |

<div class="lead">Quelles sources permettent de vérifier sa réponse?</div>

<div class="source"><a href="https://code.visualstudio.com/docs/agents/run/agents-window">Microsoft · fenêtre Agents</a> · <a href="https://code.visualstudio.com/docs/agents/run/planning">Planification</a> · <a href="https://code.visualstudio.com/docs/agents/run/agents-window-configuration">Réglages</a></div>

<!--
1 minute d’explication puis 4 minutes de démo, 08:00–13:00. La diapositive donne une mission et une question au public; la manipulation suivante reste dans ces notes.
0:00–0:45 : ouvrir Chat: Open Agents Window (ou code --agents), montrer le dossier et la branche déjà expliqués. Présenter une session comme une conversation pour une tâche. Copilot est le moteur d’exécution choisi; le modèle sera sélectionné à l’intérieur de Copilot. Auto choisit le modèle pour nous; ses niveaux seront expliqués plus loin. Montrer /plan pour demander une démarche avant l'implémentation. La fenêtre Agents ne propose pas actuellement l'agent Plan dans son menu; /plan est disponible dans une session Copilot. Ne pas confondre les sélecteurs de la vue Chat et de cette fenêtre. Laisser les confirmations manuelles; expliquer une autorisation au moment où elle apparaît.
0:45–1:30 : Ctrl+, agrandir la lecture et montrer le retour à la ligne. Expliquer la portée : Utilisateur = mes projets; Espace de travail = ce projet. Ne pas appliquer un réglage global sans raison. Montrer le panneau Changes, soit la comparaison avant/après lorsqu’un fichier change.
1:30–2:20 : Chat: Open Customizations, puis Instructions. Une instruction est une consigne écrite destinée à l’agent; les règles permanentes seront détaillées à la diapositive suivante. Montrer la règle Python zéro/absence dans le dépôt.
2:20–4:00 : lancer le prompt de découverte dans handouts/prompts.md; lire References comme « fichiers ou sources consultés ». Ouvrir un fichier cité et faire vérifier une affirmation par le public. Découverte ne signifie pas respect : il faudra encore tester le comportement.
Si l’authentification échoue, suivre les mêmes repères dans les fichiers locaux, en annonçant que la réponse ne sera pas générée en direct.
-->

---

# Écrire une règle que l’on peut vérifier

<div class="small">Exemple fictif : trois créneaux horaires de température attendus.</div>

<div class="measure-example">
<div><span>08 h</span><b>10 °C</b><em>Mesure reçue</em></div>
<div><span>09 h</span><b>0 °C</b><em>Mesure reçue</em></div>
<div class="missing"><span>10 h</span><b>None</b><em>Aucune mesure</em></div>
</div>

<div class="strip compact"><b>2 mesures valides sur 3 : couverture de 66,67 %.</b><br>Remplacer l’absence par 0 inventerait une troisième mesure.</div>

<div class="rule-file"><b>Instruction permanente :</b> « Conserver les valeurs manquantes.<br>Compter 0 comme une mesure numérique valide. »<br><code>.github/instructions/python.instructions.md</code></div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Microsoft · instructions personnalisées</a> · <a href="/20">Voir le panneau Instructions →</a></div>

<!--
2 minutes, 13:00–15:00. Une instruction est une consigne durable, enregistrée dans le projet et relue comme le code. Montrer trois températures fictives : zéro Celsius est bien une mesure, None signifie qu’on n’en a pas reçu. En pandas, l’absence peut apparaître sous forme NaN ou NA; en JSON, null. La règle ne prétend pas qu’un zéro est physiquement plausible pour toute série : les contrôles métier restent nécessaires. Ici, ne pas convertir un manque en zéro pour remplir un graphique. Si on le faisait, la couverture deviendrait artificiellement 100 %. Le test doit compter deux valeurs, et non une (zéro exclu) ou trois (absence remplacée). Montrer le fichier du dépôt. Le lien interne conduit à la capture réelle en annexe 20; l’ouvrir pendant la démo seulement si nécessaire. Les notes et le guide fournissent la marche à suivre.
-->

---

# Où écrire la règle, et quand s’applique-t-elle?

<div class="small"><b>Socle Copilot :</b> <code>.github/copilot-instructions.md</code> · Markdown commun au projet.<br><b>Règle ciblée :</b> <code>.github/instructions/python.instructions.md</code></div>

```md
---
description: Qualité des transformations Python
applyTo: "etl/**/*.py"
---
Conserver les valeurs manquantes.
Compter 0 comme une mesure numérique valide.
```

<div class="small"><b>applyTo</b> : motif des fichiers créés ou modifiés. <b>description</b> : pertinence de la tâche.<br>Sans ces deux champs, joindre le fichier manuellement.</div>

<div class="strip compact">Pour ce groupe : socle Copilot + règles Python ciblées.<br>Plusieurs harnais compatibles? Préférer <code>AGENTS.md</code> pour le socle partagé.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions#instructions-file-format">Microsoft · format et activation</a> · <a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions#use-an-agentsmd-file">AGENTS.md</a></div>

<!--
2 minutes, 15:00–17:00. L’en-tête entre --- est du YAML; le corps est du Markdown. etl/**/*.py signifie les fichiers .py dans etl et ses sous-dossiers; ce chemin est relatif à la racine de l’espace de travail. applyTo et description sont optionnels et expriment deux mécanismes distincts; une description peut conduire l’agent à charger une règle même hors correspondance de fichier. instructions.md seul n’est pas un nom magique : conserver le suffixe .instructions.md et le dossier attendu. La recommandation de cette formation privilégie Copilot, l’outil effectivement utilisé par le public. Pour une équipe multiharnais, AGENTS.md à la racine constitue un socle portable si chaque harnais le prend en charge; les règles ciblées Copilot restent des compléments. Ne pas dupliquer les mêmes consignes dans deux gros fichiers. HydroDataLab conserve actuellement sa source commune dans copilot-instructions.md; AGENTS.md est une courte porte d’entrée qui demande sa lecture. Un renvoi Markdown ne garantit pas que son contenu est chargé : vérifier References et tester. Les fichiers AGENTS imbriqués ont des règles de découverte propres au harnais; le réglage Local expérimental n’est pas un réglage universel Copilot. Fiche détaillée : handouts/personnalisations-vscode.md.
-->

---

# Sept concepts, sept responsabilités

<div class="concept-table">

| Concept | Question à laquelle il répond | Petit exemple BI |
|---|---|---|
| **Instructions** | Quelles règles respecter? | Garder les valeurs manquantes. |
| **[Skill](https://code.visualstudio.com/docs/agent-customization/agent-skills)** | Quelle procédure réutiliser? | Tests → unités → rapport qualité. |
| **[Agent personnalisé](https://code.visualstudio.com/docs/agent-customization/custom-agents)** | Quel rôle et quels outils? | Réviseur BI en lecture seule. |
| **Tool · outil** | Quelle action exécuter? | Lire un CSV ou lancer pytest. |
| **[Serveur MCP](https://code.visualstudio.com/docs/agent-customization/mcp-servers)** | Comment connecter un système? | Fournir un outil de lecture SQL. |
| **Hook** | Que lancer à un événement? | Journaliser un appel d’outil. |
| **Plugin** | Comment distribuer un ensemble? | Installer une skill et un serveur MCP. |

</div>

<div class="small">L’instruction guide le modèle; le hook exécute du code à l’événement configuré.<br>Le serveur MCP expose des outils; le plugin peut les regrouper avec d’autres capacités.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/concepts/customization">Microsoft · comparer les concepts</a> · <a href="https://code.visualstudio.com/docs/agent-customization/agent-skills">Skills</a> · <a href="https://code.visualstudio.com/docs/agent-customization/hooks">Hooks</a> · <a href="https://code.visualstudio.com/docs/agent-customization/agent-plugins">Plugins</a></div>

<!--
2 minutes, 17:00–19:00. Suivre les questions du tableau, sans lire chaque définition comme une liste à mémoriser. Développer MCP : Model Context Protocol, un protocole de connexion entre l’agent et des services. Un outil peut être intégré au harnais ou fourni par un serveur MCP; MCP n’est pas le modèle. Skill = recette avec SKILL.md, scripts et ressources; notre validate-etl se lance volontairement par /validate-etl. Agent personnalisé = configuration du rôle; notre reviseur-bi limite les outils. Hook = code déclenché par un événement; ses noms, schémas et événements dépendent du harnais. L’exemple SQL, le hook de journalisation et le plugin sont des exemples conceptuels, pas des composants installés dans HydroDataLab. Un plugin distribue des capacités; il n’est pas un niveau de priorité des instructions. Les formats, déclenchements et liens propres à chaque type sont dans handouts/personnalisations-vscode.md. Démarrer avec instructions et tests; ajouter les autres mécanismes pour un besoin concret.
-->

---

# Context layers · les couches du contexte

<ContextLayers />

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Microsoft · activation des règles</a> · <a href="https://code.visualstudio.com/docs/agents/guides/context-engineering-guide">Structurer le contexte</a></div>

<!--
3 minutes, 19:00–22:00. Context layers signifie couches du contexte. Ce sont nos catégories pédagogiques, pas une hiérarchie officielle ni un ordre de priorité. Lire de haut en bas : Project instructions / Consignes du projet, File-specific instructions / Consignes ciblées, Reference documents / Documents de référence, Task prompt / Demande courante, Tool results / Résultats des outils. Les deux premières sont des consignes durables. Les documents et la demande cadrent le travail du moment. Les outils apportent ensuite les observations réelles. Les flèches alimentent un même Model context / Contexte du modèle; cliquer une couche illustre son apport, sans prétendre que le harnais charge automatiquement tous les fichiers. Montrer Reference documents et expliquer unité/fréquence/fuseau, puis Tool results et son exemple 23/24. Le mot « contrat » désigne ici un document décrivant les données; il n’est pas un niveau universel de contexte. Les autres sources (historique, instructions de service, préférences utilisateur) existent, mais ce dessin se concentre sur ce que le participant peut préparer dans son projet. Les règles applicables se combinent; résoudre les contradictions. Les détails applyTo restent à la slide 9. Le PDF garde toutes les couches, leurs traductions et leurs exemples de fichiers; la partie de droite montre la première couche.
-->

---

# Les tokens : lire et produire

<div class="lead">Un token est un morceau de texte :<br>un mot, une partie de mot ou un signe.</div>

<div class="columns"><div>

## Ce que le modèle reçoit

Votre demande et les consignes.<br>Les extraits de code ou de données.<br>Les résultats des outils.

</div><div>

## Ce que le modèle produit

Sa réponse et le code proposé.<br>Du raisonnement facturé<br>selon le modèle utilisé.

</div></div>

<div class="strip">Pour notre ETL : le schéma, un petit échantillon représentatif<br>et les tests donnent un contexte utile.</div>

<div class="source"><a href="https://code.visualstudio.com/docs/agents/concepts/context">Microsoft · contexte</a> · <a href="https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing">GitHub · facturation</a>. Évaluer toutes les tentatives nécessaires.</div>

<!--
2 minutes, 22:00–24:00. Ne pas assimiler un token à un mot : la découpe varie. Le modèle dispose d’une quantité limitée de contexte; la longueur de la demande n’est qu’une partie de ce qui est traité. Un petit prompt peut déclencher de nombreuses lectures. Exemple concret : donner le dictionnaire, quelques lignes avec zéro/absence/doublon, la fonction ciblée et ses tests. Demander un extrait de journal autour de l’erreur au lieu de toute la sortie. Une nouvelle tâche sans rapport mérite une nouvelle session. /compact résume une conversation mais peut perdre des détails. Le cache réutilise certains calculs d’entrée et peut changer leur prix; ce n’est pas une instruction permanente. Les reprises peuvent annuler le gain d’un modèle moins cher. Conserver décisions et état dans les fichiers du projet pour reprendre après interruption.
-->

---

# Auto : un bon point de départ

| Niveau | Priorité | Exemple BI |
|---|---|---|
| Efficiency | Coût | Expliquer une fonction courte |
| **Balance** | Coût, qualité et temps d’attente | Transformer une série avec tests |
| Intelligence | Qualité | Investiguer une anomalie difficile |

<div class="strip">Auto choisit selon la tâche et les modèles autorisés.</div>

<div class="small">Plans payants : remise Auto de 10 % sur le coût des modèles.<br>Le volume total dépend du travail effectué.</div>

<div class="source"><a href="https://docs.github.com/en/copilot/concepts/models/auto-model-selection">GitHub · Auto model selection</a>, consulté le 18 octobre 2026. Politiques et abonnement restent déterminants.</div>

<!--
2 minutes, 24:00–26:00. Balance est notre défaut pédagogique, pas une règle institutionnelle. Les niveaux modifient une préférence de routage, pas une liste exclusive de modèles. Auto considère complexité et disponibilité, dans les politiques applicables. La remise n’implique pas une économie pour toute tâche face à n’importe quel modèle manuel. On peut voir le modèle utilisé dans les informations de la réponse. 1 AI credit représente 0,01 dollar US. Les anciens abonnements annuels individuels peuvent avoir un régime historique distinct du cas entreprise.
-->

---

# Comparer 4 tâches et 5 modèles

<ModelChooser />

<div class="doc-link"><a href="https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing">GitHub · tarifs au 18 octobre 2026</a> · <a href="https://docs.github.com/en/copilot/reference/ai-models/model-comparison">Comparer les modèles</a></div>

<!--
2 minutes, 26:00–28:00. Les quatre types de tâches sont visibles simultanément en colonnes et les cinq modèles en lignes. Lire verticalement pour comparer les modèles d’une tâche, horizontalement pour comparer les quatre scénarios avec le même modèle. Chaque cellule comporte une barre et sa valeur en dollars US. Le plafond commun est 0,40 dollar par essai; les valeurs numériques conservent la lisibilité des petits montants. Les volumes fictifs entrée/sortie sont inscrits sous chaque tâche. Le losange et le fond vert indiquent un choix pédagogique à essayer, pas un score mesuré. Luna/Haiku pour une explication courte; Sonnet pour la modification bornée d’un ETL; Sol pour des changements complexes; Opus si le diagnostic reste difficile. Sonnet et Sol ont le même tarif, ce qui ne prouve pas une qualité identique. Multiplier les essais montre leur coût cumulé; leur volume réel peut varier. Cache et remise Auto sont exclus. Auto Balance reste le départ conseillé, sans lui inventer un prix fixe. Ces calculs ne sont pas des benchmarks ni une estimation mesurée du coût d’un projet. Confirmer les modèles disponibles dans le sélecteur HQ.
-->

---

# Auto choisit; HydraFusion orchestre

<div class="columns"><div>

## Auto

Un modèle choisi pour la requête.<br>Préférence Efficiency, Balance<br>ou Intelligence.

</div><div>

## HydraFusion · préversion

**Single** : un modèle.<br>**Cascade** : proposition → jugement.<br>**Critique** : proposition → revue → révision.

</div></div>

<div class="strip compact"><b>Coût HydraFusion = somme des modèles utilisés.</b><br>Pas de supplément propre à HydraFusion; pas de remise Auto.<br>Max augmente l’effort sur les tâches les plus exigeantes.</div>

<div class="doc-link"><a href="https://docs.github.com/en/early-access/copilot/hydrafusion">GitHub · fonctionnement et facturation HydraFusion</a><br><a href="https://github.blog/changelog/2026-10-14-hydrafusion-updates-and-new-max-option/">Annonce du 14 octobre</a> · <a href="https://docs.github.com/en/copilot/concepts/models/auto-model-selection">Documentation Auto</a></div>

<!--
1 minute, 28:00–29:00. HydraFusion n’est pas lui-même un modèle et ne lance pas systématiquement plusieurs modèles. Single peut suffire; Cascade juge et peut escalader; Critique ajoute revue puis révision. Le choix du parcours et de l’effort varie par prompt. Les passages supplémentaires peuvent augmenter durée et crédits. Les modèles doivent être disponibles dans le plan et autorisés par les politiques; l’utilisateur ne choisit pas individuellement le pool. Le système reste en research preview, sans promesse de production ni d’économie universelle. Capsule préparée, aucune activation requise pour la formation.
-->

---
class: breakwater
---

# Démo 2 · suivre le trajet des données

<div class="flow"><b>Données HQ</b><span>→</span><b>Calcul Python</b><span>→</span><b>Tableau de bord</b></div>

| Élément préparé | Son rôle |
|---|---|
| **API .NET** | Transmettre les commandes et les résultats à l’interface Vue. |
| **Aspire** | Démarrer les composants et regrouper leurs journaux. |
| **Image Docker** | Emballer le code et ses dépendances pour l’exécuter en conteneur. |

<div class="strip">D’où vient ce résultat? Et comment reprendre un traitement en erreur?</div>

<div class="source">Architecture HydroDataLab [S12]. Le calcul métier reste dans le même code Python.</div>

<!--
1 minute d’explication puis 5 minutes de démo, 29:00–35:00. Définir API comme le point d’accès du programme : l’interface envoie une demande et reçoit une réponse. Vue est la technologie de l’écran; .NET celle de l’API préparée. Aspire démarre les composants et permet de lire leurs journaux. Docker démarre un conteneur à partir d’une image déjà préparée; il ne transforme pas à lui seul un script en service déployé. L’audience n’a pas à reconstruire cette infrastructure.
Démo : exécuter la commande Python locale déjà répétée. CLI signifie interface en ligne de commande; dire d’abord « commande Python ». Source fixture = copie locale préparée des données publiques. Montrer les sorties : CSV (table texte), Parquet (format de table typée), JSON (rapport structuré). Depuis Vue, lancer le traitement et lire son état, sa provenance et ses résultats. Montrer une ressource et son journal dans Aspire. Expliquer seulement à ce moment : Quartz déclenche les tâches; SQLite est la base locale où l’API conserve l’historique. Aspire n’est pas le planificateur métier. Simuler un échec puis relancer. À 4:30, exécuter la commande Docker préremplie et déjà testée sans réseau; comparer son résumé au résultat natif. Les images ont été préparées avant la séance; ne pas lancer une construction en direct. La planification reste désactivée. Le guide conserve tous les raccourcis, commandes et seuils de secours.
-->

---
class: sdd
---

# SDD : une spécification avant le code

<div class="small"><b>Spec-Driven Development</b> · développement piloté par spécifications.<br>Le besoin devient des critères, puis un plan, du code et des preuves.</div>

<div class="small">Notre règle : la <strong>couverture</strong> mesure la part des créneaux attendus avec une mesure valide.</div>

| Exemple sur une série horaire | Critère de réussite |
|---|---|
| 24 mesures valides sur 24 attendues | 100 % |
| 23 mesures valides sur 24 attendues | 95,83 % |
| Valeur zéro, doublon ou absence | Zéro compte; doublon et absence n’ajoutent rien. |
| Changement d’heure | 23 ou 25 heures attendues selon la journée. |

<div class="strip compact">Ces exemples deviennent des tests. Le SDD sera approfondi<br>dans la prochaine formation animée par l’autre équipe.</div>

<div class="doc-link"><a href="https://github.github.com/spec-kit/">GitHub · Spec Kit et développement piloté par spécifications</a></div>

<!--
3 minutes, 35:00–38:00. Définir « spec » comme le raccourci de spécification, puis « critère d’acceptation » comme un exemple vérifiable qui décide si le résultat convient. Un test exécutable compare le résultat calculé au résultat attendu. Formule : mesures valides dans des créneaux distincts / créneaux attendus × 100. Ne pas utiliser le nombre de lignes reçues au dénominateur. Un zéro est une mesure; un doublon ne crée pas un créneau. Les bornes de la journée sont calculées à Toronto puis comparées comme instants UTC (référence de temps commune). La demande à 15 minutes attend 96 points, ou 92/100 lors des changements d’heure. Le survol du développement piloté par spécifications est la démarche besoin → critères relus → plan → tâches → code → preuves. Spec Kit aide à organiser ces documents; nous n’en faisons pas l’installation ici.
-->

---
class: breakwater
---

# Démo 3 · détecter une mesure absente

<div class="lead">24 mesures horaires attendues.<br>23 mesures numériques valides.</div>

<p class="coverage-example">23 ÷ 24 = <strong>95,83 %</strong></p>

<div class="strip">L’agent ajoute le calcul en Python.<br>Nous vérifions les tests et l’indicateur affiché.</div>

<div class="source">Scénario pédagogique : une mesure est retirée en mémoire. Les données brutes restent intactes.</div>

<!--
4 minutes de démo, 38:00–42:00. La diapositive annonce le résultat que l’audience doit pouvoir vérifier, pas le déroulé technique.
0:00–0:40 : montrer brièvement « Couverture : Non calculée » sur le parcours préparé au jalon 06, puis ouvrir la branche demo-live, la spécification, les tests et la fonction Python à compléter. Le tableau de bord sait déjà afficher la couverture; elle est absente tant que le calcul n’est pas implémenté.
0:40–2:10 : choisir le modèle validé en répétition, coller le prompt live et limiter la modification à Python. Attendre le résultat réel des tests.
À 2:10 : si la génération n’est pas prête, annoncer « Je passe à la solution préparée pour garder le temps de vérifier. » Préserver le travail et basculer vers le dossier de secours 07-solution selon le guide.
2:10–3:00 : lire la comparaison avant/après (diff) et un test. Expliquer diff au moment de l’afficher.
3:00–4:00 : revenir au même tableau de bord et déplier Qualité par série. Exécuter les cas complet et manquant : le niveau horaire passe de 24/24 = 100 % à 23/24 = 95,83 %. Marquer une pause sur ce changement : une règle en français est devenue du code testé, puis un résultat visible. Déplier la qualité par série : la couverture globale 167/168 = 99,4 % utilise un autre dénominateur. Ne pas présenter la solution de secours comme une génération en direct.
-->

---

# Votre prochaine tâche Python

<div class="lead">Écrivez un résultat attendu, confiez une petite tâche,<br>puis examinez les preuves.</div>

<div class="columns compact"><div>

## Apprendre et échanger

[Microsoft Learn : agents](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/)<br>[Pratiques VS Code](https://code.visualstudio.com/docs/agents/best-practices)<br>[Awesome Copilot](https://awesome-copilot.github.com/)<br>[GitHub Community](https://github.com/community/community/discussions/categories/copilot?locale=en-US)

</div><div>

## À découvrir : Canvas

Une surface interactive partagée<br>avec l’agent, à côté du chat.<br>Exemple : suivre les étapes de l’ETL<br>dans un tableau de tâches.

[Microsoft · Canvas en préversion](https://code.visualstudio.com/docs/agents/run/agent-harnesses#use-canvases-with-the-copilot-harness)<br><a href="/22">Voir le fonctionnement →</a>

</div></div>

<div class="strip compact">Quelle règle de vos analyses aimeriez-vous rendre vérifiable?</div>

<!--
3 minutes, 42:00–45:00, dont 30 secondes pour Canvas. Reprendre la règle mesurable, le contexte sélectionné et les preuves. Canvas est une surface interactive partagée avec l’agent, fournie par une extension; par exemple un tableau de tâches. Le présenter comme une nouveauté 1.142 en preview, sans en faire une dépendance du parcours ETL. L’annexe 22 détaille activation et limites; ne pas l’ouvrir systématiquement pendant les 45 minutes. Le composant interactif de nos slides et l’application Vue ne sont pas des Canvas VS Code. Proposer le guide participant. Learn structure l’apprentissage; Awesome Copilot fournit des exemples à relire; Community permet les échanges. Le SDD sera approfondi par l’autre équipe. Questions 45:00–55:00, marge jusqu’à 60:00.
-->

---
class: annex
---

# Annexe · du fichier au panneau Instructions

<div class="screenshot-window"><img src="/vscode-customizations-user.png" alt="Capture fournie : Agent Customizations pour Copilot, Instructions sélectionné, sections User et Workspace vides." /></div>

<div class="scope-map">
<div><b>User · vos préférences</b><code>~/.copilot/copilot-instructions.md</code><span>Commun à vos projets, sur ce poste.</span></div>
<div><b>Workspace · ce projet</b><code>.github/copilot-instructions.md</code><span>Partagé dans Git avec l’équipe.</span></div>
</div>

<div class="small">Règles ciblées : <code>~/.copilot/instructions/</code> ou <code>.github/instructions/</code>.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/overview#agent-customizations-editor">Microsoft · panneau Customizations</a> · <a href="/8">Retour à l’exemple →</a></div>
<div class="source">Capture réelle fournie par le présentateur, autre projet, listes vides. Les chemins ci-dessus expliquent les portées; ils ne sont pas des entrées visibles dans cette capture.</div>

<!--
Annexe. Le harnais Copilot doit être sélectionné avant d’ouvrir Chat: Open Customizations ou Customizations dans la barre latérale. La capture est conservée sans modification de pixels; le site cadre la partie utile par CSS. Elle vient du projet RealEstateFinancing et ne prouve pas la découverte des fichiers HydroDataLab. User 0 et Workspace 0 sont effectivement visibles. Dans HydroDataLab, ouvrir la racine du dépôt, Instructions, puis l’élément découvert et son fichier. Les réglages User de VS Code et les fichiers du harnais sous ~/.copilot ne sont pas le même stockage. ~ désigne le dossier personnel de la machine où l’agent tourne. Les anciens fichiers de profil et chat.*FilesLocations peuvent nécessiter une migration. Vérifier References, puis un test de comportement; présence ne signifie pas conformité. Les réglages de lisibilité, leur portée et le diagnostic sont détaillés dans handouts/personnalisations-vscode.md.
-->

---
class: annex
---

# Annexe · modèles et tarifs Copilot

| Modèle | Entrée | Cache lu | Cache écrit | Sortie |
|---|---:|---:|---:|---:|
| GPT-6 Luna | 0,10 | 0,01 | 0,125 | 0,50 |
| Claude Haiku 5.5 | 0,10 | 0,01 | 0,125 | 0,50 |
| GPT-6.1 Sol | 2,00 | 0,10 | 2,50 | 10,00 |
| Claude Sonnet 5.5 | 2,00 | 0,10 | 2,50 | 10,00 |
| Claude Opus 5.5 | 4,00 | 0,20 | 5,00 | 20,00 |

<div class="small">$ US / million de tokens, tarifs standards. 1 AI credit = 0,01 $ US.<br>Long contexte : Haiku > 100k; Luna et Sol > 272k → grille distincte.</div>

<div class="source"><a href="https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing">GitHub · grille officielle des tarifs</a>, 18 octobre 2026. Disponibilité HQ à confirmer.</div>

<!--
Annexe. Tarifs GitHub Copilot, pas extrapolation d’une API fournisseur. Lecture et écriture du cache sont des catégories distinctes; ne pas les additionner arbitrairement sur tous les tokens. Les seuils modifient la grille. Voir matrice-modeles.md pour les paliers longs et la simulation. Les coûts d’infrastructure de revue cloud ne sont pas modélisés. Les rôles suggérés se mesurent avec exactitude, reprises, temps et crédits. Le coût réel dépend de l’usage et de son contrat de facturation.
-->

---
class: annex
---

# Annexe · Canvas, travailler dans le même objet

<div class="canvas-flow"><div><b>Vous</b><span>Déplacez une carte</span></div><strong>↔</strong><div><b>Canvas partagé</b><span>Tableau des étapes ETL</span></div><strong>↔</strong><div><b>Agent</b><span>Met à jour les tâches</span></div></div>

<div class="small">Schéma pédagogique : une extension fournit l’interface et ses actions.<br>Ce dessin et le tableau de bord Vue ne sont pas des Canvas VS Code.</div>

<div class="columns compact"><div>

## Ouvrir une surface

Activer <code>chat.canvases.enabled</code>.<br>Créer une nouvelle session Copilot.<br><code>/create-canvas</code> si disponible;<br>puis <b>+ → Canvas</b> pour un canvas installé.

</div><div>

## Cadre actuel · Preview

Application VS Code de bureau.<br>Session locale Copilot / Agent Host.<br>Pas le harnais « Local », ni Claude,<br>Codex ou une session distante.

</div></div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/run/agent-harnesses#use-canvases-with-the-copilot-harness">Microsoft · prérequis et fonctionnement</a> · <a href="https://code.visualstudio.com/updates/v1_142#canvases-preview">Nouveauté 1.142</a> · <a href="/19">Retour →</a></div>

<!--
Annexe. Disponible en preview sur desktop pour les sessions locales du harnais Copilot dans Agent Host. Chaque Canvas provient d’une extension. /create-canvas crée une extension dans le workspace quand la skill est disponible. Autre chemin documenté : installer un chat plugin contenant une extension Canvas via l’application GitHub Copilot, puis demander à VS Code les Canvas disponibles. Add Tab (+) > Canvas ouvre une extension déjà disponible; ce menu seul ne crée pas une capacité. Le canvas reste associé à son chat; selon l’extension, les actions utilisateur et agent font évoluer le même état. Exemple de prompt de préparation : « Crée un canvas Kanban pour nos étapes extraire, valider, transformer et exporter. Affiche les critères de réussite de chaque carte. » Faire cela avant la séance si souhaité et autorisé, sans promettre que notre Slidev est relié à Copilot. Aucun Canvas n’a été créé ou installé dans le cadre de cette révision.
-->

---
class: annex
---

# Annexe · le contrat d’une observation

| Dimension | Question à résoudre |
|---|---|
| Schéma et identité | Quelle série, station et instant unique? |
| Unité et sens | MW, m ou °C? Mesure, minimum, maximum? |
| Fréquence | 15 minutes, horaire ou quotidien? |
| Temps | Instant UTC; journée America/Toronto? |
| Provenance | URL, licence, récupération et version brute? |
| Qualité | Null, doublon, invalidité et couverture attendue? |

<div class="source">Contrat HydroDataLab [S12] et métadonnées publiques HQ [S11]. Préserver le sens de la source.</div>

<!--
Annexe. L’instant UTC ne définit pas à lui seul la journée métier. Début inclusif, fin exclusive. Construire les bornes locales puis comparer en UTC pour les changements d’heure. Zéro et absence diffèrent; un doublon n’améliore pas la couverture. Si le schéma source change, signaler l’échec plutôt que fabriquer un indicateur. Min/max gardent leur agrégation propre; ne pas leur attribuer l’attendu horaire. Cette fiche aide à décider ce qui doit entrer dans la spec avant de demander le code.
-->

---
class: annex
---

# Annexe · reprendre sans improviser

<div class="flow compact"><b>00 socle</b><span>→</span><b>01 instructions</b><span>→</span><b>02 extraction</b><span>→</span><b>03 BI</b><span>→</span><b>04 pilotage</b><span>→</span><b>05 Docker</b><span>→</span><b>06 spec</b><span>→</span><b>07 solution</b></div>

- Un tag Git est un repère nommé sur une version enregistrée.
- Conserver ses modifications avant de changer de version.
- Séparer les données d’exécution des fichiers de code.
- Reprendre avec la copie locale des données et les tests.

<div class="strip">PDF, solution préparée et Git pour reprendre.</div>

<div class="source">Guide présentateur et procédure de reprise du projet. Les checkpoints VS Code n’annulent pas les effets externes des outils [S06].</div>

<!--
Annexe. Carte du parcours Git, pas invitation à utiliser reset --hard. Les identifiants et commandes sont dans le runbook validé. Préserver la mini-démo en cours. Séparer runtime et source pour éviter une base SQLite d’une autre étape. Après crash, consulter PROGRESS.md, Git et le dernier contrôle réussi. Ne pas présumer que le dernier message correspond à une commande entièrement exécutée. Les données fixture rendent la reprise indépendante du réseau et évitent une source mouvante pendant la prestation.
-->

---
class: annex
---

# Annexe · sources et limites

<div class="columns compact"><div>

## VS Code et GitHub

[Personnalisations](https://code.visualstudio.com/docs/agent-customization/overview)<br>[Contexte](https://code.visualstudio.com/docs/agents/concepts/context)<br>[Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection)<br>[Tarifs Copilot](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing)<br>[HydraFusion, 14 octobre](https://github.blog/changelog/2026-10-14-hydrafusion-updates-and-new-max-option/)

</div><div>

## Données et pratique

[Données ouvertes HQ](https://www.hydroquebec.com/documents-donnees/donnees-ouvertes/)<br>[Microsoft Learn](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/)<br>[Awesome Copilot](https://awesome-copilot.github.com/)<br>[Spec Kit](https://github.github.com/spec-kit/)

</div></div>

<div class="strip compact">Sources consultées le 18 octobre 2026. Les fonctions disponibles dépendent de la version et du compte.</div>

<div class="source">Registre complet : docs/SOURCES.md. Schémas pédagogiques; aucune capture VS Code inventée.</div>

<!--
Annexe de références. Les fiches locales reprennent les liens essentiels. Les fonctionnalités sont marquées expérimentales ou preview selon leur source. La date de vérification n’est pas une garantie sur toute version installée. Confirmer politiques HQ et modèles sur le portable. Les documents techniques du dépôt définissent les commandes finales. Aucun lien externe n’est requis pour afficher le support après build : polices et composants sont locaux. Le PDF fige les vues interactives; utiliser le web pour explorer.
-->


