# Preuve de validation navigateur — HydroDataLab

Validation effectuée le **18 octobre 2026 à 16 h 16, heure de Toronto** (résultat automatisé : `2026-10-18T20:16:26.323Z`).

## Environnement vérifié

- Application servie par la composition de conteneurs : `http://127.0.0.1:53170`.
- Tableau de bord de télémétrie : `http://127.0.0.1:53174`.
- Microsoft Edge en mode headless, Playwright 1.64.1.
- Vue de bureau : 1440 × 1100; mobile : 390 × 844.
- Données locales de démonstration, scénario complet puis scénario manquant.

## Résultats

| Parcours | Résultat observé |
|---|---|
| Collecte locale | Traitement terminé, rapport affiché |
| Visualisations | Trois graphiques; températures minimale et maximale séparées |
| Période | Filtre 6 h activé puis retour à toute la période |
| Clavier | Inspection de la demande avec les flèches; unité MW visible |
| Qualité | Couverture du niveau d’eau réduite et détail **23 / 24 = 95,83 %** visible |
| Tableau | Mesures disponibles dans le tableau détaillé |
| Échec contrôlé | Échec visible dans l’historique |
| Relance | Nouveau traitement terminé après relance depuis l’interface |
| Planification | Activation enregistrée, puis désactivation enregistrée et confirmée par l’API |
| CSV | Téléchargement réussi, contenu non vide |
| Parquet | Téléchargement réussi, signature binaire `PAR1` vérifiée |
| Mobile | Aucun débordement horizontal à 390 px |
| JavaScript | Aucune exception de page capturée |
| Réseau du navigateur | **Aucune requête HTTP(S) vers une origine externe à l’application locale** pendant le parcours |

État laissé en fin de test : **dernier traitement local réussi, scénario complet; planification désactivée**.

## Captures de secours

- `01-poste-controle.png` : poste de contrôle à l’ouverture.
- `02-observatoire-complet.png` : rapport complet et graphiques.
- `03-qualite-mesure-absente.png` : scénario manquant, qualité par série dépliée.
- `03b-qualite-detail.png` : cadrage rapproché du détail qualité, conservé du contrôle natif.
- `04-echec-controle.png` : échec explicite avec possibilité de relance.
- `05-relance-et-exports.png` : état après relance et contrôles de planification.
- `06-mobile.png` : interface sur écran de 390 px.
- `07-aspire.png` : ressources du démarrage **natif**, capture conservée; aucun token de connexion visible.
- `08-compose.png` : traces reçues par le tableau de bord de la composition de conteneurs, filtrées sur les activités `ETL HydroDataLab`; huit traces et les échecs simulés sont visibles.

Les captures 01 à 06 ont été produites par le parcours Compose. Le fichier 03b et la capture 07 proviennent du mode natif.

## Reproduction et limites

Après démarrage des conteneurs, depuis `web/` :

```powershell
$env:HYDRO_WEB_URL = 'http://127.0.0.1:53170'
node qa/browser-smoke.mjs
```

Le script crée des exécutions de démonstration et active brièvement la planification avant de la désactiver. Ne pas lancer simultanément un autre traitement pendant ce test. Son résultat machine est `web/qa-results/browser-smoke.json`, ignoré par Git.

Ce contrôle vérifie le navigateur et les téléchargements du scénario local. Il ne constitue pas un test de récupération des sources publiques en direct, une certification d’accessibilité ou une répétition sur le portable Hydro-Québec. La surveillance réseau couvre les requêtes du navigateur, pas les sorties réseau éventuelles des services côté serveur.

