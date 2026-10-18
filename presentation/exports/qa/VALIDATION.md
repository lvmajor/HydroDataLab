# Validation de la présentation

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

## Complément après audit de dépendances

DOMPurify remplacé par 3.4.17 via override compatible le 18 octobre. Build relancé avec succès; voir `../../DEPENDENCY-AUDIT.md` et les rapports npm pour les 17 entrées résiduelles. Le serveur d’édition a été observé sur ::1 uniquement. Aucun contenu de diapositive n’a changé; le PDF reste celui contrôlé ci-dessus.
