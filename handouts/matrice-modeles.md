# Matrice de modèles — GitHub Copilot

**Vérifiée le 18 octobre 2026** dans [GitHub Models and pricing](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing). Disponibilité réelle à vérifier dans le sélecteur et les politiques HQ. Les usages proposés ci-dessous sont des recommandations pédagogiques, pas des résultats de benchmark.

## Choisir par tâche

| Situation | Point de départ | Ce qui décide de conserver le choix |
|---|---|---|
| Usage quotidien sans besoin de fixer un modèle | Auto Balance | Résultat, temps et crédits sur vos tâches |
| Expliquer une petite fonction, changement mécanique | Auto Efficiency, GPT-6 Luna ou Claude Haiku 5.5 | Exactitude et absence de reprises inutiles |
| Fonctionnalité Python délimitée, avec contrat et tests | Claude Sonnet 5.5; comparer Sol au même tarif | Tests et respect du périmètre |
| Changements complexes entre composants | GPT-6.1 Sol | Cohérence des interfaces et validations ciblées |
| Diagnostic qui reste difficile | Auto Intelligence ou Claude Opus 5.5 | Décisions justifiées, défaut reproduit, gain de qualité |
| Génération live de cette formation | GPT-6.1 Sol, si autorisé | Répétition réussie et solution de secours |

Astra n’est pas proposé dans ce parcours HQ. Une tâche bien bornée peut réussir avec un modèle moins cher; un modèle plus capable peut éviter des tentatives coûteuses. Ne pas choisir uniquement au tarif par token.

## Tarifs standards

Dollars US par **million de tokens**. Colonnes distinctes; les catégories de cache ne s’appliquent pas toutes aux mêmes tokens.

| Modèle | Entrée | Entrée cachée | Écriture cache | Sortie | Seuil standard |
|---|---:|---:|---:|---:|---|
| GPT-6 Luna | 0,10 | 0,01 | 0,125 | 0,50 | ≤272k tokens d’entrée |
| Claude Haiku 5.5 | 0,10 | 0,01 | 0,125 | 0,50 | ≤100k |
| GPT-6.1 Sol | 2,00 | 0,10 | 2,50 | 10,00 | ≤272k |
| Claude Sonnet 5.5 | 2,00 | 0,10 | 2,50 | 10,00 | Pas de seuil distinct indiqué |
| Claude Opus 5.5 | 4,00 | 0,20 | 5,00 | 20,00 | Pas de seuil distinct indiqué |

| Palier long contexte | Entrée | Entrée cachée | Écriture cache | Sortie |
|---|---:|---:|---:|---:|
| GPT-6 Luna, >272k | 0,20 | 0,02 | 0,25 | 0,75 |
| Claude Haiku 5.5, >100k | 0,50 | 0,05 | 0,625 | 2,50 |
| GPT-6.1 Sol, >272k | 4,00 | 0,20 | 5,00 | 15,00 |

1 AI credit = **0,01 $ US**. Les forfaits, crédits inclus et politiques budgétaires restent ceux du compte/organisation. Certains anciens abonnements annuels individuels ont encore un régime historique de requêtes; ne pas transposer leurs multiplicateurs au cas d’entreprise actuel.

## Graphique de coûts par type de tâche

**Scénarios fictifs**, sans volume réel mesuré, sans cache ni remise Auto. Les volumes sont des hypothèses de comparaison, pas des consommations typiques certifiées. Sortie = tokens facturables, raisonnement compris lorsqu’il est facturé. Formule par essai : `(entrée × tarif entrée + sortie × tarif sortie) / 1 000 000`.

| Tâche illustrative | Entrée / sortie supposées | Luna ou Haiku | Sonnet ou Sol | Opus | Choix à essayer |
|---|---:|---:|---:|---:|---|
| Expliquer une fonction | 3 000 / 500 | 0,00055 $ | 0,011 $ | 0,022 $ | Luna ou Haiku |
| Modifier un ETL testé | 20 000 / 2 000 | 0,003 $ | 0,060 $ | 0,120 $ | Sonnet |
| Changer plusieurs composants | 60 000 / 8 000 | 0,010 $ | 0,200 $ | 0,400 $ | Sol |
| Diagnostiquer une anomalie difficile | 60 000 / 8 000 | 0,010 $ | 0,200 $ | 0,400 $ | Opus si justifié |

La diapositive 17 montre **les quatre tâches en colonnes et les cinq modèles en lignes, simultanément**. Lire une colonne pour comparer les modèles d’une tâche, une ligne pour comparer les tâches avec un modèle. Montants en dollars US. Multiplier par le nombre d’essais; multiplier les dollars par 100 pour obtenir les crédits. Par exemple, trois essais du scénario ETL avec Sonnet coûtent 0,180 $ = 18 crédits sous ces hypothèses. Le graphique garde un axe linéaire partant de zéro, plafonné à 0,40 $ par essai pour tous les scénarios. Les losanges signalent les recommandations pédagogiques, sans score de qualité inventé.

Le volume identique entre modèles d’un scénario isole l’effet du tarif. En réel, appels, raisonnement, cache et reprises diffèrent. Deux modèles au même tarif peuvent produire des résultats différents. Auto Balance reste le défaut conseillé; on ne lui attribue pas un prix fixe puisqu’il route les requêtes. Valider les choix sur quelques tâches représentatives : exactitude, tests, temps, reprises et crédits totaux. [Comparaison officielle des modèles](https://docs.github.com/en/copilot/reference/ai-models/model-comparison).

## Thinking effort : effort de raisonnement

Ajout vérifié le **19 octobre 2026**. Ce paramètre module le raisonnement consacré à une requête. Commencer par le réglage recommandé; le raisonnement adaptatif ajuste l’effort lorsqu’il est disponible. Dans le sélecteur, ouvrir la flèche du modèle puis **Thinking Effort**. Les niveaux proposés varient : Low, Medium et High ne sont pas un menu universel, ni Medium un défaut universel. Auto choisit un modèle; ce réglage concerne son effort. [Microsoft : configuration](https://code.visualstudio.com/docs/agent-customization/language-models#configure-thinking-effort).

Exemples pédagogiques à tester : effort faible pour une correction mécanique; intermédiaire pour une modification délimitée; élevé pour un diagnostic complexe. Davantage de raisonnement consomme plus de tokens; cela ne remplace pas un contexte clair et des tests. Noter modèle, effort, résultat, nombre de reprises, durée et crédits pour quelques tâches représentatives.

## Coût par tâche et benchmarks de code

La diapositive **18** complète les montants simulés de la 17. Le **coût du résultat validé** additionne les appels et les reprises nécessaires. Exemple fictif : trois essais à 0,10 $ totalisent 0,30 $; un essai réussi à 0,20 $ est moins cher. Évaluer aussi le temps de validation humaine.

[Artificial Analysis — Coding Agents](https://artificialanalysis.ai/agents/coding-agents) propose le graphique **Coding Agent Index vs. Cost per Task** : score de code vers le haut, coût API moyen par tâche vers la droite, sur un **axe logarithmique**. La slide contient maintenant une [vraie capture locale du graphique](../presentation/public/aa-cost-effort-20261019.png), filtrée sur les six configurations suivantes. Les infobulles publiques ont été lues le **19 octobre 2026**, heure de Montréal; les indices ci-dessous sont arrondis tels qu’affichés.

| Modèle | Harnais testé | Effort | Indice v1.5 affiché | Coût API moyen / tâche ($ US) |
|---|---|---|---:|---:|
| GPT‑6.1 Sol | Codex | Low — faible | 57 | 0,50 |
| GPT‑6.1 Sol | Codex | High — élevé | 60 | 0,89 |
| GPT‑6.1 Sol | Codex | Max — maximal | 60 | 1,55 |
| Sonnet 5.5 | Claude Code | Low — faible | 42 | 0,48 |
| Sonnet 5.5 | Claude Code | High — élevé | 55 | 1,24 |
| Sonnet 5.5 | Claude Code | Max — maximal | 68 | 14,19 |

Trois lectures concrètes, **calculées ici à partir des valeurs affichées** :

- **Sol, High → Max :** coût +74 % environ, indice affiché identique (60). Tester High avant Max peut être pertinent; les arrondis ne prouvent pas l’égalité exacte des performances.
- **Sonnet, High → Max :** indice +13 points, coût ×11,4 environ. Pour un diagnostic complexe, mesurer si le gain justifie le surcoût; ne pas appliquer Max systématiquement.
- **Budget proche :** Sol Low coûte 0,50 $ pour 57 points; Sonnet Low, 0,48 $ pour 42. Les harnais diffèrent aussi : le résultat compare des configurations, sans isoler l’effet du seul modèle.

Ces exemples sont une présélection à vérifier sur vos tâches, pas une garantie dans Copilot. Les libellés d’effort n’assurent pas le même budget entre fournisseurs. Preuves : [relevé des six infobulles](../docs/screenshots/artificial-analysis/readings.json), captures voisines et [paramètres du cadrage](../docs/screenshots/artificial-analysis/capture-settings.json). Les filtres et le réglage de police viennent du site; les pixels et valeurs du graphique ne sont pas retouchés.

Le coût affiché tient compte des tarifs API et du cache applicable. Il ne mesure ni la facture Copilot ni un coût par réussite garantie; l’indice agrège plusieurs évaluations. Lire la [méthodologie](https://artificialanalysis.ai/methodology/coding-agents-benchmarking) et la date des résultats. La capture fige cet instantané pour la séance; le site et les modèles autorisés dans HQ peuvent évoluer.

## Auto et HydraFusion

[Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection) propose Efficiency, Balance et Intelligence dans VS Code. Le routage considère tâche, disponibilité et politiques; les modèles autorisés peuvent évoluer. Les plans payants bénéficient d’une remise annoncée de 10 % sur le coût des modèles en Auto. Cela ne garantit pas qu’une tâche coûtera moins qu’avec un choix manuel donné.

[HydraFusion, documentation](https://docs.github.com/en/early-access/copilot/hydrafusion) choisit un parcours Single (un modèle), Cascade (proposition puis jugement avec escalade possible) ou Critique (proposition, revue et révision). Les modèles utilisés sont facturés à leur tarif standard, sans supplément HydraFusion et **sans remise Auto**. Davantage de passes peut augmenter le coût. Le pool varie et respecte les modèles disponibles et autorisés; il ne se sélectionne pas manuellement. [Max, annonce du 14 octobre](https://github.blog/changelog/2026-10-14-hydrafusion-updates-and-new-max-option/) augmente l’effort sur les problèmes exigeants. Les deux options restent en research preview et ne sont pas nécessaires à notre démo.

Avant la séance du 20 octobre : rouvrir les sources et vérifier le sélecteur du portable. Les données de cette fiche sont datées, pas figées contractuellement.
