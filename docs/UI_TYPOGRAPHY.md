# Typographie et lisibilité des interfaces

Cette grille s’applique avant de livrer une interface nouvelle ou modifiée. Lire une page sur les « meilleures pratiques » ne suffit pas : contrôler le rendu réel, les états et les interactions, puis conserver les résultats. Références consultées le 19 octobre 2026.

## Conventions HydroDataLab

Le tableau de bord sert à lire des données BI sur un portable et à les montrer en salle. Conserver les courbes comme point focal, les alignements à gauche, les unités explicites et une palette bleu réseau / cyan eau. La lisibilité prime sur le nombre de cartes visibles sans défilement.

| Rôle | Taille à la base navigateur de 16 px | Règle |
| --- | --- | --- |
| Lecture, navigation, champs, action principale | 1.125rem = 18 px | Interligne 1,5; paragraphes autour de 55–65 caractères |
| Légendes, axes, provenance, aide, dates, statuts, tableaux | 1rem = 16 px | Même minimum sur mobile; ne pas les traiter comme une décoration |
| Titres de section | 1.5rem = 24 px | Graisse 700; interligne 1,25 |
| Sous-sections | 1.25rem = 20 px | Hiérarchie sémantique h2/h3 préservée |
| Chiffres clés | 2–2.25rem = 32–36 px | Chiffres tabulaires; unités et libellés lisibles |
| Titre principal | 2–3rem = 32–48 px | Adaptation fluide; pas de police condensée |

Police : Segoe UI, Aptos, sans-serif, disponibles localement. Ne pas télécharger une police pour la démonstration hors ligne. Tokens dans `web/src/style.css`. Les SVG sont tracés à la taille CSS réelle; leurs textes ne doivent pas diminuer avec une transformation ou un `viewBox` fixe. Adapter les colonnes, autoriser les retours à la ligne et élargir les panneaux avant de réduire les lettres.

Ces tailles sont **des conventions du projet**, pas des seuils universels imposés par WCAG. L’échelle [GOV.UK](https://design-system.service.gov.uk/styles/type-scale/) est un exemple de système public explicite (corps 19 px, secondaire 16 px); nous adaptons cette démarche au tableau de bord.

## Critères à vérifier

- **Contraste du texte :** au moins 4,5:1; WCAG permet 3:1 pour le grand texte (24 px normal ou environ 18,67 px gras). Ici, viser 4,5:1 aussi pour les informations secondaires. Mesurer les couleurs effectivement composées avec leur fond, pas seulement deux tokens isolés. [WCAG 1.4.3](https://www.w3.org/WAI/WCAG22/Understanding/contrast-minimum.html)
- **Agrandissement :** 200 % sans perte de contenu ou de fonctionnalité. Vérifier aussi l’agrandissement du texte seul. Les valeurs `rem` aident mais ne constituent pas une preuve. [WCAG 1.4.4](https://www.w3.org/WAI/WCAG22/Understanding/resize-text.html)
- **Redistribution :** à 320 pixels CSS, aucun défilement horizontal de page. Un tableau de données peut avoir son propre défilement, accessible au clavier. Aucun libellé coupé pour sauver de la place. [WCAG 1.4.10](https://www.w3.org/WAI/WCAG22/Understanding/reflow.html)
- **Espacement personnalisé :** pas de perte avec interligne 1,5, espace après paragraphe 2em, entre lettres 0,12em et entre mots 0,16em. Ce sont des valeurs de test à supporter, pas le style par défaut à imposer. [WCAG 1.4.12](https://www.w3.org/WAI/WCAG22/Understanding/text-spacing.html)
- **Interaction et contenu :** focus visible, courbes au clavier, filtres, tableaux ouverts, formulaire et erreur lisibles. Vérifier état vide, chargement, panne, traitement actif, couverture absente et résultat partiel. Les couleurs seules ne doivent pas porter les statuts.

## Validation avant livraison

1. Lancer les tests et le build Vue. Démarrer l’application via Aspire si nécessaire, sans arrêter une session utilisateur active.
2. Depuis la racine : `node web/qa/typography.mjs`. Edge doit être présent; `HYDRO_BROWSER_CHANNEL=chrome` permet Chrome. `HYDRO_WEB_URL` remplace l’URL par défaut.
3. Le contrôle lit le rapport public déjà chargé, puis simule les autres états **dans son navigateur uniquement**. Il ne lance aucun ETL et ne modifie ni les données ni la planification. Il inspecte les tailles et les contrastes calculés, la largeur et les axes, le clavier, les états et l’espacement. Résultats et captures : `.runtime/typography-qa/`.
4. Examiner les captures portable 1366×768 et projection 1920×1080 à leur taille utile. Vérifier 1280, 390 et 320 CSS px, l’équivalent de zoom 200 % (1280 pixels physiques pour 640 CSS px) et le texte seul à 200 %. Refaire une vérification humaine avec le vrai zoom du navigateur et sur le projecteur de la salle avant la séance.
5. Consigner les contrôles réussis et les limites dans `docs/validation/`. Ces contrôles ciblés et l’examen visuel ne constituent pas un audit exhaustif ni une certification de conformité WCAG.

## Où inscrire la règle pour un agent ?

- **Projet partagé :** exigence courte dans `.github/copilot-instructions.md`, déjà référencé par `AGENTS.md`. Elle oriente tout travail d’interface vers cette grille.
- **Frontend :** `.github/instructions/vue.instructions.md`, avec `applyTo: "web/**"`, pour les critères et commandes propres à Vue. Ce format est celui de Copilot; ne pas supposer que tout autre harnais interprète `applyTo`.
- **Tous vos projets personnels :** ajouter la même exigence courte dans **Customizations → Instructions → User**, dans le harnais visé. Pour Copilot, l’emplacement personnel permanent documenté est `~/.copilot/copilot-instructions.md`. Ne pas modifier les réglages personnels d’un collègue depuis un dépôt.
- **Procédure réutilisable :** un skill de revue UI peut détailler le travail, mais une instruction permanente doit demander explicitement son utilisation et une preuve de vérification. Un skill seul n’assure pas son activation.

Instruction courte réutilisable : « Pour toute interface créée ou modifiée, vérifier la typographie et l’accessibilité selon WCAG 2.2 et le système de design du projet. Contrôler tailles effectives, contrastes, zoom 200 %, redistribution et espacement personnalisé dans le navigateur. Fournir les résultats et les limites; ne pas conclure sur la seule compilation. »

Vérifier la découverte dans Customizations, les References de la réponse et surtout les actions réellement effectuées : [documentation officielle VS Code](https://code.visualstudio.com/docs/agent-customization/custom-instructions).
