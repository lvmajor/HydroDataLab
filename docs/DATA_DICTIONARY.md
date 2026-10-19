# Dictionnaire de données HydroDataLab

## Grain et séries

Une ligne normalisée représente une mesure pour une série et un instant. La clé est `(series_id, timestamp_utc)`. Les données proviennent exclusivement des deux jeux publics Hydro-Québec cités dans `data/README.md` et `data/fixtures/manifest.json`.

| `series_id` | Source / site | Champ brut | Unité | Cadence attendue |
|---|---|---|---|---|
| `demand` | `demande-electricite-quebec`, Québec | `valeurs_demandetotal` | MW | 15 min |
| `water_level` | `donnees-hydrometeorologiques`, Réservoir Gouin Nord, `1-15412`, type `Niveau` | `valeur` | m | 60 min |
| `temperature_min` | Gouin Bois (CM2T), `1-10622`, type `Température Minimum` | `valeur` | °C | 60 min |
| `temperature_max` | Gouin Bois (CM2T), `1-10622`, type `Température Maximum` | `valeur` | °C | 60 min |

Les coordonnées publiées des sites sont environ 48,64748 N / 74,98146 O pour le niveau et 48,3327 N / 74,09 O pour la station météo, en Mauricie. Ce sont deux sites distincts. Le niveau est une élévation selon le référentiel de la source, pas une profondeur; aucun calcul de volume ou d'énergie n'en est déduit. Les températures sont les extrêmes horaires publiés, pas une température moyenne.

## Exports CSV et Parquet

| Colonne | Type logique | Règle |
|---|---|---|
| `series_id` | texte | Une des quatre séries ci-dessus |
| `timestamp_utc` | instant avec fuseau UTC | CSV ISO 8601 `...Z`; Parquet timestamp UTC |
| `value` | nombre flottant nullable | Les absences restent nulles; zéro est valide |
| `unit` | texte | `MW`, `m` ou `°C` selon la série |
| `station` | texte | Libellé explicite de la zone ou du site |

Le CSV utilise UTF-8, virgule comme séparateur, point décimal, en-tête et fin de ligne LF. Une cellule vide représente une valeur absente. Parquet conserve les types et la nullabilité. Les deux exports ont les mêmes lignes, triées par série puis instant UTC. Le scénario pédagogique `missing` retire une mesure de niveau valide après validation et dédoublonnage; il ne retire pas simplement une ligne qui était déjà nulle.

## Temps, validation et qualité

- `period.start` et `period.end` sont des dates civiles à Toronto; fin exclusive. Elles ne sont jamais interprétées comme minuits UTC.
- Le champ brut `date` contient son décalage UTC. Il est converti en instant UTC; un horodatage sans décalage est rejeté. Les mesures futures à l'instant d'exécution et les mesures hors période sont exclues.
- Les champs numériques absents ne sont ni imputés ni transformés en zéro. Une valeur non numérique/non finie ou booléenne devient absente et est comptée comme invalide. Une unité inattendue rejette l'observation.
- Les doublons d'une même clé sont dédupliqués, dernière occurrence conservée et nombre signalé. Le pipeline ne tente pas de réconcilier des révisions historiques.
- `observationCount` compte les valeurs numériques présentes après validation et dédoublonnage, pas les valeurs nulles. `seriesCount` vaut 4 si l'exécution réussit. Une série sans aucune valeur valide provoque un échec explicite.
- `minimum`, `maximum` et `latest` utilisent les valeurs présentes; `latest` est la dernière valeur numérique valide chronologiquement, non une prévision.
- `coverage.observed` compte les créneaux distincts alignés contenant une valeur valide; `expected` utilise la durée réelle entre les minuits locaux et la cadence. Les journées de 23/25 heures sont prises en compte. `percent` est arrondi à deux décimales. Au checkpoint 06 uniquement, `coverage` vaut `null` tant que la fonctionnalité n'est pas implémentée.

## Traçabilité et publication

Chaque exécution possède un UUID et un dossier contenant `observations.csv`, `observations.parquet` et `report.json`. Les trois fichiers sont d'abord écrits dans un dossier temporaire voisin, puis publiés ensemble par renommage. Un échec n'expose pas un rapport partiel. Une relance avec le même UUID et les mêmes paramètres renvoie le résultat déjà publié; des paramètres différents sont refusés.

`generatedAtUtc` est l'instant réel de génération du rapport. `provenance[].retrievedAtUtc` est l'instant réel d'extraction, distinct des dates d'observation. Le snapshot est extrait le 18 octobre 2026 à 19:44:01 UTC et couvre le 17 octobre; il comporte 168 observations valides. Les SHA-256 portent sur les fichiers bruts conservés. Le scénario `missing` indique explicitement la suppression pédagogique en mémoire dans `warnings`.

La licence des données est **CC BY-NC 4.0**, distincte de celle du code; les exports dérivés conservent l'attribution et les restrictions des données sources. Aucune série publique de « réserve énergétique disponible » n'est présentée.

## Synthèse horaire et indicateurs Python

Tous ces résultats décrivent le passé; aucun modèle de prévision n’est entraîné.

| Résultat | Définition et unité | Données incomplètes |
|---|---|---|
| Demande moyenne horaire | Somme des valeurs aux créneaux 00, 15, 30, 45 minutes ÷ 4, MW | `null` si un créneau manque ou n’est pas numérique. Les dates hors grille sont ignorées. |
| Pointe horaire moyenne | Maximum des moyennes horaires complètes, MW; première heure en cas d’égalité | Porte seulement sur les heures complètes; peut manquer une pointe réelle. |
| Énergie demandée estimée | Σ(demande MW × 0,25 h), MWh; équivalent à Σ(moyenne horaire MW × 1 h) | `null` tant que la période n’est pas terminée ou si un seul créneau manque. Aucune extrapolation. |
| Variation du niveau observé | (Dernier niveau valide − premier niveau valide) × 100, cm | Au moins deux instants valides; bornes réellement observées affichées. Ce n’est pas la variation de volume ou de réserve énergétique. |

L’estimation d’énergie suppose une puissance constante pendant chacun des intervalles de 15 minutes suivant la mesure. C’est une approximation rectangulaire pédagogique, pas une mesure officielle de consommation ou de facturation. La demande couvre le Québec hors réseaux autonomes selon le jeu source; elle n’est pas la production hydroélectrique.

Les heures sont générées entre les minuits locaux convertis en UTC : 23, 24 ou 25 lignes selon la journée. Une heure non terminée n’entre pas dans la synthèse. `localHour` contient le décalage UTC pour distinguer les deux occurrences de 01 h au changement d’heure. Zéro est valide, les doublons ne créent pas de créneau supplémentaire, aucune absence n’est remplacée par zéro. Les températures minimales/maximales restent deux séries distinctes et ne participent pas à ces trois indicateurs.

Jeu enregistré du 17 octobre 2026 : 96 demandes → 24 moyennes horaires; pointe 22 009,75 MW à 18 h Toronto; énergie estimée 489 442 MWh; niveau +3 cm entre 00 h et 23 h. Ces nombres sont des résultats du calcul, pas des valeurs codées dans l’interface.
