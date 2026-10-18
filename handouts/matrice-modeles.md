# Matrice de modèles — GitHub Copilot

**Vérifiée le 18 octobre 2026** dans [GitHub Models and pricing](https://docs.github.com/en/copilot/reference/copilot-billing/models-and-pricing). Disponibilité réelle à vérifier dans le sélecteur et les politiques HQ. Les usages proposés ci-dessous sont des recommandations pédagogiques, pas des résultats de benchmark.

## Choisir par tâche

| Situation | Point de départ | Ce qui décide de conserver le choix |
|---|---|---|
| Usage quotidien sans besoin de fixer un modèle | Auto Balance | Résultat, temps et crédits sur vos tâches |
| Expliquer une petite fonction, changement mécanique | Auto Efficiency, GPT-6 Luna ou Claude Haiku 5.5 | Exactitude et absence de reprises inutiles |
| Transformation Python avec contrat et tests | GPT-6.1 Sol ou Claude Sonnet 5.5 | Tests et respect du périmètre |
| Plan complexe, investigation difficile | Auto Intelligence ou Claude Opus 5.5 | Décisions justifiées, défaut reproduit, gain de qualité |
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

## Simulation de la diapositive interactive

Hypothèse volontairement simplifiée : **20 000 tokens d’entrée et 2 000 de sortie par tentative**, pas de cache, pas de remise Auto. Aucun volume réel mesuré. Formule : `(entrée × tarif entrée + sortie × tarif sortie) / 1 000 000`.

| Modèle | Une tentative | Trois tentatives |
|---|---:|---:|
| Luna ou Haiku, tarif standard | 0,003 $ = 0,3 crédit | 0,009 $ = 0,9 crédit |
| Sol ou Sonnet, tarif standard | 0,060 $ = 6 crédits | 0,180 $ = 18 crédits |
| Opus | 0,120 $ = 12 crédits | 0,360 $ = 36 crédits |

Le volume identique sert à isoler l’effet du tarif. En réel, nombre d’appels, tokens de raisonnement/sortie, cache et reprises diffèrent. Ce tableau ne prédit pas une facture et ne compare pas la qualité.

## Auto et HydraFusion

[Auto](https://docs.github.com/en/copilot/concepts/models/auto-model-selection) propose Efficiency, Balance et Intelligence dans VS Code. Le routage considère tâche, disponibilité et politiques; les modèles autorisés peuvent évoluer. Les plans payants bénéficient d’une remise annoncée de 10 % sur le coût des modèles en Auto. Cela ne garantit pas qu’une tâche coûtera moins qu’avec un choix manuel donné.

[HydraFusion et Max, annonce du 14 octobre](https://github.blog/changelog/2026-10-14-hydrafusion-updates-and-new-max-option/) restent en research preview. Ils orchestrent plusieurs modèles; Max augmente l’effort sur les problèmes exigeants. Le pool annoncé inclut notamment Astra, donc ne pas présumer son admissibilité HQ ni l’utiliser en direct sans validation de disponibilité. Une économie moyenne annoncée n’est pas une économie garantie sur votre tâche.

Avant la séance du 20 octobre : rouvrir les sources et vérifier le sélecteur du portable. Les données de cette fiche sont datées, pas figées contractuellement.
