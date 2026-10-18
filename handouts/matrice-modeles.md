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

Montants en dollars US. Multiplier par le nombre d’essais; multiplier les dollars par 100 pour obtenir les crédits. Par exemple, trois essais du scénario ETL avec Sonnet coûtent 0,180 $ = 18 crédits sous ces hypothèses. Le graphique garde un axe linéaire partant de zéro, plafonné à 0,40 $ par essai pour tous les scénarios. Les losanges signalent les recommandations pédagogiques, sans score de qualité inventé.

Le volume identique entre modèles d’un scénario isole l’effet du tarif. En réel, appels, raisonnement, cache et reprises diffèrent. Deux modèles au même tarif peuvent produire des résultats différents. Auto Balance reste le défaut conseillé; on ne lui attribue pas un prix fixe puisqu’il route les requêtes. Valider les choix sur quelques tâches représentatives : exactitude, tests, temps, reprises et crédits totaux. [Comparaison officielle des modèles](https://docs.github.com/en/copilot/reference/ai-models/model-comparison).

## Auto et HydraFusion

[Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection) propose Efficiency, Balance et Intelligence dans VS Code. Le routage considère tâche, disponibilité et politiques; les modèles autorisés peuvent évoluer. Les plans payants bénéficient d’une remise annoncée de 10 % sur le coût des modèles en Auto. Cela ne garantit pas qu’une tâche coûtera moins qu’avec un choix manuel donné.

[HydraFusion, documentation](https://docs.github.com/en/early-access/copilot/hydrafusion) choisit un parcours Single (un modèle), Cascade (proposition puis jugement avec escalade possible) ou Critique (proposition, revue et révision). Les modèles utilisés sont facturés à leur tarif standard, sans supplément HydraFusion et **sans remise Auto**. Davantage de passes peut augmenter le coût. Le pool varie et respecte les modèles disponibles et autorisés; il ne se sélectionne pas manuellement. [Max, annonce du 14 octobre](https://github.blog/changelog/2026-10-14-hydrafusion-updates-and-new-max-option/) augmente l’effort sur les problèmes exigeants. Les deux options restent en research preview et ne sont pas nécessaires à notre démo.

Avant la séance du 20 octobre : rouvrir les sources et vérifier le sélecteur du portable. Les données de cette fiche sont datées, pas figées contractuellement.
