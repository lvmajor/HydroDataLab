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
**30 s · 00:00–00:30**

> **Fil rouge :** déléguer à Copilot tout en gardant la maîtrise de son travail.

### Dire

- **Ouvrir :** « Comment déléguer à Copilot tout en gardant la maîtrise de son travail? »
- **Annoncer :** contexte, instructions, skills, outils, permissions et choix du modèle dans VS Code.
- **Situer l’exemple :** HydroDataLab rend ces pratiques observables dans un projet BI/Python.

### Enchaîner

« Partons de votre quotidien de développement. »

<details>
<summary>Précisions · cadrage de la formation</summary>

- Ce n’est pas un cours de construction d’ETL ou d’Aspire. La petite modification Python sert à observer la démarche et ses preuves.
- Ce support n’est pas une publication officielle d’Hydro-Québec; les exemples utilisent des données publiques.

</details>
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
**1 min · 00:30–01:30**

> **Fil rouge :** expérimenter librement, avec des données et des accès maîtrisés.

### Partir de leur quotidien

- **Dire :** « Vous développez et testez sur votre poste des ETL Python et des visualisations, puis vous préparez leur exécution dans des conteneurs. »
- **Définir ETL :** extraire, transformer, charger ou exporter les données.
- **Définir conteneur :** exécuter l’application depuis une image regroupant code et dépendances.

### Donner les quatre réflexes

- Données autorisées pour l’outil et l’usage; clés et mots de passe protégés.
- Accès de l’agent limités; changements relus et testés.
- **Rappeler :** exécuter l’ETL localement ne signifie pas que les données envoyées au service IA restent sur le poste.

### Enchaîner

« Cette marge d’expérimentation permet de comparer et d’apprendre. Voyons comment bien déléguer. »

<details>
<summary>Précisions · contexte HQ</summary>

- Le cadre plus souple est le contexte décrit par l’organisateur, pas une exemption aux règles de sécurité HQ ni une description vérifiée de ses politiques.
- Le déploiement exige encore une configuration et un environnement autorisé. Les règles internes s’appliquent aussi aux essais.

</details>
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
**30 s · 01:30–02:00**

> **Fil rouge :** cadrer la demande, fournir le contexte, puis vérifier le résultat.

### Annoncer le parcours

- Utiliser Agents → fournir des règles et du contexte → choisir capacités et modèle → vérifier un changement.
- **Durée :** 32 min d’explications, 13 min de démos, puis 10 min de questions et 5 min de marge.

### Enchaîner

« Voici les données qui nous serviront à vérifier ce que produit l’agent. »

<details>
<summary>Repères · niveau du public et annexes</summary>

- Aucune maîtrise de Git, Docker, .NET ou Vue n’est supposée : expliquer les mots avant de les utiliser.
- Les annexes 24 à 29 servent aux questions.

</details>
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
**2 min · 02:00–04:00**

> **Fil rouge :** connaître les données avant de demander à l’agent de les transformer.

### Montrer les trois familles

- **Demande électrique :** une puissance en MW, mesurée toutes les 15 minutes.
- **Niveau d’eau :** une mesure horaire en mètres; ce n’est pas un pourcentage de réserve.
- **Températures :** minima et maxima selon la définition source, sur une station distincte du capteur de niveau.

### Faire retenir

- Les fréquences diffèrent : ne pas attendre 24 valeurs pour chaque série.
- **Provenance :** 168 observations du 17 octobre, conservées pour reproduire les mêmes essais.
- On explore la qualité et la provenance des données, pas les opérations hydriques.

### Enchaîner

« Qui fait quoi quand nous demandons à Copilot de travailler sur ces données? »

<details>
<summary>Précisions · unités et manifeste</summary>

- `intervalMinutes` porte la fréquence propre à chaque série.
- Ne pas convertir directement une puissance instantanée en énergie, ni un niveau en réserve.
- Le manifeste indique une récupération le 18 octobre à 19:44:01 UTC.

</details>
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
**2 min · 04:00–06:00**

> **Fil rouge :** le modèle propose; le harnais encadre; les outils agissent.

### Raconter un exemple familier

- **Modèle :** propose une modification du script.
- **Outil :** exécute Python et retourne le résultat.
- **Harnais :** prépare le contexte et applique les autorisations.
- **Agent :** poursuit le travail à partir du résultat reçu.

### Situer notre environnement

- Nous utilisons le harnais **Copilot** dans VS Code.
- Un modèle Claude sélectionné dans Copilot ne change pas le harnais.
- **Attention au libellé Local :** c’est un autre harnais, pas un synonyme d’exécution sur le poste.

### Enchaîner

« Avant de lui confier un changement, repérons le dossier et la version sur lesquels il va agir. »

<details>
<summary>Pour les questions · Copilot SDK et version 1.142</summary>

- Le Copilot SDK est une base commune aux produits Copilot; cela ne garantit pas des outils et réglages identiques partout.
- En 1.142, le harnais Copilot existant devient progressivement le défaut des nouvelles sessions; il n’est pas né dans cette version.
- Sélectionner explicitement Copilot pour ce parcours.

</details>
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
**2 min · 06:00–08:00**

> **Fil rouge :** essayer un changement sans mélanger l’essai et la version de référence.

### Définir les repères Git

- **Dépôt :** dossier du projet avec son historique Git.
- **Commit :** version enregistrée dans cet historique; enregistrer le fichier dans l’éditeur ne suffit pas.
- **Branche :** chemin d’évolution nommé. `main` reste notre référence; `codex/repetition-01` accueille l’exercice.
- **Worktree :** autre dossier associé à une autre branche, avec un historique partagé. Facultatif ici.

### Montrer notre choix

- Une copie Git indépendante est déjà préparée pour l’exercice.
- Le dépôt principal reste sur `main` pendant toute la séance.
- **Faire reformuler :** « Dans quel dossier et sur quelle branche l’agent va-t-il agir? »

### Enchaîner

« Ouvrons maintenant notre première session Agents. »

<details>
<summary>Précisions · préparation et isolation</summary>

- La copie est préparée depuis un bundle et la branche `codex/preparation-demo-06` : améliorations actuelles présentes, couverture à compléter.
- Un worktree n’est ni un conteneur ni une isolation des services externes.
- Le conducteur décrit la préparation et le retour à `main` en secours.

</details>
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
**5 min · 08:00–13:00 · explication 1 min + démo 4 min**

> **Fil rouge :** « Quelles sources permettent de vérifier la réponse de Copilot? »

### Avant la démo · 08:00–09:00

- **Dire :** mission de découverte en lecture seule; le dépôt principal reste sur `main`.
- **À portée de main :** conducteur des démos, rubrique **Démo 1**, avec le prompt complet.
- Les repères ci-dessous démarrent au début de la démo, à **09:00**.

### 0:00–0:45 · Ouvrir la session

- `Chat: Open Agents Window` → vérifier dossier, branche `main`, harnais Copilot et modèle préparé.
- Créer une session; garder les confirmations d’actions. Situer `/plan`, sans lancer une deuxième demande.

### 0:45–1:30 · Régler la lisibilité

- `Ctrl+,` → montrer **Utilisateur / Espace de travail** et les réglages préparés.
- Repérer **Changes** : il peut rester vide ici; le vrai diff viendra en démo 3.

### 1:30–2:20 · Retrouver les consignes

- **Customizations** → situer **Overview** et **Customize Your Agent** pendant 10 secondes, sans envoyer de demande.
- **Instructions** → consignes communes et Python → retrouver la règle zéro/absence.

### 2:20–4:00 · Vérifier une réponse

- Coller le prompt du conducteur. Lire **References**.
- Ouvrir une fonction ou un test cité et vérifier une affirmation.
- **Enchaîner à 4:00 :** « Une consigne utile doit pouvoir se vérifier. » Revenir aux slides.

<details>
<summary>Secours · Copilot tarde ou est indisponible</summary>

- Ouvrir `pipeline.py` et un test local; annoncer que la génération n’a pas abouti en direct.
- Revenir aux slides à 4:00, même si la réponse n’est pas prête.

</details>

<details>
<summary>Précisions · réglages et références</summary>

- Auto Balance ou le modèle autorisé préparé. Dans cette fenêtre, ne pas chercher l’agent Plan du menu de la vue Chat.
- Les sept rubriques de personnalisation sont expliquées en slide 11.
- Un fichier qui existe n’est pas nécessairement chargé. Les références montrent des sources consultées; la présence d’une règle ne garantit pas son respect.

</details>
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

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Microsoft · instructions personnalisées</a> · <a href="/24">Se repérer dans Customizations →</a></div>

<!--
**2 min · 13:00–15:00**

> **Fil rouge :** zéro est une mesure; une absence n’en est pas une.

### Définir l’instruction

- **Dire :** « Une instruction est une consigne durable, enregistrée dans le projet et relue comme le code. »
- Montrer le fichier du dépôt et les trois températures fictives : **10, 0, None**.

### Lire l’exemple

- **0 °C :** température mesurée, donc valeur à compter.
- **None :** aucune mesure reçue, donc valeur à laisser absente.
- **Résultat :** 2 mesures sur 3, soit **66,67 %** de couverture.
- Remplacer l’absence par zéro produirait artificiellement 100 %.

### Enchaîner

« Où écrire cette règle, et quand sera-t-elle utilisée? »

<details>
<summary>Précisions · formats et contrôles métier</summary>

- En pandas, l’absence peut apparaître comme `NaN` ou `NA`; en JSON, `null`.
- Le test compte deux valeurs : ni une (zéro exclu), ni trois (absence remplacée).
- Zéro n’est pas physiquement plausible pour toute série : les contrôles métier restent nécessaires.
- La capture réelle est en annexe 24; l’ouvrir pendant la démo seulement si nécessaire.

</details>
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
**2 min · 15:00–17:00**

> **Fil rouge :** un fichier reconnu, une portée explicite, puis une vérification réelle.

### Lire le format

- **En-tête entre `---` :** YAML. **Corps :** Markdown.
- `etl/**/*.py` vise les fichiers Python dans `etl` et ses sous-dossiers, depuis la racine du workspace.
- `applyTo` cible les fichiers; `description` peut aider l’agent à choisir la règle selon la tâche. Ce sont deux mécanismes distincts et optionnels.

### Recommander une organisation simple

- Ici, privilégier les formats Copilot utilisés par le groupe : fichier commun et règles ciblées.
- Pour plusieurs harnais : `AGENTS.md` peut fournir un socle portable, si chacun le prend en charge.
- Éviter de dupliquer les mêmes consignes dans deux gros fichiers.

### Enchaîner

« Ces instructions rejoignent d’autres informations dans le contexte du modèle. »

<details>
<summary>Précisions · découverte et portabilité</summary>

- `instructions.md` seul n’est pas un nom magique : garder le suffixe `.instructions.md` et le dossier attendu.
- Une `description` peut conduire au chargement même sans correspondance de fichier.
- HydroDataLab conserve les règles communes dans `copilot-instructions.md`; son court `AGENTS.md` demande de le lire.
- Un renvoi Markdown ne garantit pas le chargement : vérifier **References** et tester le comportement.
- Les `AGENTS.md` imbriqués ont des règles de découverte propres au harnais; le réglage Local expérimental n’est pas universel.
- Détails : `handouts/personnalisations-vscode.md`.

</details>
-->

---

# Context layers · les couches du contexte

<ContextLayers />

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/custom-instructions">Microsoft · activation des règles</a> · <a href="https://code.visualstudio.com/docs/agents/guides/context-engineering-guide">Structurer le contexte</a></div>

<!--
**2 min · 17:00–19:00**

> **Fil rouge :** plusieurs apports composent un même contexte; ce dessin n’est pas un ordre de priorité.

### Lire les couches de haut en bas

- **Project instructions / Consignes du projet** et **File-specific instructions / Consignes ciblées** : règles durables.
- **Reference documents / Documents de référence** et **Task prompt / Demande courante** : informations et objectif du travail.
- **Tool results / Résultats des outils** : observations réelles après une action.

### Manipuler depuis la vue présentateur

- Cliquer **Reference documents** → montrer unité, fréquence et fuseau.
- Cliquer **Tool results** → montrer l’exemple **23/24**.
- Suivre les flèches vers **Model context / Contexte du modèle**.

### Enchaîner

« Voyons maintenant les capacités que l’on retrouve dans le panneau Customizations. »

<details>
<summary>Précisions · limites du schéma</summary>

- *Context layers* signifie couches du contexte : catégories pédagogiques, pas hiérarchie officielle ni garantie de chargement automatique de tous les fichiers.
- « Contrat » désigne ici un document décrivant les données, pas un niveau universel de contexte.
- Historique, instructions de service et préférences utilisateur existent aussi; le schéma se concentre sur les apports préparables dans le projet.
- Les consignes applicables se combinent; résoudre les contradictions. Les détails `applyTo` restent en slide 9.
- Le PDF conserve toutes les couches, traductions et exemples; le panneau de droite affiche la première couche.

</details>
-->

---

# Les personnalisations, dans VS Code

<CustomizationMap />

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/concepts/customization">Microsoft · comparer les concepts</a> · <a href="https://code.visualstudio.com/docs/agent-customization/overview">Le panneau</a> · <a href="/24">Vue d’ensemble et portées →</a></div>

<!--
**1 min 15 · 19:00–20:15**

> **Fil rouge :** repérer ce qui guide l’agent, ce qui lui donne des moyens et ce qui se partage.

### 0:00–0:15 · Partir du menu

- **Montrer Overview :** point d’entrée des personnalisations.
- **Dire :** « Les trois familles à droite nous aident à nous repérer. »

### 0:15–0:40 · Guider le travail

- **Instructions :** règles → préserver les absences.
- **Skills :** méthode réutilisable → valider l’ETL.
- **Agents :** rôle, consignes et outils → réviseur BI.

### 0:40–1:05 · Donner des moyens et partager

- **Tools :** outils disponibles → lecture, terminal.
- **MCP Servers :** connecter des outils et données externes → lecture SQL.
- **Hooks :** commande déclenchée par un événement configuré.
- **Plugins :** distribuer un ensemble → skill et serveur MCP.

### 1:05–1:15 · Enchaîner

« Une skill apporte la méthode; un outil permet d’agir. Voyons quand chacun intervient. »

<details>
<summary>Pour les questions · interface et exemples</summary>

- **Customize Your Agent** aide à préparer des fichiers à relire; visible en annexe 24.
- **Explore Customizations** ouvre les mêmes rubriques sous forme de cartes.
- Les compteurs ne prouvent pas l’utilisation dans une réponse. La capture est agrandie sans retoucher ses pixels.
- MCP = *Model Context Protocol*. SQL, hook et plugin sont des exemples illustratifs, pas des composants identifiés dans la capture.
- Les trois familles ne sont pas une hiérarchie officielle ni un ordre de priorité. Formats dans la fiche; aucune configuration supplémentaire en direct.

</details>
-->

---

# Skills et tools : deux choix distincts

<SkillToolFlow />

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agent-customization/agent-skills#how-copilot-uses-skills">Microsoft · chargement progressif</a> · <a href="https://code.visualstudio.com/docs/agent-customization/agent-skills#use-skills-as-slash-commands">Activation</a> · <a href="https://code.visualstudio.com/docs/agents/run/tools">Choix des outils</a></div>

<!--
**1 min 30 · 20:15–21:45**

> **Fil rouge :** une skill apporte une méthode au contexte; un outil exécute une action.

### 0:00–0:35 · Suivre le parcours Skill

- **Repérer :** nom et description rendent la skill découvrable.
- **Charger :** si la tâche correspond, le modèle peut choisir la skill et lire son `SKILL.md`. On peut aussi utiliser `/nom`.
- **Approfondir :** références et scripts sont consultés au besoin, pas tous chargés d’avance.

### 0:35–1:00 · Situer notre cas

- `validate-etl` contient **`disable-model-invocation: true`**.
- **Dire :** « Ici, nous l’invoquons avec `/validate-etl`. C’est notre réglage, pas le comportement par défaut de toutes les skills. »

### 1:00–1:20 · Suivre le parcours Tool

- Le modèle choisit **le terminal et ses paramètres** parmi les outils disponibles.
- Le harnais vérifie les permissions, demande confirmation si nécessaire, puis lance l’appel autorisé.
- Le résultat revient au modèle : **terminal = outil; pytest = programme lancé**.

### 1:20–1:30 · Enchaîner

« Charger une méthode ne lance pas automatiquement ses scripts et n’accorde aucune permission. Gardons une configuration simple. »

<details>
<summary>Pour les questions · activation et choix</summary>

- Le choix dépend de la demande, du contexte et des descriptions. Être découvrable ne garantit pas une sélection à chaque demande pertinente.
- La description d’une skill doit donc expliquer **quand** l’utiliser. Le schéma décrit son chargement dans le contexte courant, hors modes expérimentaux.
- Une skill n’est pas obligatoire avant chaque appel d’outil. Elle ne crée ni outil ni permission.
- `user-invocable: false` masque la commande du menu `/`, mais permet encore le choix automatique.
- Avec aussi `disable-model-invocation: true`, les deux voies sont désactivées. Matrice détaillée dans la fiche.
- Une référence `#` peut orienter le choix d’outil, sans contourner les contrôles.
- Ne pas modifier `validate-etl` ni lancer sa procédure pendant cette explication.

</details>
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
**45 s · 21:45–22:30**

> **Fil rouge :** partir des capacités natives, ajouter seulement ce qui répond à un besoin observé.

### Dire

- **Vanille :** capacités natives et quelques consignes lisibles. C’est ma préférence pour commencer.
- Chaque intégration ajoute des versions, permissions et comportements à comprendre.
- Un bon skill évite de répéter une procédure : choisir un format standard, une portée limitée et une version connue.

### Enchaîner

« Comparons le résultat sur une vraie tâche. Le nombre d’ajouts ne dit pas, à lui seul, combien de tokens nous consommons. »

<details>
<summary>Pour les questions · nuances et entretien</summary>

- Recommandation pédagogique, pas politique HQ ni mesure universelle de productivité.
- Exemple documenté : changer de harnais peut nécessiter de déplacer ou convertir des personnalisations.
- La configuration native évolue aussi : rester simple ne dispense pas de lire les nouveautés.
- Les skills chargent leur contenu progressivement; leur nombre seul ne prouve ni surcharge de tokens ni ralentissement.
- Un plugin maintenu peut faciliter la distribution. Comparer avant/après sur une tâche réelle.
- Sources S32 et S33, consultées le 19 octobre 2026.

</details>
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
**1 min 30 · 22:30–24:00**

> **Fil rouge :** fournir les bonnes informations et limiter les reprises inutiles.

### Expliquer le coût du contexte

- Un **token** est une unité de découpage du texte, pas forcément un mot.
- Règles, documents et résultats occupent aussi le contexte, dont la capacité est limitée.
- Un petit prompt peut déclencher beaucoup de lectures.

### Donner une recette concrète

- Joindre le dictionnaire, la fonction ciblée et ses tests.
- Fournir quelques lignes avec **zéro, absence et doublon**.
- Demander l’extrait de journal autour de l’erreur, pas toute la sortie.
- Nouvelle tâche sans rapport → nouvelle session. Conserver décisions et état dans les fichiers.

### Enchaîner

« Le choix du modèle compte aussi, mais une reprise peut annuler l’économie initiale. »

<details>
<summary>Pour les questions · compactage et cache</summary>

- `/compact` résume la conversation, avec un risque de perte de détails.
- Le cache réutilise certains calculs d’entrée et peut modifier leur prix; ce n’est pas une instruction permanente.
- Les fichiers de suivi permettent la reprise après interruption.

</details>
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
**1 min 30 · 24:00–25:30**

> **Fil rouge :** Auto Balance est notre point de départ, puis nous vérifions le résultat et le coût.

### Expliquer Auto

- Auto tient compte de la complexité et de la disponibilité, dans les politiques applicables.
- Les niveaux changent une **préférence de routage**, pas une liste exclusive de modèles.
- **Balance :** défaut pédagogique de cette formation, pas règle institutionnelle.

### Montrer ce qui se vérifie

- Le modèle utilisé apparaît dans les informations de la réponse.
- Une remise ne garantit pas une économie face à tout choix manuel.
- **Repère :** 1 AI credit = 0,01 dollar US.

### Enchaîner

« Regardons maintenant quatre tâches avec des volumes supposés, pour comparer les choix manuels. »

<details>
<summary>Pour les questions · régime de facturation</summary>

- Les anciens abonnements annuels individuels peuvent relever d’un régime historique distinct du cas entreprise.
- Confirmer l’offre et les modèles autorisés sur le poste HQ.

</details>
-->

---

# Comparer 4 tâches et 5 modèles

<ModelChooser />

<div class="doc-link"><a href="https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing">GitHub · tarifs au 18 octobre 2026</a> · <a href="https://docs.github.com/en/copilot/reference/ai-models/model-comparison">Comparer les modèles</a></div>

<!--
**2 min · 25:30–27:30**

> **Fil rouge :** comparer le coût d’un résultat validé, pas seulement celui d’un essai.

### Lire le graphique

- **Colonnes :** quatre tâches. **Lignes :** cinq modèles.
- Lire verticalement pour comparer les modèles d’une tâche; horizontalement pour suivre un modèle.
- Barres à échelle commune et valeurs en dollars US; volumes fictifs inscrits sous chaque tâche.

### Guider les choix

- **Luna / Haiku :** explication courte. **Sonnet :** modification délimitée et testée.
- **Sol :** changements complexes. **Opus :** diagnostic difficile.
- Le losange est un choix pédagogique à essayer, **pas un score mesuré**. Auto Balance reste le départ conseillé.

### Manipuler et enchaîner

- Depuis le présentateur, passer de **1 à 2 essais** avec `+`, puis revenir à **1** avec `−`.
- **Dire :** « Les reprises se cumulent. Plusieurs passages peuvent aussi être organisés par HydraFusion. »

<details>
<summary>Précisions · hypothèses des coûts</summary>

- Plafond des barres : 0,40 dollar par essai. Les valeurs numériques rendent lisibles les petits montants.
- Sonnet et Sol ont le même tarif, sans preuve d’une qualité identique.
- Le volume réel des essais peut varier. Cache et remise Auto exclus; aucun prix fixe inventé pour Auto.
- Ce n’est ni un benchmark ni une mesure du coût d’un projet. Confirmer les modèles disponibles dans le sélecteur HQ.

</details>
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
**1 min · 27:30–28:30**

> **Fil rouge :** HydraFusion organise un parcours; ce n’est pas un modèle supplémentaire.

### Distinguer les parcours

- **Single :** un passage peut suffire.
- **Cascade :** évalue puis peut escalader.
- **Critique :** ajoute revue puis révision.
- Le parcours et l’effort varient selon le prompt; les passages supplémentaires peuvent augmenter durée et crédits.

### Enchaîner

« Quelle que soit la stratégie, il reste à vérifier ce que le système a réellement produit. »

<details>
<summary>Précisions · capsule préparée</summary>

- Research preview : aucune promesse de production ni d’économie universelle.
- Les modèles doivent être disponibles dans l’offre et autorisés par les politiques; l’utilisateur ne choisit pas individuellement le pool.
- HydraFusion ne lance pas systématiquement plusieurs modèles. Aucune activation requise pour la formation.

</details>
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
**6 min · 28:30–34:30 · explication 1 min + démo 5 min**

> **Fil rouge :** confronter une réponse convaincante à un résultat exécuté et traçable.

### Avant la démo · 28:30–29:30

- **Dire :** Vue présente, .NET pilote, Python calcule. Une API relie les programmes; Aspire démarre les composants et regroupe les journaux.
- Infrastructure préparée; rester sur `main`. Ouvrir le conducteur, **Démo 2**.
- Les repères ci-dessous démarrent au début de la démo, à **29:30**.

### 0:00–0:45 · Exécuter la CLI

- Lancer le bloc CLI préparé du conducteur.
- Montrer **168 observations, 4 séries et `hourly.csv`**; les sources locales rendent l’essai reproductible.

### 0:45–2:15 · Lire l’avant / après ETL

- Vue → **Parcours ETL** → Extraire, Transformer, Charger / exporter. Utiliser la collecte réussie préparée.
- Champs fournisseur → schéma normalisé → **24 moyennes horaires**.
- Lire un seul indicateur : **pointe horaire moyenne 22 009,75 MW à 18 h Toronto**.

### 2:15–3:15 · Retrouver la règle et son test

- Dans VS Code, ouvrir `analysis.py` et `test_analysis.py`.
- Comparer la règle **4 quarts d’heure valides** avec un test; éviter le parcours complet du code.

### 3:15–4:15 · Examiner une exécution

- **Voir les détails** d’un succès → paramètres, étapes Python, durées et export propre à cette exécution.
- Quartz déclenche; SQLite conserve le bilan. Un seul journal Aspire si le temps le permet.

### 4:15–5:00 · Rejouer dans Docker

- Lancer la commande préremplie `--pull never --network none`; comparer le résumé.
- **Enchaîner à 5:00 :** « Maintenant, formulons le critère d’une petite modification. » Revenir aux slides.

<details>
<summary>Secours · Docker ou services indisponibles</summary>

- Montrer le résultat préparé en l’annonçant. Aucune construction ni installation devant le public.
- Revenir aux slides à 5:00.

</details>

<details>
<summary>Variante · échec et relance, à choisir avant la séance</summary>

- Remplacer uniquement **3:15–5:00** par échec contrôlé, détails et relance.
- Dans ce cas, ne pas ajouter Aspire ni Docker. Ne pas cumuler les deux parcours; la planification reste désactivée.

</details>

<details>
<summary>Précisions · ce que montre le tableau de bord</summary>

- Les onglets explorent un rapport déjà calculé; ils ne lancent pas le pipeline au clic.
- Les étapes Python sont un bilan, pas une animation en temps réel.
- Autres indicateurs réservés aux questions. La vérification du résultat est le message central.
- CLI : `uv run --project etl python -m hydrodatalab run --source fixture --date 2026-10-17 --output outputs/cli`.

</details>
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
**2 min · 34:30–36:30**

> **Fil rouge :** SDD — définir un résultat vérifiable avant de demander le code.

### Montrer · garder cette slide projetée

- Dans `$Depot` : `./scripts/stop-demo.ps1`.
- Dans `$Live` : `git status --short --branch`, puis `./scripts/start-demo.ps1 -NoBuild`.
- Recharger Vue, lancer une collecte complète et vérifier **Couverture : Non calculée**. Les slides restent sur `main`.

### Dire pendant la transition

- **Spec :** spécification. **Critère d’acceptation :** exemple vérifiable qui décide si le résultat convient.
- **Test :** compare le résultat calculé au résultat attendu.
- **Couverture :** mesures valides dans des créneaux distincts / créneaux attendus × 100.
- SDD : besoin → critères relus → plan → tâches → code → preuves. L’autre équipe approfondira cette démarche.

### Enchaîner

« Confions cette règle délimitée à Copilot et examinons ses preuves. »

<details>
<summary>Secours · démarrage trop long</summary>

- Si la bascule ne tient pas dans les deux minutes répétées, utiliser le secours annoncé. Ne pas improviser d’installation.
- La copie utilise `codex/repetition-01`, issue de `codex/preparation-demo-06` (`9e1ecde`), pas un ancien tag seul.

</details>

<details>
<summary>Pour les questions · calcul et Spec Kit</summary>

- Le dénominateur n’est pas le nombre de lignes reçues. Un zéro compte; un doublon ne crée pas un créneau.
- Construire les bornes de la journée à Toronto, puis comparer les instants UTC, référence de temps commune.
- La demande à 15 minutes attend 96 points; 92 ou 100 aux changements d’heure.
- Spec Kit aide à organiser les documents SDD; aucune installation ici.

</details>
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
**4 min · 36:30–40:30 · démo 3**

> **Fil rouge :** une règle en français devient un calcul testé, puis un contrôle visible.

### 0:00–0:40 · Montrer le point de départ

- Dans la copie `$Live`, branche `codex/repetition-01` : **Couverture : Non calculée**.
- Ouvrir `specs/coverage.md`, `coverage.py` et `specs/test_coverage_acceptance.py`.
- Deux tests d’acceptation rouges vérifiés avant la séance. Prompt et commandes : conducteur, **Démo 3**.

### 0:40–2:10 · Confier la modification à Copilot

- Nouvelle session dans cette copie; modèle autorisé préparé; coller le prompt du conducteur.
- Limiter le changement à la fonction Python et demander les résultats réels des tests.
- Ne pas recréer l’API ou Vue, ni assouplir les critères.

### À 2:10 · Décider

- Si code et tests ne sont pas prêts : **« Je passe à la solution préparée pour garder le temps de vérifier. »**
- Arrêter la génération et les services de `$Live`, préserver ses fichiers, démarrer `$Depot/main` avec `-NoBuild`.

### 2:10–3:00 · Examiner les preuves

- Lire **Changes** et les tests réels; lancer la commande de validation du conducteur depuis la racine active.
- La solution préparée passe **43 tests**, dont les journées de 23/25 heures.

### 3:00–4:00 · Voir le contrôle fonctionner

- Collecte complète, puis collecte avec **Retirer une mesure de niveau d’eau**.
- **Qualité par série : 24/24 = 100 %, puis 23/24 = 95,83 %.** Marquer une pause.
- **Enchaîner à 4:00 :** « Vous voyez ce que cette règle permet de détecter. » Revenir à la slide 21.

<details>
<summary>Secours · preuve préparée et limites</summary>

- Si les services tardent, montrer la preuve préparée en le disant; ne pas dépasser 4:00.
- En secours, utiliser le diff historique limité à `coverage.py` décrit dans le conducteur et annoncer sa provenance.
- Ne pas basculer le dépôt principal sur un ancien tag. La transition vers la copie préparée a déjà eu lieu en slide 19.
- La copie vient de `codex/preparation-demo-06` (`9e1ecde`) et conserve la couverture à implémenter.

</details>

<details>
<summary>Précisions · commande et dénominateurs</summary>

- `uv run --project etl pytest etl/tests specs/test_coverage_acceptance.py --basetemp etl/.test-output/live`.
- Le **99,4 % global** utilise un autre dénominateur. Le retrait reste en mémoire; les sources sont conservées.

</details>
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
**1 min 30 · 40:30–42:00**

> **Fil rouge :** choisir une procédure ou un outil pour un besoin récurrent, pas pour remplir un catalogue.

### Relier chaque exemple à une tâche

- **Python :** pytest et critères métier restent la base; `validate-etl` est notre procédure locale.
- **Navigateur :** Playwright pour vérifier l’interface.
- **Application distribuée :** famille de skills Aspire; serveur MCP facultatif.
- **.NET :** regarder `dotnet-aspnetcore` ou `dotnet-test`, selon la tâche.
- **Matt Pocock :** `grill-me` pour questionner le besoin; `tdd` pour la boucle test → correction → amélioration.

### Nuancer et enchaîner

- **Dire :** « Nous commençons à explorer ces skills; ce n’est pas une liste universelle de must have. »
- « Quel besoin récurrent justifierait un premier ajout dans votre travail? »

<details>
<summary>Pour les questions · outils, adoption et installation</summary>

- Playwright est l’outil; `playwright-cli` est sa CLI; le dépôt Microsoft distribue un skill pour guider son emploi.
- `dotnet/skills` distribue des collections : ne pas installer tout le catalogue par défaut.
- TDD = *Test-Driven Development*, développement piloté par les tests.
- Étoiles et installations ne mesurent pas un consensus ni la qualité pour notre cas.
- Consultation ne signifie pas installation ni validation dans Copilot HQ.
- Dépôts, limites et chemins à vérifier selon le harnais : `handouts/skills-et-veille.md`, sources S34–S38.

</details>
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
**1 min 30 · 42:00–43:30**

> **Fil rouge :** choisir une petite tâche dont vous savez juger le résultat.

### Faire choisir un premier essai

- **Dire :** « Pensez à une correction de script, un test ou une revue que vous connaissez assez bien pour vérifier le résultat. »
- Demande délimitée → bon contexte → règles vérifiables → permissions nécessaires → preuve avant acceptation.

### Ancrer la pratique

- Commencer simplement; ajouter une skill ou un serveur MCP quand un besoin le justifie.
- Garder les échanges pour les dix minutes de questions. Le SDD sera approfondi par l’autre équipe.

### Enchaîner

« Voici les ressources pour poursuivre. Cette page restera affichée pendant les questions. »

<details>
<summary>Repères · conclusion</summary>

- Ne pas introduire de nouvelle technologie dans cette conclusion.
- Canvas reste disponible en annexe 26 sur demande.

</details>
-->

---
class: resources
---

# Pour continuer et rester à jour

<ResourceLinks />

<div class="resource-routine">Routine proposée : 15 minutes de veille par semaine, puis une pratique testée sur votre projet.</div>

<div class="source">Liens publics vérifiés le 19 octobre 2026. Ressources HQ à compléter. <a href="/26">Canvas en préversion · annexe 26</a></div>

<!--
**1 min 30 · 43:30–45:00**

> **Fil rouge :** suivre les sources officielles, puis essayer les pratiques dans son contexte.

### Orienter vers les ressources

- **Nouveautés officielles :** fonctions disponibles, préversions et migrations.
- **Learn :** exercice guidé. **Awesome Copilot :** exemples à relire. **Community :** échanges.
- **Aspire et Spec Kit :** prolongements du parcours.
- **Espaces HQ :** emplacements à compléter, pas des ressources internes vérifiées.

### Ouvrir les questions

- Laisser cette page visible de **45:00 à 55:00**; marge jusqu’à **60:00**.
- Canvas en annexe 26 seulement si demandé; ne pas l’ajouter aux 45 minutes.

<details>
<summary>Préparation · ajouter les ressources HQ</summary>

- Éditer `presentation/resources/links.json` : `label`, `url` et `description` dans `internal`.
- Une URL vide affiche « Lien interne à ajouter » sans faux lien. Garder trois entrées courtes pour la projection.
- Mettre les détails dans la fiche participant, puis reconstruire le site et réexporter le PDF.
- Sources S39 et fiche `handouts/skills-et-veille.md`.

</details>
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
**Annexe · pendant les questions**

> **Fil rouge :** retrouver le fichier derrière le menu et vérifier sa portée.

### Montrer

- En-tête **Copilot · Formation Copilot - HQ**; **Overview** comme point d’entrée.
- **Customize Your Agent :** aide à préparer des fichiers, à relire et à valider pour le harnais choisi.
- Pour une règle : **Instructions → User ou Workspace → élément → fichier**.

### Faire retenir

- Compteur ≠ preuve d’utilisation. Présence ≠ conformité.
- Vérifier **References**, puis un test de comportement.

### Revenir au fil

Revenir aux ressources, slide 23, après la réponse.

<details>
<summary>Précisions · capture et stockage</summary>

- Capture du 19 octobre, pixels conservés; cadrage CSS sur le menu et Customize Your Agent.
- Image complète liée dans `handouts/personnalisations-vscode.md`. **Explore Customizations** répète les rubriques en cartes.
- Les chemins sous l’image sont des explications, pas des entrées visibles dans cette capture Overview.
- Les réglages User de VS Code et les fichiers du harnais sous `~/.copilot` sont des stockages distincts.
- `~` désigne le dossier personnel de la machine où l’agent tourne. Formats, conditions et lisibilité détaillés dans la fiche.

</details>
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
**Annexe · pendant les questions**

> **Fil rouge :** utiliser les tarifs Copilot et mesurer le coût d’un résultat validé.

### Expliquer

- La grille provient de **GitHub Copilot**, pas d’une extrapolation des API fournisseurs.
- Lecture et écriture du cache sont distinctes; ne pas les additionner sur tous les tokens.
- Mesurer **exactitude, reprises, temps et crédits** pour évaluer un choix de modèle.

### Revenir au fil

Coût réel = usage et contrat de facturation. Revenir à la slide 23.

<details>
<summary>Précisions · seuils et simulation</summary>

- Les seuils modifient la grille. Voir `matrice-modeles.md` pour les paliers longs et la simulation.
- Les coûts d’infrastructure de revue cloud ne sont pas modélisés.

</details>
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

<div class="doc-link"><a href="https://code.visualstudio.com/docs/agents/run/agent-harnesses#use-canvases-with-the-copilot-harness">Microsoft · prérequis et fonctionnement</a> · <a href="https://code.visualstudio.com/updates/v1_142#canvases-preview">Nouveauté 1.142</a> · <a href="/23">Retour →</a></div>

<!--
**Annexe · pendant les questions**

> **Fil rouge :** un Canvas est une interface liée à un chat, fournie par une extension.

### Situer la fonctionnalité

- **Preview desktop**, sessions locales du harnais Copilot dans Agent Host.
- Selon l’extension, utilisateur et agent font évoluer le même état.
- **Add Tab (+) → Canvas** ouvre une extension déjà disponible; ce menu ne crée pas une capacité.

### Illustrer sans installer

- Exemple préparatoire : « Crée un canvas Kanban pour extraire, valider, transformer et exporter, avec les critères de réussite de chaque carte. »
- Notre Slidev n’est pas relié à Copilot. Aucun Canvas créé ou installé pour cette révision.

### Revenir au fil

Revenir aux ressources, slide 23.

<details>
<summary>Pour les questions · chemins de création</summary>

- `/create-canvas` crée une extension dans le workspace quand la skill est disponible.
- Autre chemin documenté : installer un chat plugin contenant une extension Canvas via l’application GitHub Copilot, puis demander les Canvas disponibles dans VS Code.
- Préparer tout essai avant la séance, si souhaité et autorisé.

</details>
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
**Annexe · pendant les questions**

> **Fil rouge :** préciser les règles des données avant de demander le code.

### Vérifier le contrat

- Journée métier : **début inclusif, fin exclusive**. Construire les bornes locales, puis comparer en UTC.
- **Zéro ≠ absence**; un doublon n’améliore pas la couverture.
- Conserver la définition et la fréquence propres aux minima/maxima.
- Si le schéma source change, signaler l’échec plutôt que fabriquer un indicateur.

### Revenir au fil

« Ce sont des décisions à écrire dans la spec. » Revenir à la slide 23.

<details>
<summary>Précisions · instants et fréquences</summary>

- Un instant UTC ne définit pas à lui seul la journée métier, notamment aux changements d’heure.
- Ne pas attribuer arbitrairement l’attendu horaire aux séries min/max : garder leur agrégation propre.

</details>
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
**Annexe · pendant les questions**

> **Fil rouge :** reprendre à partir de l’état vérifié, en préservant le travail en cours.

### Montrer les repères de reprise

- Lire **PROGRESS.md**, l’état Git et le dernier contrôle réussi.
- Préserver la mini-démo en cours; garder source et runtime séparés.
- Utiliser les identifiants et commandes du conducteur validé.
- Les données locales rendent la reprise indépendante du réseau.

### Revenir au fil

« Le dernier message n’est pas la preuve qu’une commande est entièrement terminée. » Revenir à la slide 23.

<details>
<summary>Précautions · branches et données</summary>

- Cette carte Git n’est pas une invitation à utiliser `reset --hard`.
- La séparation des runtimes évite de réutiliser une base SQLite d’une autre étape.
- Les fixtures évitent une source mouvante pendant la prestation.

</details>
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
**Annexe · pendant les questions**

> **Fil rouge :** des références datées et des commandes locales vérifiées.

### Orienter la réponse

- Les fiches locales reprennent les liens essentiels et les mentions preview ou expérimental.
- Confirmer les politiques HQ et les modèles sur le portable; une date de vérification ne garantit pas toutes les versions.
- Les documents techniques du dépôt définissent les commandes finales.

### Clore la réponse

Revenir à la slide 23 pour poursuivre les questions.

<details>
<summary>Repères · hors ligne</summary>

- Après build, polices et composants sont locaux : aucun lien externe requis pour afficher le support.
- Le PDF fige les interactions; utiliser le web pour explorer.

</details>
-->


