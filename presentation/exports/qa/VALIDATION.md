# Validation de la présentation

## État courant : dernière passe pédagogique

19 octobre 2026. Relecture des 31 slides et de leurs notes. Définitions de contexte/test, diff et instruction placées avant les démonstrations qui les utilisent. Démo 1 limitée aux instructions; calcul zéro/absence réservé à la 8; format en 9, portée en 10 et chargement en 11. Le bloc 15–19 distingue volumes, Auto, simulation de coûts, benchmark modèle/effort et orchestration. Conclusion remplacée par une formulation de tâche personnelle dans les mêmes 90 secondes, dont 20 secondes de réflexion. Notes, guide, aide-mémoire et conducteur Markdown/HTML alignés; aucune commande de démo modifiée.

Build réussi. Contrôle CUA des 31 routes en 1366×768 : aucun débordement détecté. Les huit slides dont le contenu projeté change ont également été contrôlées en 1920×1080. Notes modifiées contrôlées sur portable, intertitres présents et précisions initialement fermées. Synchronisation des trois états de contexte et conservation au rechargement vérifiées; coûts 1 → 2 → 1 essai vérifiés dans les deux vues. Message FloatingVue/Popper connu, sans défaut visuel ou d’interaction constaté ici.

PDF régénéré : 31 pages, textes/liens attendus et six destinations internes valides; trois planches examinées. Les 25 plages notes/guide concordent : **32 + 13 minutes**. Démos à **09:00, 29:30 et 36:30**; questions à 45:00, marge jusqu’à 60:00. Le rythme oral et la projection sur le portable HQ restent à confirmer par la répétition humaine. Pas de nouveau test réseau coupé. Le conducteur HTML a reçu les deux mêmes paragraphes que sa source; cette passe ne revalide pas ses contrôles de copie.

Preuves : `../../../docs/validation/presentation-final-pass.json`, `presentation-flow.json`, captures `final-pass-*`, rendus PDF `page-*` et planches `contact-*`.

## Historique : contexte → interface → skill et outil

19 octobre 2026. La slide 11 montre les règles et les références qui enrichissent une tâche, sans reprendre le parcours d’une skill. La 12 situe les rubriques de configuration et prépare la comparaison. La 13, « Charger une skill, appeler un outil », porte seule le détail nom/description → SKILL.md → références et choix d’outil → permissions → résultat. Le temps de parole distingue mieux méthode disponible et preuve d’exécution. Notes, guide, aide-mémoire, fiche et sources alignés.

Build réussi. Contrôle CUA des slides 11/13 en 1366×768 et 1920×1080 : aucun débordement; marges avant les références respectivement 21,6/11,3 px et 30,4/15,9 px. Les trois états du contexte sont contrôlés et synchronisés; sélection conservée après rechargement, retour audience → présentateur vérifié, premier état restauré. Notes 11–13 contrôlées sur portable : quatre repères chronométrés par slide, détails fermés, pas de débordement horizontal. Scripts de vérification existants adaptés; validation interactive de cette révision effectuée avec CUA.

PDF régénéré et contrôlé : 31 pages, six destinations internes, pages 11/13 inspectées. L’absence de SKILL.md sur la 11 et sa présence sur la 13 sont vérifiées. Les 25 plages notes/guide correspondent, total **32 + 13 minutes**, créneaux **60 / 75 / 90 secondes** inchangés. Message FloatingVue/Popper d’export déjà connu; aucun défaut observé dans ces contrôles. La répétition humaine sur HQ et un nouveau test réseau coupé restent à faire.

Preuves : `../../../docs/validation/context-skill-sequence.json`, `presentation-flow.json`, captures `context-flow-*` et rendus PDF `page-11.png` / `page-13.png`.

## Historique : portée et chargement progressif

19 octobre 2026. Slides 10–11 remaniées sans ajout de page : **31 slides, dont 25 principales**. La portée (projet, fichiers ou tâche) est distincte du chargement. Une skill peut être Task scoped ET On demand. Exemples : brut/provenance, règle Python applicable, procédure /validate-etl volontaire, dictionnaire et test de couverture. Progression nom/description → corps SKILL.md → références utiles. Notes ventilées, aide-mémoire, fiche et guide synchronisés.

Build réussi. Contrôle CUA des deux slides audience en **1366×768 et 1920×1080**; dimensions réellement lues dans le DOM. Marges avant les références : 30,7/23,6 px sur portable et 43,1/33,2 px en projection. Aucun débordement dans les trois états interactifs. Notes 10–11 contrôlées sur portable : quatre repères chronométrés par slide, détails initialement repliés, aucun débordement horizontal; ouverture au clavier vérifiée en 11. Les trois sélections se synchronisent présentateur → audience et dans l’autre sens, persistent après rechargement et retour de navigation native. L’override de viewport ne redimensionnant pas uniformément tous les onglets, les captures ont été prises uniquement après confirmation des dimensions effectives.

PDF régénéré : **31 pages, six destinations internes**, textes et liens attendus présents; pages 10–11 inspectées. Les 25 plages notes/guide correspondent; **1 min 30 + 1 min**, de 16:30 à 19:00, et **32 + 13 minutes** au total. Message console FloatingVue/Popper connu, sans défaut observé dans ces vérifications. Pas de nouveau test réseau coupé ni de répétition sur le portable HQ. Les scripts QA existants ont été adaptés aux trois contrôles; leur syntaxe est vérifiée, la validation interactive de cette révision a été réalisée avec CUA.

Preuves : `../../../docs/validation/context-scope-loading.json`, `presentation-flow.json`, captures `scope-loading-*`, rendus PDF `page-10.png` et `page-11.png`. Les rapports précédents restent historiques.

## Historique : architecture du contexte par paliers

19 octobre 2026. **31 slides, dont 25 principales et 6 annexes.** La 9 emploie « règles communes du projet ». La 10 distingue Always on, Task scoped et On demand; la 11 montre les contributions à une tâche de couverture. Documents détaillés, règles de chemin, contexte implicite IDE/CLI, rôle choisi et workflow sont distingués. MCP/outils/permissions figurent dans le rôle du harnais; aucune inclusion automatique de toutes leurs données n’est affirmée. Notes, aide-mémoire, fiche, guide et conducteur HTML alignés.

Build réussi. Contrôle CUA des 31 routes audience : aucun débordement détecté. Rendus 9–11 en 1366×768 et 1920×1080, notes 10–11 aux deux tailles, détails accessibles au clavier. Chevauchements initiaux corrigés avant livraison. Les cinq sélections du contexte se propagent du présentateur à l’audience, sont conservées après rechargement et répondent dans l’autre sens. Les coûts de la slide 17 passent de 1 à 2 essais dans les deux vues, puis reviennent à 1. Console : message FloatingVue/Popper déjà connu dans l’outillage, sans défaut observé sur les interactions et rendus contrôlés. Cette vérification CUA n’intercepte pas les requêtes réseau; elle ne remplace pas un nouveau test réseau coupé sur le portable HQ.

PDF : 31 pages, termes et sources attendus, six destinations internes valides. Pages 9–11 examinées à pleine taille; renvois des pages suivantes actualisés. Les 25 plages notes/guide correspondent : **32 + 13 minutes**, démos inchangées. Les slides 9/10/11 occupent 1 min 30 / 1 min 30 / 1 min, de 15:00 à 19:00. Preuves : `../../../docs/validation/tiered-context.json`, `presentation-flow.json`, captures `tiered-*` et rendus `page-*`. Les anciens rapports nommés thinking-effort/browser-report/presenter-sync décrivent leurs révisions antérieures.

## Historique : capture réelle Artificial Analysis

19 octobre 2026. Slide 17 : capture réelle du graphique public, six configurations Sol/Codex et Sonnet/Claude Code aux niveaux Low, High et Max. Les valeurs du tableau viennent des six infobulles enregistrées; l’axe du coût est logarithmique et les indices affichés sont arrondis. Deux comparaisons sont commentées : Sol High → Max (+74 % de coût, indice affiché 60) et Sonnet High → Max (×11,4, indice 55 → 68). Les coûts API du benchmark sont distingués de la facturation Copilot. Capture et preuves disponibles hors ligne.

Build réussi. Nouvelle slide vérifiée dans le navigateur aux formats 1366×768 et 1920×1080 : aucun débordement interne, capture chargée, marges de 23,6 et 33,2 pixels avant les références. Notes présentateur vérifiées aux deux tailles, sans débordement horizontal; détails ouverts au clavier et repliés après rechargement. Compteur corrigé pour utiliser le nombre réel de slides. Preuves : `../../../docs/validation/thinking-effort.json` et captures `thinking-*`.

PDF régénéré : 30 pages, valeurs, liens et six destinations internes validés; page 17 examinée à pleine taille. Les autres pages sont inchangées selon les rendus générés. Avertissement FloatingVue d’export inchangé, sans effet constaté. Les 24 plages notes/guide correspondent toujours : 32 minutes d’explications + 13 de démos; slide 17 maintenue à 26:30–28:00. La répétition orale sur le portable HQ reste à faire.

## Historique : Thinking Effort et coût par tâche

19 octobre 2026. **30 slides, dont 24 principales et 6 annexes.** Nouvelle slide 17 : niveaux de raisonnement, réglage distinct d’Auto, coût des appels/reprises et lecture du graphique Artificial Analysis. Le schéma ne contient pas de points mesurés. Liens Microsoft, benchmark et méthodologie accessibles dans le site et le PDF.

Build et 30 routes audience validés : aucun débordement, erreur de page ou chargement externe. Interactions contexte/coûts réussies. Rendus audience/présentateur de la 17 examinés en 1366×768 et 1920×1080; marges au-dessus des références de 29 et 41 pixels. Notes en quatre repères, détails ouverts au clavier; pas de débordement horizontal. Le premier chevauchement a été corrigé avant livraison.

PDF : 30 pages, textes et liens attendus, six liens internes convertis en destinations autonomes. Page 17 inspectée à pleine taille, planches 2–3 parcourues; pages précédentes inchangées à part les liens d’annexe. Avertissement FloatingVue connu dans l’export, sans effet constaté sur les rendus. Les 24 plages notes/guide concordent : **32 + 13 minutes**; trois démos aux mêmes horaires. La répétition orale sur le portable HQ reste à faire. Preuves : `../../../docs/validation/thinking-effort.json`, `presentation-flow.json`, `browser-report.json` et captures `thinking-*`.

## Historique : notes du présentateur structurées

19 octobre 2026. Les 29 notes sont ventilées en horloge, fil rouge, intertitres et listes courtes; les précisions et procédures facultatives se déplient. Les démos indiquent le départ de leurs temps relatifs, et la décision de secours à 2:10 reste visible dans le fil de la démo 3. Guide et README expliquent la lecture et le redimensionnement du panneau.

Build réussi. Vérification des 29 routes présentateur : titres/listes rendus, détails fermés initialement, ouverture/fermeture au clavier, aucun débordement horizontal, aucune erreur JavaScript ni requête externe. Captures des slides 7/11/12/20 examinées sur portable 1366×768, et des notes de la 12 en 1600×900; agrandissement en largeur/hauteur et commandes natives de taille de texte fonctionnels. Le défilement vertical reste normal pour les notes longues; ne pas réduire les caractères pour tout afficher.

Les 23 plages notes/guide correspondent toujours : 32 minutes d’explications et 13 de démos. Synchronisation contexte 10/coûts 16 revalidée dans les deux vues. Contenu audience inchangé, comparaison du source hors commentaires; PDF conservé avec la même empreinte SHA-256. Preuves : docs/validation/presenter-notes.json, presentation-flow.json et presenter-sync.json. Répétition orale sur le portable HQ encore à effectuer.

## Historique : skills et outils sur deux slides visuelles

19 octobre 2026. **29 diapositives : 23 principales et 6 annexes.** La slide 11 relie une capture réelle agrandie aux familles de personnalisations; la slide 12 compare découverte/chargement/ressources d’une skill au choix/contrôle/résultat d’un outil. La skill du dépôt reste volontaire. Réglages par défaut, user-invocable et disable-model-invocation détaillés dans la fiche; sources officielles relues.

Les 23 plages du guide correspondent aux notes : 32 minutes d’explications et 13 de démonstrations. Les nouvelles slides totalisent 2 min 45; vanille dure 45 secondes et tokens 1 min 30. L’horloge des trois démonstrations reste inchangée. Numéros actualisés : contexte 10, coûts 16, démos 7/18/20, SDD 19, ressources 23, annexes 24–29.

Build réussi; 29 routes sans débordement détecté, erreur de page ou ressource externe. Le premier rendu de la slide 12 débordait vers les références : espacements corrigés sans diminuer les polices, puis rendu inspecté et contrôle repassé. PDF de 29 pages, six destinations internes et liens officiels vérifiés; pages 11–12 et planches examinées. Avertissement FloatingVue d’export déjà connu, sans défaut constaté dans le PDF final.

Synchronisation présentateur/audience repassée pour contexte 10 et coûts 16 : sélection, valeurs, navigation, réouverture et rechargement. Conducteur : huit rubriques, 21 copies, liens, quatre largeurs et impression contrôlés. Preuves : browser-report.json, presenter-sync.json, docs/validation/presentation-flow.json et presentation-guide.json. Répétition orale sur le portable HQ encore à effectuer.

## Historique : disponibilité et choix des outils

19 octobre 2026. Correction de la slide 11 : Tools indique les outils disponibles pour la tâche. La phrase sous le tableau distingue choix du modèle et contrôle du harnais. La fiche et les notes expliquent les définitions d’outils, le contexte, les paramètres, les permissions et le retour du résultat; terminal/pytest sont distingués. Guide aligné sur le même créneau de deux minutes.

Build réussi; 28 routes sans débordement détecté, erreur de page ou requête externe; interactions existantes réussies. PDF de 28 pages, libellés et liens officiels contrôlés, six destinations internes conservées. Page 11 inspectée à pleine taille : aucun chevauchement, tailles de texte conservées. Notes/guide : 32 + 13 minutes. La synchronisation n’a pas changé; la preuve précédente reste applicable. L’avertissement FloatingVue de l’exporteur demeure sans défaut constaté. Répétition humaine encore requise pour le rythme oral.

## Historique : panneau Agent Customizations

19 octobre 2026. Slide 11 : Overview, Customize Your Agent, puis Plugins, MCP Servers, Skills, Instructions, Agents, Hooks et Tools dans l’ordre de la capture. Chaque rubrique a une définition, une utilité, un exemple et un lien officiel. Explore Customizations, la composition instruction/skill/outil et les compteurs sont expliqués dans les notes et la fiche. Capture réelle du projet en annexe 23, pixels préservés avec cadrage CSS.

Build et contrôle des 28 routes réussis : aucun débordement détecté, erreur de page ou chargement externe. Interactions contexte/coûts réussies; leur synchronisation n’a pas été modifiée et conserve la preuve de la révision précédente. PDF de 28 pages avec les neuf libellés attendus en page 11, liens officiels et six destinations internes. Rendus 11 et 23 examinés à pleine taille; page 8 actualisée pour le libellé de son renvoi. Avertissement FloatingVue de l’exporteur inchangé, sans défaut constaté dans les rendus.

Notes et guide : mêmes 22 plages, 32 minutes d’explications et 13 de démos. Conducteur HTML : huit rubriques, 21 copies, liens, quatre largeurs et impression vérifiés. La démonstration ne lance aucune personnalisation nouvelle depuis Customize Your Agent. Le rythme de deux minutes pour ce survol reste à répéter humainement sur le portable HQ. Les fichiers sources des captures sont inclus dans le kit pour préserver les liens de la fiche.

## Historique : progression Copilot et notes synchronisées

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
