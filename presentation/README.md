# Présentation HydroDataLab

**24 diapositives : 18 principales et 6 annexes.** Prestation du 20 octobre 2026; sources vérifiées le 18 octobre. Le contenu et les démos occupent 45 minutes : 32 minutes d’explications et 4 + 5 + 4 minutes de démonstrations. Questions : 10 minutes; marge : 5.

## Lancer localement

Les fichiers de présentation sont disponibles à partir du tag `05-docker`. Au jalon `04-pilotage`, préparer uniquement l’application avec `./scripts/prepare.ps1 -SkipPresentation`.

À la racine de la version finale : `./scripts/start-presentation.ps1`. Sinon, depuis ce dossier : `npm ci` puis `npm run dev`. Ouvrir http://localhost:53173, mode présentateur : http://localhost:53173/presenter. Les flèches changent de page; les composants interactifs fonctionnent au clavier et à la souris.

`npm run build` produit `dist/`. `npm run preview` le sert sur http://127.0.0.1:53173. Après installation, polices, favicon et composants sont locaux; aucune CDN n’est nécessaire. Les liens de référence ne s’ouvrent que sur action explicite. Un serveur HTTP local est requis : ne pas ouvrir index.html directement par file://.

Le build est servi à la racine du serveur (`--base /`) pour que les liens directs vers `/presenter/17` chargent aussi leurs ressources. La vue audience affiche les notions et objectifs; les raccourcis, minutages et procédures de secours sont dans les notes du présentateur. Le lexique est dans l'aide-mémoire.

Pour une installation qui utilise Chrome/Edge existant, définir `PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD=1` avant `npm ci` afin d’éviter de télécharger un autre navigateur. Node 22.12+ est requis par Slidev; un Node LTS 22 ou 24 évite l’avertissement de compatibilité de postcss-nested observé avec Node 25.

## Export PDF

`npm run export` produit `exports/formation-hydrodatalab.pdf`. Le script détecte Chrome/Edge sur Windows ou Chromium Playwright. Pour un autre emplacement, définir `SLIDEV_BROWSER_PATH`. Aucun téléchargement n’est lancé par ce script d’export. Le PDF contient 24 pages statiques : interactions figées à l’état initial avec leurs hypothèses visibles.

## Vérifications

Après `npm run build`, démarrer `npm run preview`, puis `node scripts/qa_browser.mjs` sur la machine de création. Le contrôle parcourt 24 routes, vérifie les débordements, bloque les requêtes externes et exerce les deux interactions. Rapport : `exports/qa/browser-report.json`.

Le contrôle PDF utilise `scripts/qa_pdf.py` avec pypdf et Pillow, après rendu Poppler vers `exports/qa/page-01.png` à `page-24.png`. Les images et planches sont des artefacts de contrôle, pas des diapositives supplémentaires. Voir `exports/qa/VALIDATION.md`.

## Direction visuelle

Bleu profond #053448, eau #006b83, glace #eef6f8, encre #123d51, jaune #e8b540. Barlow Condensed pour les titres, IBM Plex Sans pour la lecture, IBM Plex Mono pour le code. Fontsource fournit les fichiers locaux. Les contrastes évoquent une fiche d’observation hydroclimatique, sans logo ni prétention à un gabarit officiel HQ. Les schémas décrivent les concepts, pas une reproduction de VS Code. Les animations sont limitées aux révélations utiles et respectent reduced motion.

Fiches : `../handouts/`. Références : `../docs/SOURCES.md`. Le conducteur et les notes restent la source de vérité du minutage.

## Dépendances de l’outil de présentation

DOMPurify est fixé en 3.4.17. L’audit npm conserve 17 entrées transitives signalées (6 moderate, 11 high), notamment des dénis de service de l’outillage sans correctif compatible publié. L’export PowerPoint n’est pas utilisé. Les détails, limites et preuves sont dans [DEPENDENCY-AUDIT.md](DEPENDENCY-AUDIT.md). Pour la projection, utiliser le build déjà vérifié avec `npm run preview` ou le PDF; ne pas activer `--remote` ni importer des contenus non fiables en séance.
