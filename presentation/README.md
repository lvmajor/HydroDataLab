# Présentation HydroDataLab

Sujet central : **GitHub Copilot dans VS Code et les bonnes pratiques des agents**. HydroDataLab sert de fil conducteur.

**28 diapositives : 22 principales et 6 annexes.** Prestation du 20 octobre 2026; sources vérifiées les 18 et 19 octobre. Le contenu et les démos occupent 45 minutes : 32 minutes d’explications et 4 + 5 + 4 minutes de démonstrations. Questions : 10 minutes; marge : 5.

## Lancer localement

Les fichiers de présentation sont disponibles à partir du tag `05-docker`. Au jalon `04-pilotage`, préparer uniquement l’application avec `./scripts/prepare.ps1 -SkipPresentation`.

À la racine de la version finale : `./scripts/start-presentation.ps1`. Sinon, depuis ce dossier : `npm ci` puis `npm run dev`. Ouvrir http://localhost:53173, mode présentateur : http://localhost:53173/presenter. Les flèches changent de page; les composants interactifs fonctionnent au clavier et à la souris.

`npm run build` produit `dist/`. `npm run preview` le sert sur http://127.0.0.1:53173. Après installation, polices, favicon et composants sont locaux; aucune CDN n’est nécessaire. Les liens de référence ne s’ouvrent que sur action explicite. Un serveur HTTP local est requis : ne pas ouvrir index.html directement par file://.

Le build est servi à la racine du serveur (`--base /`) pour que les liens directs vers `/presenter/20` chargent aussi leurs ressources. La vue audience affiche les notions et objectifs; les raccourcis, minutages et procédures de secours sont dans les notes du présentateur. Le lexique est dans l'aide-mémoire.

Pour une installation qui utilise Chrome/Edge existant, définir `PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD=1` avant `npm ci` afin d’éviter de télécharger un autre navigateur. Node 22.12+ est requis par Slidev; un Node LTS 22 ou 24 évite l’avertissement de compatibilité de postcss-nested observé avec Node 25.

## Présenter avec deux écrans

Dans Windows, utiliser `Win + P` → **Étendre**. Ouvrir deux fenêtres du **même navigateur et du même profil** :

- Portable : http://127.0.0.1:53173/presenter/1 — notes et commandes.
- Écran de projection : http://127.0.0.1:53173/1 — audience, en plein écran avec `F11`.

Dans l’aperçu **Current → Slides** du présentateur, cliquer une couche de contexte (diapositive 10) ou les boutons `−`/`+` du comparatif des coûts (15). La vue audience reproduit la sélection, le texte et les coûts. La synchronisation fonctionne dans les deux sens, sans Internet, avec le serveur de développement, le build statique ou le kit Python.

Les réglages sont conservés après navigation, rechargement ou ouverture tardive de la fenêtre audience. Pour revenir au début : sélectionner la première couche et ramener les essais à 1. Garder **la même adresse et le même port** dans les deux fenêtres : `localhost` et `127.0.0.1` sont deux origines distinctes pour le navigateur. Des profils ou navigateurs différents ne partagent pas ces réglages. Après une mise à jour du site, recharger les deux fenêtres.

La synchronisation utilise le stockage local réactif [VueUse `useStorage`](https://vueuse.org/core/usestorage/), avec deux clés propres à HydroDataLab. Les valeurs lues sont validées avant affichage.

## Modifier la page finale de ressources

La diapositive **22** utilise [resources/links.json](resources/links.json). Modifier `label`, `url` et `description` dans `internal` pour ajouter les liens HQ. Une URL vide reste un emplacement sans faux lien. Conserver trois entrées courtes pour la projection; compléter la fiche [skills-et-veille.md](../handouts/skills-et-veille.md) pour plus de détails.

Après modification : `npm run build`, `npm run export`, puis recharger les deux fenêtres. La page 12 expose le choix « vanille »; la page 20 propose une sélection par besoin. Matt Pocock y reste « en exploration ».

## Export PDF

`npm run export` produit `exports/formation-hydrodatalab.pdf`. Le script détecte Chrome/Edge sur Windows ou Chromium Playwright. Pour un autre emplacement, définir `SLIDEV_BROWSER_PATH`. Aucun téléchargement n’est lancé par ce script d’export. Le PDF contient 28 pages statiques : interactions figées à l’état initial avec leurs hypothèses visibles.

## Vérifications

Après `npm run build`, démarrer `npm run preview`, puis `node scripts/qa_browser.mjs` sur la machine de création. Le contrôle parcourt 28 routes, vérifie les débordements, bloque les requêtes externes et exerce les deux interactions. Rapport : `exports/qa/browser-report.json`.

`node scripts/qa_presenter_sync.mjs` vérifie les contrôles dans deux pages d’un même navigateur, dont la vraie vue présentateur : couches, coûts, navigation, rechargement et ouverture tardive, avec les requêtes externes bloquées. Rapport : `exports/qa/presenter-sync.json`. `PRESENTATION_URL` permet de contrôler un kit servi sur un autre port.

Le contrôle PDF utilise `scripts/qa_pdf.py` avec pypdf et Pillow, après rendu Poppler vers `exports/qa/page-01.png` à `page-28.png`. Les images et planches sont des artefacts de contrôle, pas des diapositives supplémentaires. Voir `exports/qa/VALIDATION.md`.

## Direction visuelle

Bleu profond #053448, eau #006b83, glace #eef6f8, encre #123d51, jaune #e8b540. Barlow Condensed pour les titres, IBM Plex Sans pour la lecture, IBM Plex Mono pour le code. Fontsource fournit les fichiers locaux. Les contrastes évoquent une fiche d’observation hydroclimatique, sans logo ni prétention à un gabarit officiel HQ. Les schémas décrivent les concepts, pas une reproduction de VS Code. Les animations sont limitées aux révélations utiles et respectent reduced motion.

Fiches : `../handouts/`. Références : `../docs/SOURCES.md`. Le conducteur et les notes restent la source de vérité du minutage.

## Mettre à jour le kit sans dupliquer Docker

Pour une révision des diapositives, reconstruire le site et le PDF, valider puis enregistrer les changements dans Git. Depuis la racine, utiliser `./scripts/refresh-kit.ps1 -KitPath artifacts/kit-hq-20261019-101734` : la présentation, les fiches, la documentation et une sauvegarde Git sont actualisées; l’archive Docker de 1,21 Go reste en place et son empreinte est vérifiée. `REVISION.txt` identifie la révision du kit; `SHA256SUMS.json` est régénéré. Les anciennes petites sauvegardes et ressources compilées restent conservées.

`package-kit.ps1` reste réservé à une nouvelle livraison complète ou à une nouvelle version des images. Il crée un dossier distinct d’environ 1,23 Go. Éviter de le relancer pour chaque correction de texte. Conserver aussi une copie du kit final sur un support distinct avant la séance.

## Dépendances de l’outil de présentation

DOMPurify est fixé en 3.4.17. L’audit npm conserve 17 entrées transitives signalées (6 moderate, 11 high), notamment des dénis de service de l’outillage sans correctif compatible publié. L’export PowerPoint n’est pas utilisé. Les détails, limites et preuves sont dans [DEPENDENCY-AUDIT.md](DEPENDENCY-AUDIT.md). Pour la projection, utiliser le build déjà vérifié avec `npm run preview` ou le PDF; ne pas activer `--remote` ni importer des contenus non fiables en séance.
