---
theme: default
favicon: /favicon.png
title: GitHub Copilot dans VS Code — bonnes pratiques avec les agents
author: Formation Hydro-Québec
info: Support du 20 octobre 2026. Sources vérifiées les 18 et 19 octobre 2026.
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

# GitHub Copilot<br>dans VS Code

Bonnes pratiques avec les agents<br>Fil conducteur : HydroDataLab

<div class="source">20 octobre 2026 • Sources vérifiées les 18–19 octobre • Démonstration pédagogique, données publiques</div>

<!--
30 secondes, 00:00–00:30. « Comment déléguer à Copilot tout en gardant la maîtrise de son travail? » Le sujet est GitHub Copilot dans Visual Studio Code : contexte, instructions, skills, outils, permissions et choix du modèle. HydroDataLab fournit des exemples proches du travail BI/Python du groupe; ce n’est pas un cours de construction d’ETL ou d’Aspire. La petite modification Python sert à observer la démarche et ses preuves. Ce support n’est pas une publication officielle d’Hydro-Québec; les exemples utilisent des données publiques.
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

# Bien déléguer avec Copilot

<div class="lead">Une mission précise, un contexte utile,<br>un résultat que vous pouvez vérifier.</div>

<div class="columns"><div>

## Ce que vous saurez faire

Cadrer une session dans VS Code.<br>Choisir les règles et outils utiles.<br>Relire les changements et les tests.

</div><div>

## Notre fil conducteur

Données publiques Hydro-Québec.<br>Une petite modification Python.<br>Un indicateur de qualité vérifiable.

</div></div>

<div class="strip">45 minutes de présentation et de démos, puis vos questions.</div>

<!--
30 secondes, 01:30–02:00. Annoncer le parcours : utiliser Agents, fournir des règles et du contexte, choisir ses capacités et son modèle, puis vérifier un changement. Les démos illustrent ces pratiques dans HydroDataLab. Aucune maîtrise de Git, Docker, .NET ou Vue n’est supposée; expliquer les mots avant de les utiliser. 32 minutes d’explications et 13 minutes de démos, puis 10 minutes de questions et 5 de marge. Les annexes 23 à 28 servent aux questions. Transition : « Voici les données qui nous serviront à vérifier ce que produit l’agent. »
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
| **Branche** | Une suite de versions pour un essai.<br>Exemple : <code>codex/repetition-01</code>. |
| **Worktree** · facultatif | Un autre dossier du même dépôt, pour travailler sur une autre branche. |

<div class="strip">Pour débuter, une branche d’essai suffit.</div>

<div class="source"><a href="https://git-scm.com/docs/git-worktree">Documentation Git · worktree</a> [S22]. Les fichiers et résultats non enregistrés demandent leur propre sauvegarde.</div>

<!--
2 minutes, 06:00–08:00. Partir du besoin « essayer un changement sans mélanger l’essai avec la version de référence ». Dépôt : dossier du projet avec son historique Git. Commit : version enregistrée, pas simple sauvegarde dans l’éditeur. Branche : chemin d’évolution nommé; main reste la référence, codex/repetition-01 accueille l’exercice. Un worktree ouvre une autre branche dans un autre dossier en partageant l’historique; il reste facultatif ici, et ne constitue ni un conteneur ni une isolation des services externes. Notre répétition utilise une copie Git indépendante, préparée depuis un bundle : elle contient la branche issue de codex/preparation-demo-06, avec les améliorations actuelles et la couverture à compléter. Aucun changement de branche n’est nécessaire sur le dépôt principal pendant la séance. Faire reformuler : « Dans quel dossier et sur quelle branche l’agent va-t-il agir? » Le conducteur décrit la préparation et le retour à main en secours.
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
1 minute d’explication puis 4 minutes de démo, 08:00–13:00. Présenter la mission en lecture seule et la question « Quelles sources permettent de vérifier sa réponse? ». Le dépôt principal reste sur main. Commandes et prompt complets : handouts/conducteur-demos.html, Démo 1.
0:00–0:45 : ouvrir Chat: Open Agents Window, vérifier dossier, branche main, harnais Copilot et Auto Balance ou le modèle autorisé préparé. Créer une session et garder les confirmations d’actions. Montrer /plan comme possibilité pour une future modification, sans lancer une deuxième demande ici. Dans cette fenêtre, ne pas chercher l’agent Plan du menu de la vue Chat.
0:45–1:30 : Ctrl+, montrer Utilisateur/Espace de travail et la lisibilité préparée. Repérer Changes; il peut rester vide pendant cette mission. Le vrai diff sera montré à la démo 3.
1:30–2:20 : ouvrir Customizations. Pendant 10 secondes, situer Overview (vue d’ensemble) et Customize Your Agent (aide à créer des personnalisations à relire), sans envoyer de demande. Ouvrir ensuite Instructions et les consignes communes et Python; retrouver zéro/absence. Les sept rubriques seront expliquées en slide 11. Si un fichier n’est pas listé, ne pas conclure qu’il est chargé simplement parce qu’il existe.
2:20–4:00 : coller le prompt de découverte du conducteur. Lire References comme sources consultées; ouvrir une fonction ou un test cité et vérifier une affirmation. La présence d’une règle ne garantit pas son respect.
Si Copilot tarde ou est indisponible, ouvrir pipeline.py et un test local, en annonçant que la génération n’a pas abouti en direct. Revenir aux slides à 4:00.
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

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Microsoft · instructions personnalisées</a> · <a href="/23">Se repérer dans Customizations →</a></div>

<!--
2 minutes, 13:00–15:00. Une instruction est une consigne durable, enregistrée dans le projet et relue comme le code. Montrer trois températures fictives : zéro Celsius est bien une mesure, None signifie qu’on n’en a pas reçu. En pandas, l’absence peut apparaître sous forme NaN ou NA; en JSON, null. La règle ne prétend pas qu’un zéro est physiquement plausible pour toute série : les contrôles métier restent nécessaires. Ici, ne pas convertir un manque en zéro pour remplir un graphique. Si on le faisait, la couverture deviendrait artificiellement 100 %. Le test doit compter deux valeurs, et non une (zéro exclu) ou trois (absence remplacée). Montrer le fichier du dépôt. Le lien interne conduit à la capture réelle en annexe 23; l’ouvrir pendant la démo seulement si nécessaire. Les notes et le guide fournissent la marche à suivre.
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

# Context layers · les couches du contexte

<ContextLayers />

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Microsoft · activation des règles</a> · <a href="https://code.visualstudio.com/docs/agents/guides/context-engineering-guide">Structurer le contexte</a></div>

<!--
2 minutes, 17:00–19:00. Après les fichiers d’instructions, montrer comment leurs consignes rejoignent les autres éléments utiles à une demande. Context layers signifie couches du contexte. Ce sont nos catégories pédagogiques, pas une hiérarchie officielle ni un ordre de priorité. Lire de haut en bas : Project instructions / Consignes du projet, File-specific instructions / Consignes ciblées, Reference documents / Documents de référence, Task prompt / Demande courante, Tool results / Résultats des outils. Les deux premières sont des consignes durables. Les documents et la demande cadrent le travail du moment. Les outils apportent ensuite les observations réelles. Les flèches alimentent un même Model context / Contexte du modèle; cliquer une couche illustre son apport, sans prétendre que le harnais charge automatiquement tous les fichiers. Montrer Reference documents et expliquer unité/fréquence/fuseau, puis Tool results et son exemple 23/24. Le mot « contrat » désigne ici un document décrivant les données; il n’est pas un niveau universel de contexte. Les autres sources (historique, instructions de service, préférences utilisateur) existent, mais ce dessin se concentre sur ce que le participant peut préparer dans son projet. Les règles applicables se combinent; résoudre les contradictions. Les détails applyTo restent à la slide 9. Le PDF garde toutes les couches, leurs traductions et leurs exemples de fichiers; la partie de droite montre la première couche.
-->

---

# Le panneau Agent Customizations

<div class="customization-intro"><div><b>Overview · vue d’ensemble</b> : retrouver les personnalisations.</div><div><b>Customize Your Agent</b> : décrire ses préférences → créer des fichiers à relire.</div></div>

<div class="concept-table">

| Dans VS Code | Ce que c’est · à quoi cela sert | Exemple |
|---|---|---|
| **[Plugins](https://code.visualstudio.com/docs/agent-customization/agent-plugins)** | Des paquets pour distribuer des capacités. | Installer skill + serveur MCP. |
| **[MCP Servers](https://code.visualstudio.com/docs/agent-customization/mcp-servers)** | Des serveurs pour connecter outils et données. | Exposer une lecture SQL. |
| **[Skills](https://code.visualstudio.com/docs/agent-customization/agent-skills)** | Des procédures réutilisables à charger au besoin. | <code>/validate-etl</code> : tests et bilan. |
| **[Instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions)** | Des consignes pour guider le travail. | Préserver les valeurs absentes. |
| **[Agents](https://code.visualstudio.com/docs/agent-customization/custom-agents)** | Des rôles avec leurs consignes et outils. | Réviseur BI en lecture seule. |
| **[Hooks](https://code.visualstudio.com/docs/agent-customization/hooks)** | Des commandes déclenchées par un événement. | Journaliser un appel d’outil. |
| **[Tools](https://code.visualstudio.com/docs/agents/run/tools)** | Des outils pour agir : lire, chercher, exécuter… | Le terminal lance pytest. |

</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/concepts/customization">Microsoft · comparer les concepts</a> · <a href="https://code.visualstudio.com/docs/agent-customization/overview">Le panneau</a> · <a href="/23">Voir la capture →</a></div>

<!--
2 minutes, 19:00–21:00. Les libellés et leur ordre correspondent à la capture du présentateur. Ce menu classe des capacités; ce n’est pas une hiérarchie de priorité.
0:00–0:20 : Overview est la vue d’ensemble. Customize Your Agent permet de décrire ses préférences pour préparer des agents, skills ou instructions : examiner la proposition, sa portée et ses fichiers avant de les enregistrer. Exemple de demande à montrer oralement, sans l’exécuter : « Prépare une instruction de projet demandant des tests pour toute transformation Python. » Explore Customizations reprend les mêmes rubriques sous forme de cartes; ce ne sont pas des mécanismes supplémentaires.
0:20–1:35 : parcourir les sept lignes, dans l’ordre. Plugins distribue un ensemble de capacités. MCP signifie Model Context Protocol : un serveur connecte un système et peut exposer une lecture SQL. Skills apporte une procédure réutilisable quand elle est pertinente ou invoquée; notre /validate-etl est volontaire. Instructions apporte des règles selon leur portée et leurs conditions d’activation. Agents définit un rôle et ses outils, pas un nouveau modèle. Hooks exécute une commande à un événement configuré, indépendamment du choix du modèle; vérifier sa configuration et ses permissions. Tools montre les actions accessibles à l’agent et permet de gérer les groupes configurables; un outil peut être natif, fourni par une extension ou exposé via MCP. Les exemples SQL/hook/plugin sont illustratifs; les compteurs de la capture ne prouvent pas que ces exemples précis sont installés ou utilisés.
1:35–2:00 : relier trois lignes : l’instruction demande de préserver les absences, le skill décrit les vérifications, l’outil terminal exécute pytest. Le résultat du test reste la preuve. La capture en annexe 23 permet de retrouver les rubriques; l’ouvrir seulement au besoin. En-tête = harnais et projet; nombres = compteurs du panneau, pas coût, qualité ni preuve d’activation dans la réponse. Les formats et déclenchements sont dans handouts/personnalisations-vscode.md. Transition : « Nous pouvons personnaliser beaucoup de choses; voici pourquoi je garde une base simple. »
-->

---
class: vanilla
---

# Une base « vanille », des ajouts ciblés

<p class="vanilla-position">Mon point de départ : garder une configuration simple à faire évoluer.</p>

<div class="vanilla-path">
<div><b>Fonctions natives</b><span>Copilot, terminal, Git et tests.<br>Des consignes de projet courtes.</span></div>
<span class="path-arrow">→</span>
<div><b>Besoin récurrent</b><span>Ajouter un skill ou un outil<br>pour une tâche précise.</span></div>
<span class="path-arrow">→</span>
<div><b>Ajout entretenu</b><span>Relire, versionner et tester.<br>Retirer ce qui ne sert plus.</span></div>
</div>

<div class="vanilla-reasoning"><div><h2>Pourquoi rester simple?</h2><p>Moins d’intégrations à adapter et de règles qui se contredisent.</p></div><div><h2>Quand personnaliser?</h2><p>Quand le gain est vérifiable : moins d’erreurs, de répétitions ou de reprises.</p></div></div>

<div class="vanilla-takeaway">Un skill ciblé peut aider. Accumuler les plugins n’est pas une stratégie de qualité.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/migrate-customizations">Microsoft · migrations</a> · <a href="https://code.visualstudio.com/docs/agent-customization/agent-skills">Skills et chargement progressif</a></div>

<!--
1 minute, 21:00–22:00. « Vanille » signifie ici utiliser surtout les capacités natives, avec quelques consignes lisibles. C’est la préférence du présentateur et notre recommandation pour commencer; aucune politique HQ ni mesure universelle de productivité n’est affirmée. L’argument est le coût d’entretien : chaque intégration ajoute des versions, permissions et comportements à comprendre. Exemple concret documenté : la migration vers le harnais Copilot peut imposer de déplacer ou convertir des personnalisations que son moteur ne lit pas sous leur ancien format. La configuration native évolue elle aussi : rester simple ne dispense pas de lire les nouveautés. Nuance : les skills chargent leur contenu progressivement et un bon skill évite de répéter une procédure. Leur nombre seul ne prouve donc ni une surcharge de tokens ni un ralentissement. Préférer un format standard, une portée limitée et une version connue; comparer sur une tâche réelle avant/après. Un plugin maintenu peut faciliter la distribution. Sources S32 et S33, consultées le 19 octobre 2026.
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
2 minutes, 22:00–24:00. Les règles, documents et résultats que nous venons de voir occupent du contexte; examinons leur coût. Ne pas assimiler un token à un mot : la découpe varie. Le modèle dispose d’une quantité limitée de contexte; la longueur de la demande n’est qu’une partie de ce qui est traité. Un petit prompt peut déclencher de nombreuses lectures. Exemple concret : donner le dictionnaire, quelques lignes avec zéro/absence/doublon, la fonction ciblée et ses tests. Demander un extrait de journal autour de l’erreur au lieu de toute la sortie. Une nouvelle tâche sans rapport mérite une nouvelle session. /compact résume une conversation mais peut perdre des détails. Le cache réutilise certains calculs d’entrée et peut changer leur prix; ce n’est pas une instruction permanente. Les reprises peuvent annuler le gain d’un modèle moins cher. Conserver décisions et état dans les fichiers du projet pour reprendre après interruption.
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
1 min 30, 24:00–25:30. Balance est notre défaut pédagogique, pas une règle institutionnelle. Les niveaux modifient une préférence de routage, pas une liste exclusive de modèles. Auto considère complexité et disponibilité, dans les politiques applicables. La remise n’implique pas une économie pour toute tâche face à n’importe quel modèle manuel. On peut voir le modèle utilisé dans les informations de la réponse. 1 AI credit représente 0,01 dollar US. Les anciens abonnements annuels individuels peuvent avoir un régime historique distinct du cas entreprise.
-->

---

# Comparer 4 tâches et 5 modèles

<ModelChooser />

<div class="doc-link"><a href="https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing">GitHub · tarifs au 18 octobre 2026</a> · <a href="https://docs.github.com/en/copilot/reference/ai-models/model-comparison">Comparer les modèles</a></div>

<!--
2 minutes, 25:30–27:30. Les quatre types de tâches sont visibles simultanément en colonnes et les cinq modèles en lignes. Lire verticalement pour comparer les modèles d’une tâche, horizontalement pour comparer les quatre scénarios avec le même modèle. Chaque cellule comporte une barre et sa valeur en dollars US. Le plafond commun est 0,40 dollar par essai; les valeurs numériques conservent la lisibilité des petits montants. Les volumes fictifs entrée/sortie sont inscrits sous chaque tâche. Le losange et le fond vert indiquent un choix pédagogique à essayer, pas un score mesuré. Luna/Haiku pour une explication courte; Sonnet pour la modification bornée d’un ETL; Sol pour des changements complexes; Opus si le diagnostic reste difficile. Sonnet et Sol ont le même tarif, ce qui ne prouve pas une qualité identique. Multiplier les essais montre leur coût cumulé; leur volume réel peut varier. Cache et remise Auto sont exclus. Auto Balance reste le départ conseillé, sans lui inventer un prix fixe. Ces calculs ne sont pas des benchmarks ni une estimation mesurée du coût d’un projet. Confirmer les modèles disponibles dans le sélecteur HQ.
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
1 minute, 27:30–28:30. HydraFusion n’est pas lui-même un modèle et ne lance pas systématiquement plusieurs modèles. Single peut suffire; Cascade juge et peut escalader; Critique ajoute revue puis révision. Le choix du parcours et de l’effort varie par prompt. Les passages supplémentaires peuvent augmenter durée et crédits. Les modèles doivent être disponibles dans le plan et autorisés par les politiques; l’utilisateur ne choisit pas individuellement le pool. Le système reste en research preview, sans promesse de production ni d’économie universelle. Capsule préparée, aucune activation requise pour la formation.
-->

---
class: breakwater
---

# Démo 2 · vérifier un résultat

<div class="flow"><b>Données HQ</b><span>→</span><b>Calcul Python</b><span>→</span><b>Tableau de bord</b></div>

| Élément préparé | Son rôle |
|---|---|
| **API .NET** | Transmettre les commandes et les résultats à l’interface Vue. |
| **Aspire** | Démarrer les composants et regrouper leurs journaux. |
| **Image Docker** | Emballer le code et ses dépendances pour l’exécuter en conteneur. |

<div class="strip">Quelles preuves faut-il avant d’accepter le résultat de l’agent?</div>

<div class="source">Architecture HydroDataLab [S12]. Le calcul métier reste dans le même code Python.</div>

<!--
1 minute d’explication puis 5 minutes de démo, 28:30–34:30. Relier à Copilot : une réponse convaincante doit devenir un résultat exécuté, traçable et vérifiable. API = point d’accès entre programmes; Vue présente, .NET pilote, Python calcule. Aspire démarre les composants et regroupe les journaux. L’infrastructure est préparée. Commandes et variantes : handouts/conducteur-demos.html, Démo 2. Conserver main.
0:00–0:45 : exécuter le bloc CLI préparé du conducteur (uv run --project etl python -m hydrodatalab run --source fixture --date 2026-10-17 --output outputs/cli). Montrer 168 observations, quatre séries et hourly.csv; la copie locale des sources rend l’essai reproductible.
0:45–2:15 : Vue → Parcours ETL → Extraire, Transformer, Charger / exporter. Utiliser la collecte réussie préparée. Passer des champs fournisseur au schéma normalisé puis aux 24 moyennes horaires. Lire un seul indicateur : pointe horaire moyenne 22 009,75 MW à 18 h Toronto. Les autres restent disponibles aux questions. Ces onglets explorent un rapport déjà calculé; ils n’exécutent pas le pipeline au clic.
2:15–3:15 : dans VS Code, ouvrir analysis.py et test_analysis.py; comparer la règle quatre quarts d’heure valides avec un test. Ne pas parcourir tout le code. Faire le lien avec la démo 1 : la réponse de Copilot peut être confrontée à un fichier et à un résultat réel.
3:15–4:15 : Voir les détails d’un succès : paramètres, étapes Python, durées et export lié à cette exécution. Quartz déclenche; SQLite conserve le bilan; les étapes ne sont pas une animation en temps réel. Si le temps le permet dans ce créneau, montrer un seul journal Aspire correspondant.
4:15–5:00 : commande Docker préremplie avec --pull never --network none; comparer le résumé. Image et dépendances préparées, aucune construction en direct. Si le moteur est indisponible, montrer le résultat préparé en l’annonçant puis revenir aux slides à 5:00.
Variante choisie avant la séance : remplacer le créneau 3:15–5:00 par échec contrôlé, détails et relance; dans ce cas, ne pas ajouter Aspire ni Docker. Ne pas cumuler les deux parcours. La planification reste désactivée. La vérification d’un résultat est le message central, pas une visite exhaustive de l’application.
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
2 minutes, 34:30–36:30. Garder cette slide projetée pendant la transition préparée : depuis $Depot, ./scripts/stop-demo.ps1; depuis $Live, vérifier git status --short --branch puis ./scripts/start-demo.ps1 -NoBuild. Recharger Vue et lancer une collecte complète; vérifier Couverture : Non calculée. La copie utilise codex/repetition-01 issue de codex/preparation-demo-06 (9e1ecde), pas un ancien tag seul. Les slides restent servies depuis main. Si le démarrage ne tient pas dans les deux minutes répétées, utiliser le secours annoncé; ne pas improviser des installations.
 Définir « spec » comme le raccourci de spécification, puis « critère d’acceptation » comme un exemple vérifiable qui décide si le résultat convient. Un test exécutable compare le résultat calculé au résultat attendu. Formule : mesures valides dans des créneaux distincts / créneaux attendus × 100. Ne pas utiliser le nombre de lignes reçues au dénominateur. Un zéro est une mesure; un doublon ne crée pas un créneau. Les bornes de la journée sont calculées à Toronto puis comparées comme instants UTC (référence de temps commune). La demande à 15 minutes attend 96 points, ou 92/100 lors des changements d’heure. Le survol du développement piloté par spécifications est la démarche besoin → critères relus → plan → tâches → code → preuves. Spec Kit aide à organiser ces documents; nous n’en faisons pas l’installation ici.
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
4 minutes de démo, 36:30–40:30. Le même geste Copilot devient visible : demande délimitée, contexte, autorisations, changement, test. La transition depuis main vers la copie d’exercice a eu lieu pendant la slide 18. Commandes et prompt complets : handouts/conducteur-demos.html, Démo 3.
0:00–0:40 : montrer Couverture : Non calculée, puis specs/coverage.md, coverage.py et specs/test_coverage_acceptance.py dans $Live, branche codex/repetition-01. Elle part de codex/preparation-demo-06 (9e1ecde) et garde la couverture à implémenter. Les deux tests d’acceptation rouges ont été vérifiés avant la séance.
0:40–2:10 : nouvelle session Copilot dans cette copie, modèle autorisé préparé; coller le prompt du conducteur. Limiter le changement à la fonction Python et demander les résultats réels des tests; ne pas recréer l’API ou Vue ni assouplir les critères.
À 2:10 : si code et tests ne sont pas prêts, annoncer « Je passe à la solution préparée pour garder le temps de vérifier. » Arrêter la génération et les services de $Live, préserver ses fichiers, puis démarrer $Depot/main avec -NoBuild. Ne pas basculer le dépôt principal vers un ancien tag.
2:10–3:00 : lire Changes et les tests réels. Commande depuis la racine active : uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live. La solution préparée passe 43 tests, dont les jours de 23/25 heures. En secours, utiliser le diff historique limité à coverage.py, documenté dans le conducteur, et annoncer sa provenance.
3:00–4:00 : lancer une nouvelle collecte complète puis une collecte avec Retirer une mesure de niveau d’eau. Déplier Qualité par série : 24/24 = 100 %, puis 23/24 = 95,83 %. Le 99,4 % global utilise un autre dénominateur. Le retrait reste en mémoire. Si les services tardent, montrer la preuve préparée en le disant; ne pas dépasser 4:00. Revenir à la slide 20, sélection de skills.
-->

---
class: skill-selection
---

# Des skills à choisir selon votre tâche

<p class="selection-intro">Des pistes à évaluer dans Copilot, selon la tâche à accomplir.</p>

<div class="skill-table">

| Besoin | Point de départ | Utilité concrète |
|---|---|---|
| ETL Python | `validate-etl` · ce dépôt | Tests, unités et exports |
| Interface Vue | [Playwright CLI · Microsoft](https://github.com/microsoft/playwright-cli) | Parcours navigateur et captures |
| Application Aspire | [Skills Aspire · officiels](https://aspire.dev/get-started/ai-coding-agents/) | Démarrage, santé et journaux |
| API .NET | [dotnet/skills · équipe .NET](https://github.com/dotnet/skills) | ASP.NET Core et tests ciblés |
| Cadrage et tests | [Matt Pocock · grill-me, tdd](https://github.com/mattpocock/skills) | Clarifier le besoin; tester par étapes |

</div>

<div class="exploration-note"><b>Matt Pocock : en exploration.</b> Pas encore validé dans notre démo.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/agent-skills#use-shared-skills">Microsoft · utiliser des skills partagés</a> · Sélection au 19 octobre 2026</div>

<!--
1 min 30, 40:30–42:00. Après la pratique, choisir une procédure ou un outil pour un besoin récurrent. Pas de liste universelle « must have » : les installations et étoiles ne mesurent pas un consensus ni la qualité pour notre cas. Pour des analystes Python, pytest et des critères métier restent la base; validate-etl est notre procédure locale, pas un standard de la communauté. Playwright est l’outil de navigateur; playwright-cli est sa CLI et le dépôt Microsoft distribue un skill qui guide son emploi. Aspire propose une famille de skills, avec serveur MCP facultatif. dotnet/skills distribue des collections : regarder dotnet-aspnetcore ou dotnet-test selon la tâche, sans installer tout le catalogue. Matt Pocock fournit notamment grill-me pour questionner le besoin et tdd pour la boucle test en échec → correction → amélioration. Développer TDD : Test-Driven Development, développement piloté par les tests. Dire explicitement que nous commençons à explorer ces skills. Leur consultation ne constitue ni une installation ni une validation dans Copilot HQ. Voir handouts/skills-et-veille.md pour les dépôts, limites, indices d’adoption et chemins d’installation à vérifier selon le harnais. Sources S34–S38.
-->

---

# Votre prochaine tâche avec Copilot

<div class="lead">Écrivez un résultat attendu, confiez une petite tâche,<br>puis examinez les preuves.</div>

<div class="columns compact"><div>

## Avant de déléguer

Définir un résultat attendu.<br>Joindre les fichiers utiles.<br>Vérifier règles et autorisations.

</div><div>

## Avant d’accepter

Examiner le diff.<br>Lire les résultats des tests.<br>Conserver les décisions utiles.

</div></div>

<div class="strip compact">Quelle petite tâche pourriez-vous confier à Copilot dès demain?</div>

<!--
1 min 30, 42:00–43:30. Revenir à la pratique Copilot transférable à leurs projets : une demande délimitée, le bon contexte, des règles vérifiables, les permissions nécessaires et une preuve avant acceptation. Inviter chaque personne à choisir mentalement une petite tâche qu’elle connaît assez bien pour juger le résultat; réserver les échanges aux dix minutes de questions. Une correction de script, un test ou une revue convient. Relier au choix « vanille » : ajouter un skill ou un serveur MCP quand le besoin le justifie. Ne pas introduire de nouvelle technologie dans cette conclusion. Le SDD sera approfondi par l’autre équipe. La page suivante rassemble les ressources et restera visible pendant les questions; Canvas reste disponible en annexe 25 sur demande.
-->

---
class: resources
---

# Pour continuer et rester à jour

<ResourceLinks />

<div class="resource-routine">Routine proposée : 15 minutes de veille par semaine, puis une pratique testée sur votre projet.</div>

<div class="source">Liens publics vérifiés le 19 octobre 2026. Ressources HQ à compléter. <a href="/25">Canvas en préversion · annexe 25</a></div>

<!--
1 min 30, 43:30–45:00. Canvas reste une capsule facultative en annexe 25, pour les questions; ne pas l’ouvrir dans le parcours de 45 minutes. Laisser cette page visible pour les questions. Commencer par les nouveautés officielles : fonctions disponibles, préversions et migrations. Learn pour un exercice guidé; Awesome Copilot pour des exemples à relire; Community pour échanger. Aspire et Spec Kit prolongent le parcours de cette formation. Les espaces HQ sont des emplacements proposés, pas des ressources internes vérifiées. Pour les compléter, éditer uniquement presentation/resources/links.json : label, url et description dans internal. Une URL vide affiche « Lien interne à ajouter » sans créer de faux lien. Garder trois entrées courtes pour la projection; mettre les détails dans la fiche participant. Après modification, reconstruire le site et réexporter le PDF. Sources S39 et liens officiels de la fiche handouts/skills-et-veille.md. Questions 45:00–55:00; marge 55:00–60:00.
-->

---
class: annex
---

# Annexe · se repérer dans Customizations

<div class="screenshot-window overview-window"><img src="/vscode-customizations-overview.png" alt="Capture réelle du projet Formation Copilot - HQ : Overview sélectionné; menu Plugins, MCP Servers, Skills, Instructions, Agents, Hooks, Tools; zone Customize Your Agent." /></div>

<div class="scope-map">
<div><b>Instructions → User</b><code>~/.copilot/copilot-instructions.md</code><span>Vos préférences communes aux projets.</span></div>
<div><b>Instructions → Workspace</b><code>.github/copilot-instructions.md</code><span>Les règles du projet, partagées dans Git.</span></div>
</div>

<div class="small">Ouvrir une rubrique → examiner le fichier → vérifier son application.</div>

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/overview#agent-customizations-editor">Microsoft · panneau Customizations</a> · <a href="/8">Exemple de règle</a> · <a href="/11">Les rubriques →</a></div>
<div class="source">Capture fournie le 19 octobre : vue Overview. Les chemins ci-dessus expliquent les portées dans Instructions; ils ne sont pas visibles dans la capture.</div>

<!--
Annexe. La capture fournie le 19 octobre est conservée sans modification des pixels; seul le cadrage CSS se concentre sur le menu et Customize Your Agent. L’image complète est liée dans handouts/personnalisations-vscode.md; sa partie basse montre Explore Customizations, qui répète les rubriques en cartes. L’en-tête indique Copilot et Formation Copilot - HQ. Les nombres visibles dans la barre latérale ne prouvent pas que toutes ces capacités ont été utilisées dans une réponse. Overview est le point d’entrée; Customize Your Agent aide à préparer des fichiers, à relire et à valider pour le harnais choisi. Pour une règle, ouvrir Instructions, User ou Workspace, puis l’élément et son fichier. Les deux chemins sous l’image sont des repères explicatifs, pas des entrées visibles dans cette capture Overview. Les réglages User de VS Code et les fichiers du harnais sous ~/.copilot ne sont pas le même stockage. ~ désigne le dossier personnel de la machine où l’agent tourne. Vérifier References, puis un test de comportement; présence ne signifie pas conformité. Les formats, conditions et réglages de lisibilité sont détaillés dans handouts/personnalisations-vscode.md.
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

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/run/agent-harnesses#use-canvases-with-the-copilot-harness">Microsoft · prérequis et fonctionnement</a> · <a href="https://code.visualstudio.com/updates/v1_142#canvases-preview">Nouveauté 1.142</a> · <a href="/22">Retour →</a></div>

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

<div class="strip compact">Sources consultées les 18 et 19 octobre 2026. Les fonctions disponibles dépendent de la version et du compte.</div>

<div class="source">Registre complet : docs/SOURCES.md. Schémas pédagogiques; aucune capture VS Code inventée.</div>

<!--
Annexe de références. Les fiches locales reprennent les liens essentiels. Les fonctionnalités sont marquées expérimentales ou preview selon leur source. La date de vérification n’est pas une garantie sur toute version installée. Confirmer politiques HQ et modèles sur le portable. Les documents techniques du dépôt définissent les commandes finales. Aucun lien externe n’est requis pour afficher le support après build : polices et composants sont locaux. Le PDF fige les vues interactives; utiliser le web pour explorer.
-->


