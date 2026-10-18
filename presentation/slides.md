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
1 minute, 00:00–01:00. Ouvrir avec une question : « Si l’agent produit un beau graphique, qu’est-ce qui vous permet de croire aux données? » La formation porte sur la délégation d’un travail vérifiable. Le projet est polyglotte, mais le geste principal sera une modification Python accessible aux personnes qui font du BI. L’agent n’est pas une autorité sur l’hydrologie. Ce support n’est pas une publication officielle d’Hydro-Québec. Les chiffres d’exemple sont signalés; les données réelles viennent des sources publiques et de leur instantané documenté.
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
1 minute, 01:00–02:00. Public BI/Python : aucune maîtrise de Git, Docker, .NET ou Vue n’est supposée. Les mots utiles seront expliqués avant leur emploi. Les démonstrations sont réparties dans les 45 minutes; 10 minutes de questions et 5 de marge suivent. Les annexes 19 à 24 servent aux questions. Objectif de sortie : le participant peut décrire une règle, donner les bons éléments à l’agent et examiner une preuve de résultat. Ne pas dérouler une liste de technologies.
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

# Un modèle, des outils, un agent

| Notion | Dans notre tâche Python |
|---|---|
| **Modèle** | Produit une proposition à partir des informations reçues. |
| **Outils** | Lisent un fichier, le modifient ou exécutent les tests. |
| **Agent** | Utilise le modèle et les outils pour avancer dans la tâche. |

<div class="flow"><b>Lire</b><span>→</span><b>Proposer</b><span>→</span><b>Tester</b><span>→</span><b>Corriger</b></div>

<div class="strip">Vous fixez l’objectif et les autorisations; vous examinez le résultat.</div>

<div class="source">Fonctionnement simplifié d’un agent [S05, S06].</div>

<!--
2 minutes, 04:00–06:00. Commencer par un script que le public connaît : « calcule une moyenne ». Le modèle propose du texte ou du code; l’outil exécute réellement Python; l’agent peut lire le résultat et ajuster sa proposition. Le contexte est l’information disponible pour cette tâche, par exemple le fichier et la règle de calcul. Une affirmation de succès dans le chat ne remplace pas une sortie de tests. Une session est une conversation consacrée à une tâche. Garder la distinction moteur/modèle pour la démonstration : Copilot est le logiciel qui fournit l’environnement de l’agent; le modèle est un choix à l’intérieur de cet environnement. Ne pas introduire harness ni sous-agents ici.
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

<div class="source">Notions Git [S22]. Les fichiers et résultats non enregistrés demandent leur propre sauvegarde.</div>

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

<div class="source">Fenêtre Agents, modes et réglages [S01, S02, S03, S04, S16].</div>

<!--
1 minute d’explication puis 4 minutes de démo, 08:00–13:00. La diapositive donne une mission et une question au public; la manipulation suivante reste dans ces notes.
0:00–0:45 : ouvrir Chat: Open Agents Window (ou code --agents), montrer le dossier et la branche déjà expliqués. Présenter une session comme une conversation pour une tâche. Copilot est le moteur d’exécution choisi; le modèle sera sélectionné à l’intérieur de Copilot. Auto choisit le modèle pour nous; ses niveaux seront expliqués plus loin. Montrer /plan pour demander une démarche avant l'implémentation. La fenêtre Agents ne propose pas actuellement l'agent Plan dans son menu; /plan est disponible dans une session Copilot. Ne pas confondre les sélecteurs de la vue Chat et de cette fenêtre. Laisser les confirmations manuelles; expliquer une autorisation au moment où elle apparaît.
0:45–1:30 : Ctrl+, agrandir la lecture et montrer le retour à la ligne. Expliquer la portée : Utilisateur = mes projets; Espace de travail = ce projet. Ne pas appliquer un réglage global sans raison. Montrer le panneau Changes, soit la comparaison avant/après lorsqu’un fichier change.
1:30–2:20 : Chat: Open Customizations, puis Instructions. Une instruction est une consigne écrite destinée à l’agent; les règles permanentes seront détaillées à la diapositive suivante. Montrer la règle Python zéro/absence dans le dépôt.
2:20–4:00 : lancer le prompt de découverte dans handouts/prompts.md; lire References comme « fichiers ou sources consultés ». Ouvrir un fichier cité et faire vérifier une affirmation par le public. Découverte ne signifie pas respect : il faudra encore tester le comportement.
Si l’authentification échoue, suivre les mêmes repères dans les fichiers locaux, en annonçant que la réponse ne sera pas générée en direct.
-->

---

# Des consignes qui survivent au chat

<div class="lead">Une instruction permanente est une consigne<br>conservée dans le projet pour les prochaines tâches.</div>

<div class="columns"><div>

## Pour tout le projet

« Conserver les données brutes. »

<code>.github/copilot-instructions.md</code>

</div><div>

## Pour les fichiers Python

« Une absence reste absente;<br>zéro reste une valeur valide. »

<code>.github/instructions/<br>python.instructions.md</code>

</div></div>

<div class="strip">L’agent reçoit la règle; les tests vérifient le comportement.</div>

<div class="source">Instructions communes et ciblées [S03]. Leur découverte dépend du moteur d’exécution choisi.</div>

<!--
2 minutes, 13:00–15:00. Rappeler la règle montrée pendant la démo. Socle commun : décision utile à toutes les tâches. Consigne ciblée : règle pertinente pour certains fichiers. Ces fichiers sont enregistrés dans Git et relus comme le code; ils ne sont pas la mémoire du dernier chat. AGENTS.md partage aussi des consignes entre agents compatibles, avec des règles de découverte propres au moteur. Préférences personnelles Copilot : ~/.copilot/copilot-instructions.md. Les consignes d’organisation peuvent également s’appliquer. Éviter plusieurs longues copies contradictoires. Les instructions de chat ne gouvernent pas automatiquement les suggestions inline. Les procédures et rôles seront définis à la diapositive 9 avant leurs formats.
-->

---

# Limiter une consigne aux fichiers utiles

<div class="small"><code>applyTo</code> indique les fichiers concernés.<br><code>etl/**/*.py</code> signifie : fichiers Python dans <code>etl/</code> et ses sous-dossiers.</div>

```md
---
description: Qualité des transformations Python
applyTo: "etl/**/*.py"
---
Une absence reste absente. Zéro est une valeur valide.
```

<div class="strip">Exemple : le test doit compter <code>0</code>, mais ignorer une valeur absente.</div>

<div class="source">Fichier Markdown .github/instructions/python.instructions.md [S03]. Exemple simplifié.</div>

<!--
2 minutes, 15:00–17:00. Lire d’abord la phrase métier. Le bloc entre les séparateurs est l’en-tête du fichier Markdown : description aide à reconnaître le sujet, applyTo cible les fichiers. Le public n’a pas à mémoriser le motif; expliquer ** = sous-dossiers et *.py = noms terminés par .py. Montrer ensuite le fichier réel, dont cet exemple est une version courte. Les consignes applicables se complètent; ne pas enseigner une priorité universelle « le plus proche gagne ». Aligner les contradictions. Pour le diagnostic : vérifier le moteur, l’emplacement, puis les références de la réponse. Developer: Open Agent Debug Logs est réservé aux questions avancées. Un lien Markdown n’assure pas l’envoi automatique de tout son contenu.
-->

---

# Réutiliser une recette ou un rôle

<div class="columns"><div>

## Une recette : une skill

Des étapes réutilisables pour<br>accomplir une tâche précise.

Exemple : <strong>valider un ETL</strong>.<br>Lancer les tests, lire le rapport<br>et vérifier les unités.

<code>validate-etl/SKILL.md</code>

</div><div>

## Un agent personnalisé

Un rôle avec ses consignes<br>et ses outils autorisés.

Exemple : <strong>réviseur BI</strong>.<br>Relire le calcul et signaler<br>les défauts, sans modifier le code.

<code>reviseur-bi.agent.md</code>

</div></div>

<div class="strip">La recette décrit les étapes; le rôle définit la mission de revue.</div>

<div class="source">Skills et agents personnalisés [S07, S08]. Exemples présents dans .github/.</div>

<!--
2 minutes, 17:00–19:00. Comparer une consigne « ne remplace pas l’absence par zéro » à une procédure « lance les tests, vérifie le rapport et résume les preuves ». Skill se prononce comme le mot anglais et désigne ici cette recette réutilisable. Le réviseur BI est une configuration de l’agent, pas un autre collègue humain ni une garantie d’exactitude. Montrer les fichiers réels .github/skills/validate-etl/SKILL.md et .github/agents/reviseur-bi.agent.md. Le nom et la description servent à la découverte; les détails sont chargés selon le besoin. /validate-etl est une commande de chat, réservée ici au lancement volontaire par disable-model-invocation: true. Les différences Copilot/Local et anciens fichiers prompt.md restent dans l’aide-mémoire et l’annexe des réglages. Leur compatibilité doit être vérifiée dans le moteur utilisé. Ne pas introduire context: fork ou des sous-agents dans cette initiation.
-->

---

# Le contexte par paliers

<div class="small">Le contexte est l’information disponible pour accomplir la tâche.</div>

<ContextLayers />

<div class="source">Méthode pédagogique HydroDataLab fondée sur VS Code [S03, S05, S07]. Ce n’est pas un ordre de priorité des instructions.</div>

<!--
3 minutes, 19:00–22:00. Cliquer Contrat, Tâche, Preuves. La fréquence fixe le nombre attendu; le fuseau définit la journée; la provenance retrouve la mesure; la qualité indique ce qu’on peut comparer. Le socle reste court, les documents détaillés se consultent selon le besoin. Séparer dans le code extraction, transformations pures et export. Le PDF montre l’état initial et la chaîne entière. L’interactivité sert à révéler le détail nécessaire. Cette organisation est notre méthode pédagogique, pas une spécification Microsoft.
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

<div class="source">Contexte et facturation [S05, S06, S13]. Évaluer le coût de toutes les tentatives nécessaires.</div>

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

<div class="source">Documentation Auto consultée le 18 octobre 2026 [S14]. Politiques et abonnement restent déterminants.</div>

<!--
2 minutes, 24:00–26:00. Balance est notre défaut pédagogique, pas une règle institutionnelle. Les niveaux modifient une préférence de routage, pas une liste exclusive de modèles. Auto considère complexité et disponibilité, dans les politiques applicables. La remise n’implique pas une économie pour toute tâche face à n’importe quel modèle manuel. On peut voir le modèle utilisé dans les informations de la réponse. 1 AI credit représente 0,01 dollar US. Les anciens abonnements annuels individuels peuvent avoir un régime historique distinct du cas entreprise.
-->

---

# Choisir et comparer sans deviner

<ModelChooser />

<div class="source">Tarifs Copilot vérifiés le 18 octobre 2026 [S13]. Recommandations pédagogiques à évaluer sur vos tâches; ce n’est pas un benchmark.</div>

<!--
2 minutes, 26:00–28:00. Choisir Expliquer 30 lignes et Luna, puis Modifier un ETL et Sol. Augmenter de 1 à 3 tentatives. Le même volume est supposé pour isoler l’effet du tarif; les modèles réels ne consomment pas forcément autant. Le calcul exclut cache et remise Auto : ce n’est ni un relevé ni une prévision de facture. Sol est prévu pour le live si autorisé; Sonnet est une alternative à comparer. Opus se justifie sur des problèmes difficiles. Astra n’est pas proposé dans le parcours HQ. L’accessibilité clavier des contrôles permet la navigation sans souris.
-->

---

# Auto et HydraFusion : deux mécanismes

<div class="columns"><div>

## Auto choisit un modèle

Il sélectionne un modèle selon<br>la tâche, sa disponibilité<br>et les règles du compte.

</div><div>

## HydraFusion en combine

Plusieurs modèles contribuent<br>à une réponse, avec des étapes<br>de vérification et de correction.

</div></div>

<div class="strip">HydraFusion Max consacre plus d’effort aux tâches difficiles.<br>Il faut mesurer le coût total du travail effectué.</div>

<div class="source">HydraFusion et Max : version expérimentale (« research preview »), 14 octobre 2026 [S14, S15]. Disponibilité selon le compte.</div>

<!--
1 minute, 28:00–29:00. Reprendre la définition du modèle de la diapositive 4. Auto fait un choix; HydraFusion combine plusieurs contributions. Max vise un effort plus élevé. Ne pas promettre une économie systématique. L’annonce inclut notamment Opus 5.5, GPT-6 Astra, GPT-6.1 Sol et GPT-6 Luna selon le parcours. Astra n’est pas proposé dans notre sélection pédagogique; ne pas en déduire une politique officielle HQ sans confirmation. Cette capsule explique le concept; aucune activation n’est requise pour la formation. Un gain moyen de benchmark n’est pas une promesse pour notre ETL.
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

# Une spécification avant le code

<div class="small">Une spécification décrit le résultat attendu et ses règles.<br>Ici, la <strong>couverture</strong> est la part des créneaux attendus qui ont une mesure valide.</div>

| Exemple sur une série horaire | Critère de réussite |
|---|---|
| 24 mesures valides sur 24 attendues | 100 % |
| 23 mesures valides sur 24 attendues | 95,83 % |
| Valeur zéro, doublon ou absence | Zéro compte; doublon et absence n’ajoutent rien. |
| Changement d’heure | 23 ou 25 heures attendues selon la journée. |

<div class="strip">Ces exemples deviennent des tests exécutables avant de demander le code.</div>

<div class="source">Cas d’acceptation pédagogiques, pas des mesures HQ [S12, S16]. Journée locale : America/Toronto.</div>

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
0:00–0:40 : ouvrir la branche demo-live au jalon 06-specification, la spécification, les tests et la fonction Python à compléter. Le tableau de bord sait déjà afficher la couverture; elle est absente tant que le calcul n’est pas implémenté.
0:40–2:10 : choisir le modèle validé en répétition, coller le prompt live et limiter la modification à Python. Attendre le résultat réel des tests.
À 2:10 : si la génération n’est pas prête, annoncer « Je passe à la solution préparée pour garder le temps de vérifier. » Préserver le travail et basculer vers le dossier de secours 07-solution selon le guide.
2:10–3:00 : lire la comparaison avant/après (diff) et un test. Expliquer diff au moment de l’afficher.
3:00–4:00 : exécuter les cas complet et manquant. Le niveau horaire passe de 24/24 à 23/24 = 95,83 %. Déplier la qualité par série : la couverture globale 167/168 = 99,4 % utilise un autre dénominateur. Ne pas présenter la solution de secours comme une génération en direct.
-->

---

# Votre prochaine tâche Python

<div class="lead">Choisissez un calcul dont vous savez vérifier le résultat.<br>Écrivez un exemple attendu, puis confiez-le à l’agent.</div>

<div class="columns"><div>

## Apprendre pas à pas

[Microsoft Learn : agents](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/)<br>[Guides pratiques VS Code](https://code.visualstudio.com/docs/agents/best-practices)

</div><div>

## Exemples et échanges

[Awesome Copilot](https://awesome-copilot.github.com/)<br>[GitHub Community](https://github.com/community/community/discussions/categories/copilot?locale=en-US)<br>[Spec Kit : organiser les spécifications](https://github.github.com/spec-kit/)

</div></div>

<div class="strip">Quelle règle de vos analyses aimeriez-vous rendre vérifiable?</div>

<div class="source">Aide-mémoire et exercice participant fournis. Ressources [S06, S16, S17, S18].</div>

<!--
3 minutes, 42:00–45:00. Faire reformuler le geste : donner une règle claire, demander une proposition, lire les changements et vérifier un exemple. Proposer l’exercice Python du guide participant. Les personnes moins techniques peuvent écrire les critères et contrôler provenance et résultat sans connaître .NET. Microsoft Learn propose un apprentissage structuré; Awesome Copilot contient des exemples à adapter; GitHub Community permet de poser des questions; Spec Kit sera approfondi dans une autre formation. Questions de 45:00 à 55:00, marge jusqu’à 60:00. Les annexes restent disponibles sans être déroulées systématiquement.
-->

---
class: annex
---

# Annexe · les réglages utiles

| Réglage exact | Usage et réserve |
|---|---|
| `sessions.editor.wordWrap` | Retour à la ligne; expérimental |
| `sessions.layout.singlePaneDetailPanel` | Panneau intégré; recharger |
| `chat.useCustomizationsInParentRepositories` | Depuis un sous-dossier; défaut false |
| `github.copilot.chat.cli.customAgents.enabled` | Agents Copilot; vérifier la valeur |
| `chat.tools.compressOutput.enabled` | Sorties compressées; Preview |
| `chat.useNestedAgentsMdFiles` | Local; expérimental, défaut false |

<div class="strip compact">Les anciens <code>chat.*FilesLocations</code> ne configurent pas tous le nouveau moteur Copilot.</div>

<div class="source">Référence VS Code, 14 octobre 2026 [S04]. Vérifier portée et disponibilité dans la version installée.</div>

<!--
Annexe hors des 45 minutes. Ne pas copier tout ce tableau dans settings.json. Les options expérimentales peuvent changer. La fenêtre partage le profil et les paramètres VS Code; vérifier la portée dans l’éditeur. Les permissions ne sont pas un mécanisme d’économie de tokens. Respecter les politiques. Pour custom agents, vérifier cli.customAgents avant de promettre un agent dans le sélecteur. Pour les règles, vérifier moteur, dossier, fichier et références.
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

<div class="source">Tarifs GitHub Copilot au 18 octobre 2026 [S13]. Pas une garantie de disponibilité HQ ni un classement de qualité.</div>

<!--
Annexe. Tarifs GitHub Copilot, pas extrapolation d’une API fournisseur. Lecture et écriture du cache sont des catégories distinctes; ne pas les additionner arbitrairement sur tous les tokens. Les seuils modifient la grille. Voir matrice-modeles.md pour les paliers longs et la simulation. Les coûts d’infrastructure de revue cloud ne sont pas modélisés. Les rôles suggérés se mesurent avec exactitude, reprises, temps et crédits. Le coût réel dépend de l’usage et de son contrat de facturation.
-->

---
class: annex
---

# Annexe · une skill de validation

```yaml
---
name: validate-etl
description: Vérifier une transformation Python et son rapport.
disable-model-invocation: true
---
```

1. Lire les critères et le contrat concernés.
2. Exécuter les tests ciblés et les contrôles prévus.
3. Vérifier unités, couverture et provenance.
4. Résumer les preuves et ce qui reste incertain.

<div class="source">Exemple pédagogique; commandes à aligner sur le dépôt. Format et déclenchement [S07, S09].</div>

<!--
Annexe. Le nom correspond au dossier. disable-model-invocation réserve le lancement à l’utilisateur. /validate-etl apparaît dans une session qui découvre le fichier. Une consigne de validation ne prouve pas que les tests ont tourné; montrer la sortie. Conserver scripts et références avec le skill dans Git. La conversion d’un prompt peut perdre un choix explicite de modèle ou outils : vérifier la migration, ne pas présumer l’équivalence.
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


