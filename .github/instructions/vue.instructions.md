---
description: Interface Vue, typographie, accessibilité et affichage des indicateurs
applyTo: "web/**"
---

Appeler l'API .NET via `/api`. Afficher unités, période, provenance et statut des données.
Ne jamais transformer une valeur absente en zéro pour un graphique.
Afficher explicitement l'absence du calcul de couverture au checkpoint de départ.
Conserver une interface lisible au clavier et en projection; ne pas coder de données fictives comme résultats réels.

Avant toute création ou modification d’interface, appliquer [la grille de validation typographique](../../docs/UI_TYPOGRAPHY.md), fondée sur WCAG 2.2 et une échelle de design explicite.
Réutiliser les variables CSS : texte courant 1.125rem, secondaire 1rem minimum dans ce projet; ne pas réduire le texte pour le faire tenir, ni laisser les SVG réduire les axes.
Contrôler les contrastes réels, zoom/texte à 200 %, largeur 320 CSS px, espacement personnalisé et parcours clavier. Inclure graphiques, légendes, formulaires, tableaux, erreurs et états vides.
Exécuter les tests et la compilation Vue, puis `node web/qa/typography.mjs` avec l’application démarrée. Examiner les captures; documenter les limites au lieu de prétendre à une certification WCAG.
