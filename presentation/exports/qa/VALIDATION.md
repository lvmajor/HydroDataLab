# Validation de la présentation

## État courant : progression Copilot et notes synchronisées

19 octobre 2026. **28 diapositives : 22 principales et 6 annexes.** Copilot dans VS Code est le sujet central; HydroDataLab reste l’exemple. Instructions 8–9, contexte 10, mécanismes 11, vanille 12; skills 20 près des ressources; Canvas uniquement en annexe 25. Le calendrier des notes et du guide comporte les mêmes 22 plages, soit 32 minutes d’explications et 13 de démos. La répétition humaine doit encore confirmer le rythme en salle.

Build et contrôle navigateur réussis : 28 routes, aucun débordement détecté, aucune erreur de page, aucune requête externe. Synchronisation présentateur/audience validée pour les couches (10) et les coûts (15), y compris navigation, rechargement et limites des contrôles. PDF de 28 pages; textes, liens officiels et quatre destinations internes contrôlés. Pages modifiées inspectées à pleine taille et planches parcourues. Deux retours à la ligne corrigés avant l’export final. Avertissement FloatingVue dans l’outillage d’export; aucun effet constaté sur les rendus ou le site compilé.

Conducteur : huit rubriques, 21 boutons Copier et repli de sélection, quatre largeurs, liens locaux et impression contrôlés; 19 blocs PowerShell et deux scripts de kit syntaxiquement valides. Preuves : `../../../docs/validation/presentation-flow.json`, `../../../docs/validation/presentation-guide.json`, `browser-report.json` et `presenter-sync.json`. L’application et le moteur ETL n’ont pas changé dans cette révision.

## Historique : approche vanille, skills et ressources

Support de **28 diapositives : 22 principales et 6 annexes**. Pages ajoutées : 11, 12, 22; la conclusion 21 renvoie à la nouvelle page de ressources. Notes et conducteur conservent 45 minutes de contenu. Renvois vers les annexes et contrôles de synchronisation adaptés.

Build réussi, 28 routes sans débordement, erreur de page ou ressource externe. Les couches (13) et coûts (16) restent synchronisés entre présentateur et audience. Sept liens publics et trois emplacements HQ sans fausse URL vérifiés. PDF de 28 pages avec liens officiels et quatre destinations internes; planches complètes et pages nouvelles inspectées. Preuve : `../../../docs/validation/vanilla-skills-resources.json`. Les rendus `page-01.png` à `page-28.png` et les trois planches correspondent à cette version.

## Historique : contexte du public

Le support compte maintenant **25 diapositives : 19 principales et 6 annexes**. La page 2 présente le développement BI/Python local, la préparation au déploiement conteneurisé, la marge d’expérimentation et les réflexes de sécurité. Le minutage reste de 32 min d’explications et 13 min de démos; l’ouverture est répartie en 30 s / 1 min / 30 s.

Build et export réussis. Contrôle navigateur des 25 routes : zéro débordement, erreur de page ou requête externe; interactions, ordre de l’ouverture et notes `/presenter/18` vérifiés. Le PDF contient 25 pages; les textes, références officielles et renvois internes sont vérifiés. Les trois planches et la page 2 agrandie ont été inspectées. Preuve : `../../../docs/validation/audience-context.json`; captures courantes `page-01.png` à `page-25.png`, `contact-1.jpg` à `contact-3.jpg` et `revision-2.png`.

Les résultats ci-dessous décrivent les révisions antérieures à 24 pages.

## Révision pédagogique du 18 octobre

Le support introduit maintenant modèle/outils/agent, puis dépôt/commit/branche/worktree avant la première mission. Le worktree reste facultatif. Les démonstrations affichent un objectif observable; leurs manipulations, minutages et recours à la solution préparée sont dans les notes. Les autres notions nouvelles sont définies au premier emploi; un lexique est joint à l'aide-mémoire. Les 18 pages principales, 6 annexes et 45 minutes de contenu sont conservées.

Après révision : build et export réussis, PDF de 24 pages avec rendu inspecté. Contrôle navigateur des 24 pages et des deux interactions sans débordement ni erreur JavaScript; requêtes externes bloquées. La séparation audience/notes est également vérifiée : la diapositive 17 montre le résultat à vérifier, tandis que `/presenter/17` contient le seuil de secours à 2:10. Le build utilise `--base /` pour corriger le chargement des ressources depuis ces liens profonds. Preuve complémentaire : `../../../docs/validation/pedagogy.json`.

Contrôle effectué le 18 octobre 2026 dans le workspace de création. Cette validation porte sur le support; la répétition sur le portable HQ reste à faire.

## Résultats

- `npm run build` : réussi, Slidev 53.0.0; sortie statique dans `dist/`.
- `node scripts/qa_browser.mjs` : 24 routes, aucune erreur JavaScript de page, aucun débordement détecté, aucune requête externe. Toutes les requêtes hors du serveur local ont été bloquées pendant le test.
- Interaction contexte : sélection du palier Contrat et affichage de son contenu vérifiés.
- Interaction modèle : Luna affiche 0.003 USD pour une tentative puis 0.006 pour deux, selon les hypothèses illustratives affichées.
- `npm run export` : réussi avec Chrome local; `formation-hydrodatalab.pdf` contient exactement 24 pages.
- Extraction PDF : texte présent dans chacune des 24 pages.
- Inspection visuelle : les 24 pages rendues avec Poppler ont été examinées sur deux planches; vérification agrandie des blocs de code. Aucun texte tronqué ou chevauchement constaté. Contraste du code renforcé pour la projection.
- Structure : 18 diapositives principales et 6 annexes. Minutage des notes et du conducteur : 32 min d’explication + 4/5/4 min de démo + 10 min de questions + 5 min de marge.
- Polices, favicon et ressources de rendu conservés localement. Les références web sont des liens à ouvrir volontairement.

Les preuves sont `browser-report.json`, `context-interactive.png`, `model-interactive.png`, les 24 images `page-*.png` et `contact-1.jpg` / `contact-2.jpg`.

## Limites connues

- L’export Slidev affiche un message console non bloquant « Failed to patch FloatingVue » provenant de l’intégration Twoslash/FloatingVue des dépendances. L’export se termine avec le code 0. Les deux interactions de la version construite et le PDF ont été contrôlés malgré ce message. Le contrôle des erreurs de page ne mesure pas toutes les lignes console des dépendances.
- L’installation avec Node 25 affiche un avertissement de plage de version pour postcss-nested. Build et export réussissent dans cet environnement; Node LTS 22 ou 24 est préférable pour la préparation.
- Le PDF fige les composants interactifs à leur état initial. Les hypothèses et le résumé des cinq paliers restent lisibles.
- Les libellés et tarifs sont documentés au 18 octobre 2026; disponibilité des modèles et politiques HQ doivent être vérifiées sur le poste de présentation.
- Pas de capture VS Code simulée. Les schémas conceptuels sont signalés. Les captures de secours de l’application réelle sont dans `docs/screenshots/` à la racine du dépôt.

## Historique : audit de dépendances avant révision pédagogique

DOMPurify remplacé par 3.4.17 via override compatible le 18 octobre. Build relancé avec succès; voir `../../DEPENDENCY-AUDIT.md` et les rapports npm pour les 17 entrées résiduelles. Le serveur d’édition a été observé sur ::1 uniquement. Cet audit précédait la révision pédagogique décrite en tête de document; le PDF a depuis été régénéré et contrôlé.
