# HydroDataLab — interface Vue

Tableau de bord francophone Vue 3 / TypeScript, graphiques SVG interactifs et API .NET uniquement. Les courbes ne sont pas simulées par le client : avant le premier rapport, un état vide propose la collecte du jeu local.

## Développement

Prérequis : Node.js 22.12+ (série 22) ou 24 LTS et le service API démarré.

```powershell
cd web
npm ci
$env:API_BASE_URL = 'http://localhost:53171'
npm run dev
```

Ouvrir http://localhost:53170. Le proxy Vite transmet `/api` à `API_BASE_URL`. Ce réglage appartient au serveur de développement; il ne contient aucun secret ni URL embarquée dans le navigateur.

```powershell
npm test
npm run build
```

En conteneur, construire `web/Dockerfile`; nginx écoute sur 80 et transmet `/api` à `http://api:8080`. Le nom `api` doit exister sur le réseau Docker. Les versions npm et le lock sont figés pour reproduire la formation.

## Démonstration

1. Démarrer l’API et le service Python, puis lancer le jeu de démonstration dans le poste de contrôle.
2. Explorer la demande en MW, le niveau du Gouin en mètres, les deux séries météo horaires en °C. Le niveau d’eau n’est jamais converti en pourcentage de réserve.
3. Survoler les courbes ou utiliser les flèches gauche/droite lorsque le graphique a le focus. Les filtres 24 h / 6 h sont relatifs au dernier instant du rapport, même pour un jeu historique.
4. Cocher « Retirer une mesure de niveau d’eau »; une valeur nulle reste absente et coupe la courbe. La couverture inconnue est affichée « Non calculée ».
5. Démontrer un échec contrôlé, puis relancer le traitement depuis l’historique. Le dernier rapport réussi reste disponible.
6. Choisir les sources ouvertes en direct, explorer la provenance et télécharger CSV ou Parquet. Les droits d’usage dépendent des licences affichées.

Le suivi des tâches interroge l’API toutes les trois secondes sans chevaucher ses requêtes; un arrêt réseau conserve le dernier rapport avec un avertissement. La planification périodique est facultative et enregistrée côté API. Son comportement métier est défini par le backend. Le tableau détaillé présente au maximum les 300 dernières mesures visibles; les exports contiennent le traitement complet.

## Direction visuelle

Palette : bleu encre `#173650`, bleu réseau `#006cbb`, bleu profond `#003f88`, cyan eau `#13a5b1`, blanc bleuté `#f2f7fb`, ambre qualité `#975e09`. Typographies locales Aptos / Bahnschrift / Segoe UI. Une grande courbe de demande, deux lectures complémentaires et un poste de contrôle distinct; aucun logo officiel ni police distante. Interface adaptable aux petits écrans, focus clavier visibles, mouvement réduit et impression simplifiée.


## Vérification du parcours dans Edge

Les services Aspire doivent être démarrés et aucun autre utilisateur ne doit lancer de collecte pendant ce test. Microsoft Edge doit être installé. Ce test crée des exécutions de démonstration, active brièvement puis désactive la planification, télécharge les exports et enregistre les captures dans `docs/screenshots/`.

```powershell
node qa/browser-smoke.mjs
```

`HYDRO_WEB_URL` permet de remplacer l’URL par défaut `http://127.0.0.1:53170`. Résultat détaillé dans `web/qa-results/browser-smoke.json` (ignoré par Git). La simulation de mesure manquante peut retirer une observation entière : les courbes sont aussi interrompues lorsqu’un écart dépasse 1,5 fois le pas attendu. Une absence en bordure raccourcit la série sans créer de rupture intérieure.
