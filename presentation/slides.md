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

# Une heure, un fil conducteur

<div class="columns"><div>

## 32 minutes pour comprendre

Fenêtre Agents et instructions<br>Contexte, tokens et modèles<br>Spécifications et bonnes pratiques

</div><div>

## 13 minutes pour constater

Une session bien cadrée · **4 min**<br>Un pipeline observable · **5 min**<br>Une règle Python vérifiée · **4 min**

</div></div>

<div class="strip">10 minutes de questions + 5 minutes de marge.</div>

<!--
1 minute, 01:00–02:00. Les démonstrations sont réparties dans le récit, pas ajoutées après les 45 minutes. Les annexes 19 à 24 servent aux questions. Aux gestionnaires : résultat, preuve et coût de reprise. Aux analystes : schéma des données, règle métier et tests. Les services seront déjà démarrés et les dépendances installées : la séance ne dépend pas d’une installation en direct. Les six sujets initiaux sont couverts; spec-driven reste un survol.
-->

---

# Quatre séries, quatre sens différents

| Série | Unité | Ce que l’on observe |
|---|---:|---|
| Demande électrique au Québec | MW | Une puissance demandée |
| Niveau d’eau au réservoir Gouin | m | Une hauteur dans le référentiel source |
| Température minimale | °C | Un minimum sur la période source |
| Température maximale | °C | Un maximum sur la période source |

<div class="strip">Un niveau en mètres ne donne pas une réserve en GWh.</div>

<div class="source">Sources publiques HQ et contrat [S11, S12]. Instantané : 17 octobre 2026, 168 observations; récupéré le 18 octobre à 19:44 UTC.</div>

<!--
2 minutes, 02:00–04:00. Les fréquences diffèrent : demande à 15 minutes, niveau horaire, minima/maxima selon la définition source. intervalMinutes porte cette information; ne pas attendre 24 valeurs partout. Ne pas convertir un niveau en réserve ni une puissance instantanée en énergie. Le manifeste confirme 168 observations pour le 17 octobre, récupérées le 18 octobre à 19:44:01 UTC. Les températures proviennent de leur station source, distincte du capteur de niveau Gouin. Le cas explore qualité et provenance, pas les opérations hydriques.
-->

---

# L’agent travaille en boucle

<div class="flow"><b>Comprendre</b><span>→</span><b>Agir</b><span>→</span><b>Vérifier</b><span>→</span><b>Corriger</b></div>

<div class="columns"><div>

## Vous fournissez

L’objectif et les contraintes.<br>Une définition de « terminé ».

</div><div>

## Il utilise

Un modèle, des outils et du contexte.<br>Des résultats à examiner.

</div></div>

<div v-click class="strip">« Ajoute un indicateur » devient « produis un indicateur que ces tests valident ».</div>

<div class="source">Contexte et bonnes pratiques VS Code [S05, S06]. Schéma de fonctionnement, non capture de l’interface.</div>

<!--
2 minutes, 04:00–06:00. Distinguer le modèle du moteur d’exécution : le moteur assemble le contexte et donne accès aux outils. Une réponse convaincante n’est pas une preuve d’exécution. Pointer tests, fichiers et journaux. L’autonomie n’efface pas le choix des permissions. Une tâche indépendante mérite une nouvelle session. Les sous-agents peuvent isoler un raisonnement; ils ne créent pas automatiquement un worktree ni un environnement isolé. L’historique du chat n’est pas la mémoire permanente du projet.
-->

---

# La fenêtre Agents, en cinq repères

<div class="flow"><b>Dépôt</b><span>→</span><b>Session</b><span>→</span><b>Mode</b><span>→</span><b>Modèle</b><span>→</span><b>Résultat</b></div>

- Ouvrir **Chat: Open Agents Window** ou `code --agents`.
- Vérifier dossier, branche et worktree actifs.
- Choisir le moteur **Copilot**, puis le mode adapté.
- Examiner **Changes** et les résultats des outils.

<div class="strip">Chat accompagne le code. La fenêtre Agents organise les sessions.</div>

<div class="source">Fenêtre Agents et configuration, 14 octobre 2026 [S01, S02]. Schéma fonctionnel, non capture VS Code.</div>

<!--
2 minutes, 06:00–08:00. Employer moteur puis donner le terme harness une fois. Choisir un modèle OpenAI dans Copilot n’est pas passer au moteur Codex. La session active détermine le dépôt visible. Un worktree est une autre copie de travail Git; ce n’est pas une sauvegarde à lui seul. Les libellés dépendent de la version installée. Montrer l’interface réelle du portable si disponible; cette diapositive est une carte des concepts, pas un faux screenshot.
-->

---

# Démo 1 · cadrer la session

<div class="columns"><div>

## Avant le premier prompt

Copilot · Auto Balance<br>Permissions manuelles<br>Dossier et branche vérifiés

</div><div>

## Personnaliser pour lire

`Ctrl+,` : portée du réglage<br>Zoom et retour à la ligne<br>**Chat: Open Customizations**

</div></div>

<div class="strip">Retrouver une instruction, puis vérifier les <strong>References</strong> de la réponse.</div>

<div class="source">Réglages et découverte [S02, S03, S04]. Démo : 4 minutes; explication : 1 minute.</div>

<!--
1 minute d’explication puis 4 minutes de démo, 08:00–13:00. Dépôt déjà ouvert. Vérifier Copilot et Auto Balance; respecter les politiques de l’organisation. Ctrl+, rechercher wordWrap et montrer la portée sans modifier les paramètres globaux. Ouvrir Chat: Open Customizations, Instructions. Lancer le prompt de découverte de handouts/prompts.md. Vérifier une instruction dans References. Préciser : découverte ne signifie pas respect; examiner une réponse concrète. Si l’authentification échoue, montrer les fichiers locaux et le schéma précédent au lieu de dépenser le temps à se reconnecter.
-->

---

# Les règles qui survivent au chat

| Besoin | Emplacement dans le dépôt |
|---|---|
| Socle commun | `.github/copilot-instructions.md` |
| Règle Python ciblée | `.github/instructions/*.instructions.md` |
| Procédure à réutiliser | `.github/skills/<nom>/SKILL.md` |
| Rôle spécialisé | `.github/agents/<nom>.agent.md` |

<div class="strip">Écrire les décisions difficiles à déduire du code.</div>

<div class="source">Instructions, skills et agents [S03, S07, S08]. AGENTS.md est également pris en charge par des moteurs compatibles.</div>

<!--
2 minutes, 13:00–15:00. Socle : « Les données brutes restent intactes; les transformations écrivent en sortie. » Règle ciblée : « Null est absent; zéro est valide. » AGENTS.md partage des consignes entre agents compatibles, mais découverte et activation restent propres au moteur. Éviter deux longues copies contradictoires. Préférences personnelles Copilot : ~/.copilot/copilot-instructions.md. Les consignes d’organisation sont aussi applicables. Versionner et relire comme du code. Ces instructions du chat ne gouvernent pas les suggestions inline.
-->

---

# Une règle au bon endroit

```yaml
# .github/instructions/python.instructions.md
---
description: Transformation et qualité des séries Python
applyTo: "etl/**/*.py"
---
```

<div class="lead">« Compter les instants valides et distincts.<br>Zéro est valide; une absence reste absente. »</div>

<div v-click class="reveal">Découvrir → vérifier les références → tester le comportement.</div>

<div class="source">Instructions ciblées [S03]. Sources additives : ne pas compter sur une priorité universelle pour résoudre les contradictions.</div>

<!--
2 minutes, 15:00–17:00. applyTo vise les fichiers Python sous etl. La description aide à reconnaître une tâche pertinente. L’instruction n’est pas une règle déterministe exécutée par Python : les tests restent nécessaires. Ne pas enseigner « le fichier le plus proche gagne » universellement. Aligner les sources contradictoires. Diagnostiquer moteur, emplacement puis References. Developer: Open Agent Debug Logs permet une inspection approfondie. Un lien Markdown aide à retrouver une référence; il ne garantit pas l’injection automatique de tout le document.
-->

---

# Une procédure n’est pas une préférence

<div class="columns"><div>

## Skill : comment faire

`validate-weather-change/SKILL.md`

1. Exécuter les tests Python.
2. Examiner rapport et unités.
3. Résumer preuves et limites.

</div><div>

## Agent : avec quel rôle

`reviewer.agent.md`

Relire contrat et qualité.<br>Instructions et outils du rôle.<br>Modèle optionnel.

</div></div>

<div class="strip compact">Copilot : skills. Local : les fichiers <code>*.prompt.md</code> restent disponibles.</div>

<div class="source">Skills, agents et migration [S07, S08, S09]. Vérifier le moteur et la découverte avant la séance.</div>

<!--
2 minutes, 17:00–19:00. Le nom et la description du skill servent à la découverte; ses instructions et ressources se chargent selon le besoin. Pour un déclenchement volontaire, utiliser disable-model-invocation: true et une slash commande. Le nouveau moteur Copilot ne charge pas prompt.md. La migration n’emporte pas automatiquement les choix modèle, agent et outils de l’ancien prompt. Le reviewer est un rôle, pas une garantie de qualité. Examiner ses constats. context: fork est expérimental; ne pas en faire une dépendance de cette initiation.
-->

---

# Le contexte par paliers

<ContextLayers />

<div class="source">Méthode pédagogique HydroDataLab fondée sur VS Code [S03, S05, S07]. Ce n’est pas un ordre de priorité des instructions.</div>

<!--
3 minutes, 19:00–22:00. Cliquer Contrat, Tâche, Preuves. La fréquence fixe le nombre attendu; le fuseau définit la journée; la provenance retrouve la mesure; la qualité indique ce qu’on peut comparer. Le socle reste court, les documents détaillés se consultent selon le besoin. Séparer dans le code extraction, transformations pures et export. Le PDF montre l’état initial et la chaîne entière. L’interactivité sert à révéler le détail nécessaire. Cette organisation est notre méthode pédagogique, pas une spécification Microsoft.
-->

---

# Réduire le travail à refaire

<div class="columns"><div>

## Ce qui utilise des tokens

Instructions et historique<br>Fichiers et sorties d’outils<br>Réponse et raisonnement facturé

</div><div>

## Ce qui aide la qualité

Une tâche bornée et des tests<br>Les fichiers pertinents<br>Des journaux ciblés et une synthèse

</div></div>

<div v-click class="strip">Optimiser le coût d’une tâche réussie, pas seulement celui d’un appel.</div>

<div class="source">Contexte, pratiques et tarifs [S05, S06, S13]. Nouveau sujet : nouvelle session. /compact résume mais peut perdre des détails.</div>

<!--
2 minutes, 22:00–24:00. Un petit prompt peut déclencher beaucoup de lectures et d’outils. Donner contrat, fonction et tests réduit la recherche inutile. Les retries peuvent annuler le gain d’un modèle moins cher. Le cache change le coût, sans garantir un comportement identique entre sessions. Demander des preuves synthétiques plutôt que tous les logs. Git, une spec et un état d’avancement diminuent aussi le coût d’une reprise. Une nouvelle session ne récupère pas magiquement le contenu d’un autre chat. Ne pas prétendre que toutes les formes de raisonnement occupent la même place dans le contexte visible.
-->

---

# Auto : un bon point de départ

| Niveau | Priorité | Exemple BI |
|---|---|---|
| Efficiency | Coût | Expliquer une fonction courte |
| **Balance** | Coût, qualité et latence | Transformer une série avec tests |
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

# HydraFusion : une expérience à distinguer

<div class="lead">Plusieurs modèles peuvent contribuer à une seule réponse.</div>

<div class="columns"><div>

## HydraFusion

Orchestration équilibrée.<br>Vérification et réparation selon le workflow.

</div><div>

## HydraFusion Max

Plus d’effort sur les tâches difficiles.<br>Coût total à mesurer.

</div></div>

<div class="strip">Research preview. L’économie n’est pas garantie pour chaque tâche.</div>

<div class="source">GitHub, 14 octobre 2026 [S15]. Capsule facultative; ne pas activer sans disponibilité conforme aux politiques HQ.</div>

<!--
1 minute, 28:00–29:00. Auto route; HydraFusion orchestre plusieurs contributions. L’annonce inclut un pool Opus 5.5, GPT-6 Astra, GPT-6.1 Sol et GPT-6 Luna selon le workflow. HQ excluant Astra du parcours, ne pas supposer disponibilité ni contrôle manuel du pool. Montrer le concept ou une capture authentique datée et autorisée, pas une exécution improvisée. Max vise un raisonnement plus approfondi. Ne pas transposer un gain moyen de benchmark en promesse pour l’ETL. C’est une piste à évaluer, pas une dépendance de la démo.
-->

---
class: breakwater
---

# Démo 2 · du fichier à l’indicateur

<div class="flow"><b>Source HQ</b><span>→</span><b>Python</b><span>→</span><b>API .NET</b><span>→</span><b>Vue</b></div>

<div class="lead">Extraire. Transformer. Exporter.<br>Voir le résultat et retrouver sa provenance.</div>

<div class="strip">Quartz pilote les tâches; SQLite conserve l’historique.<br>Aspire rend les services observables.</div>

<div class="source">Architecture HydroDataLab [S12]. CLI et FastAPI partagent le moteur Python. CLI natif et Docker; Aspire 13.6.1; pile Compose préparée séparément.</div>

<!--
1 minute d’explication puis 5 minutes de démo, 29:00–35:00. Exécuter la CLI fixture déjà répétée. Montrer CSV, Parquet et JSON en sortie. Dans Vue, lancer fixture puis suivre queued/running/succeeded et provenance. Dans Aspire, montrer ressources et un log. Simuler un échec avec le contrôle prévu, relancer et constater l’historique. La planification reste désactivée initialement. Le live réseau est facultatif; le snapshot local est le chemin fiable. Quartz ordonnance, notre Vue fournit le tableau de suivi. Aspire n’est pas l’ordonnanceur métier. De 4:30 à 5:00, exécuter la commande Docker CLI préremplie du guide présentateur, image chaude et réseau coupé. Le résumé doit retrouver les mêmes valeurs que le natif. La pile Compose complète a été préparée séparément et ne se relance pas dans ces cinq minutes. Arrêter la démonstration après 5 minutes.
-->

---

# La mini-spec rend la qualité observable

<div class="lead">Calculer la couverture d’une série sur une journée locale.</div>

| Cas horaire | Résultat attendu |
|---|---|
| 24 instants valides sur 24 | 100 % |
| 23 instants valides sur 24 | 95,83 % |
| Zéro, doublon ou absence | Zéro compte; les autres ne gonflent pas le total |
| Changement d’heure | 23 ou 25 heures attendues selon la journée |

<div class="source">Exemples de tests, pas des mesures HQ. Instants UTC; journée America/Toronto, fin exclusive [S12, S16].</div>

<!--
3 minutes, 35:00–38:00. observed compte les instants attendus distincts avec valeur valide. expected vient du calendrier local et de la fréquence, pas du nombre de lignes reçues. Le tableau est horaire; la demande à 15 minutes attend 96 points, ou 92/100 aux jours de 23/25 heures. Les séries quotidiennes gardent leur grain. Zéro n’est pas absent. Doublon ne double pas la couverture; conflit à signaler. Écrire ces critères avant le code. Le survol spec-driven : intention, critères, plan, tâches, code, preuves. Spec Kit aide à formaliser; sa formation complète vient ultérieurement.
-->

---
class: breakwater
---

# Démo 3 · quatre minutes de Python

<div class="lead">Une fonction à modifier.<br>Des tests déjà prêts.<br>Un résultat visible dans Vue.</div>

<div class="strip">0:40 : lancer l’agent<br>2:10 : basculer vers la solution préparée si nécessaire<br>3:00 : montrer la couverture et le scénario manquant</div>

<div class="source">Checkpoint 06-specification → 07-solution. La solution préparée est annoncée comme telle, jamais comme une génération en direct.</div>

<!--
4 minutes de démo, 38:00–42:00. Ouvrir 06-specification dans le dossier prévu. Lire les critères en 40 secondes. Choisir GPT-6.1 Sol si disponible, sinon le modèle validé en répétition. Coller le prompt live de handouts/prompts.md. Limiter la modification à la fonction Python et aux éléments strictement nécessaires. À 2:10, si le code n’est pas prêt, annoncer le passage à la solution préparée. Utiliser le mécanisme de checkpoint du dépôt en préservant le travail. Afficher la couverture complète puis l’exemple horaire à 95,83 % après retrait en mémoire. Les originaux restent intacts. Lire le diff et un test avant de conclure.
-->

---

# Votre prochaine tâche : petite et vérifiable

<div class="lead">Une règle de données.<br>Une spec courte.<br>Une preuve reproductible.</div>

<div class="columns compact"><div>

## Pour pratiquer

[Microsoft Learn : agent mode](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/)<br>[Guides VS Code](https://code.visualstudio.com/docs/agents/best-practices)

</div><div>

## Pour progresser ensemble

[Awesome Copilot](https://awesome-copilot.github.com/)<br>[GitHub Community](https://github.com/community/community/discussions/categories/copilot?locale=en-US)<br>[GitHub Spec Kit](https://github.github.com/spec-kit/)

</div></div>

<div class="source">Fiches, prompts et matrice dans handouts/. Questions : 10 minutes; marge : 5 minutes. Sources [S06, S16, S17, S18].</div>

<!--
3 minutes, 42:00–45:00. Faire reformuler : « Je donne un contrat clair et demande une preuve ». Proposer l’exercice Python du guide participant. Les personnes moins techniques rédigent les critères et vérifient provenance et résultat sans devoir comprendre .NET. Ajouter une règle après une erreur récurrente et une skill réellement utile. Relire les exemples communautaires avant de les adopter. Questions 45:00–55:00; marge 55:00–60:00. Ne pas dérouler les annexes systématiquement.
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
name: validate-weather-change
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
Annexe. Le nom correspond au dossier. disable-model-invocation réserve le lancement à l’utilisateur. /validate-weather-change apparaît dans une session qui découvre le fichier. Une consigne de validation ne prouve pas que les tests ont tourné; montrer la sortie. Conserver scripts et références avec le skill dans Git. La conversion d’un prompt peut perdre un choix explicite de modèle ou outils : vérifier la migration, ne pas présumer l’équivalence.
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

- Arrêter les services avant de changer de checkpoint.
- Préserver le travail; utiliser le mécanisme du dépôt.
- Garder un runtime distinct par étape.
- Reprendre avec l’instantané local et les tests.

<div class="strip">PDF, solution préparée et Git pour reprendre.</div>

<div class="source">Guide présentateur et runbook du projet. Les checkpoints VS Code n’annulent pas les effets externes des outils [S06].</div>

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

<div class="strip compact">Vérifié le 18 octobre 2026. Revalider modèles, politiques et snapshot avant le 20 octobre.</div>

<div class="source">Registre complet : docs/SOURCES.md. Schémas pédagogiques; aucune capture VS Code inventée.</div>

<!--
Annexe de références. Les fiches locales reprennent les liens essentiels. Les fonctionnalités sont marquées expérimentales ou preview selon leur source. La date de vérification n’est pas une garantie sur toute version installée. Confirmer politiques HQ et modèles sur le portable. Les documents techniques du dépôt définissent les commandes finales. Aucun lien externe n’est requis pour afficher le support après build : polices et composants sont locaux. Le PDF fige les vues interactives; utiliser le web pour explorer.
-->


