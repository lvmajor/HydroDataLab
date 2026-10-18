# Données publiques et attribution

Les fichiers de `fixtures/` contiennent un **snapshot réel** extrait de l'API publique Hydro-Québec le **18 octobre 2026 à 19:44:01 UTC**. Les observations couvrent la journée civile du **17 octobre 2026**, de minuit inclus à minuit du 18 exclu, dans `America/Toronto`. Il ne s'agit pas de données fabriquées. Les exemples synthétiques des tests restent dans les tests.

| Fichier | Source et sélection | Observations |
|---|---|---:|
| `demand.json` | [Demande d'électricité au Québec](https://donnees.hydroquebec.com/explore/dataset/demande-electricite-quebec/) — intervalle 15 minutes, MW | 96 |
| `hydrometeo.json` | [Données hydrométéorologiques](https://donnees.hydroquebec.com/explore/dataset/donnees-hydrometeorologiques/) — niveau Réservoir Gouin Nord (`1-15412`), températures minimum/maximum Gouin Bois (`1-10622`), horaire | 72 |

**Attribution : Hydro-Québec. Licence des données : [CC BY-NC 4.0](https://creativecommons.org/licenses/by-nc/4.0/deed.fr).** Cette licence impose l'attribution, l'indication des modifications et une utilisation non commerciale. La licence du code ne remplace pas celle des données. Aucune approbation d'Hydro-Québec de ce projet n'est sous-entendue. Les conditions du fournisseur sont consultables depuis les fiches sources et sa [page de licence](https://www.hydroquebec.com/documents-donnees/donnees-ouvertes/licence.html).

`manifest.json` fournit les URL exactes des requêtes, la période, l'instant de récupération UTC, le nombre de lignes et les empreintes SHA-256 des réponses conservées. L'extracteur vérifie ces empreintes à chaque lecture du snapshot. Les exports sont des données **transformées** : conversion des instants en UTC, nombres typés, tri, validation et dédoublonnage. Le rapport indique ces opérations et leur provenance. Le scénario `missing` retire un point de niveau d'eau uniquement dans une copie en mémoire; le snapshot demeure intact.

Le mode `live` demande par défaut la dernière journée civile terminée à Toronto; `--date YYYY-MM-DD` permet d'en sélectionner une autre, si elle est encore disponible. Il échoue explicitement si la source est inaccessible ou vide. Il ne remplace pas discrètement les données par le snapshot. Le mode `fixture` refuse une date différente de celle du manifeste. Les données disponibles et leur horizon roulant peuvent changer; `fixture` est le mode reproductible pour la présentation sans réseau.

Un niveau en mètres n'est ni un volume stocké, ni un pourcentage de remplissage, ni une réserve d'énergie en TWh. Les températures horaires sont des minima/maxima, pas des moyennes. Cette démonstration n'établit aucun lien causal entre la demande provinciale et les conditions locales du réservoir.
