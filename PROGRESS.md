# HydroDataLab — journal de reprise

Mis à jour : 18 octobre 2026. Formation cible : mardi 20 octobre 2026.

## État
- [x] Plan accepté : public BI/Python, formation française, web local + PDF.
- [x] Données publiques capturées et provenance conservée (17 octobre : 168 observations).
- [x] ETL Python, tests, CLI et adaptateur HTTP.
- [x] API .NET/Quartz/SQLite, Vue et Aspire, parcours natif intégré.
- [x] Parcours Docker Compose et comparaison avec le mode natif.
- [x] 24 diapositives, PDF, notes et aide-mémoire contrôlés.
- [x] Huit tags annotés, branche demo-live, scripts de reprise et restauration Git réelle.
- [x] Quatre images archivées avec manifest et empreinte SHA-256.
- [x] Kit de transfert hors ligne assemblé; images, bundle et inventaire SHA-256 vérifiés.
- [x] Vérification navigateur et répétition technique sur le poste de construction.

## Décisions
- Python porte les transformations. .NET pilote et conserve l'historique; Vue présente les résultats.
- Aspire 13.6.1, .NET 10, Quartz avec stockage SQLite.
- Les niveaux en mètres ne sont pas des volumes ou des stocks énergétiques.
- Aucun envoi vers un dépôt distant ni déploiement cloud.
- Données locales par défaut; accès direct aux données publiques explicite.

## Reprise
Lire ce journal, puis `git status --short --branch` et `git log -5 --oneline`.
Ne pas écraser les modifications existantes. Le travail des agents est réparti entre `etl/`, `web/`, `presentation/` et le reste du dépôt.

## Prochaine action
Révision pédagogique terminée et contrôlée : notions expliquées avant emploi, consignes d'animation dans les notes, site et PDF reconstruits. Copier le dossier `artifacts/kit-hq-<date-heure>/` le plus récent sur le portable HQ; `artifacts/LIVRAISON.txt` identifie le kit de référence. Répéter le 19 octobre et confirmer les accès Copilot. La version technique de l'application reste validée.

Dernier jalon technique validé : `07-solution` = `4a462eec09f6f2cc62b4ec0ca36fa4a6a033bd78`. `demo-live` pointe sur `06-specification` = `114d57d137c4064ab9617bb736e187421c83afdc`. Les commits de livraison suivants portent sur les scripts de transport et la documentation.

Dernier commit d'outillage validé : `5e5b476` (assemblage du kit). Le commit de clôture suivant ajoute ce bilan et la preuve navigateur. `git log -1` donne la version exacte de livraison.

## Vérifications
- Python : 35 tests réussis; lecture réelle de la source publique vérifiée; date CLI, idempotence, changements d'heure et interruptions couverts.
- API : compilation .NET 10 et 8 tests SQLite réussis.
- Vue : compilation et 11 tests réussis; parcours Edge complet et mobile validé, captures dans docs/screenshots.
- HTTP natif : collecte, scénario manquant 95,83 %, échec/relance, trois exports vérifiés.
- Slidev : 24 diapositives, aucun débordement ni requête externe; interactions vérifiées, PDF produit.
- Git : huit jalons annotés de 00 à 07, histoire linéaire, branche `demo-live` au jalon 06. Aucun dépôt distant.
- Image ETL finale testée sans réseau : CSV identique, Parquet et JSON métier équivalents au mode natif.
- Aspire : publication Compose réussie; images API, Vue, ETL et dashboard téléchargées/construites.
- 16:16 : Compose sain et parcours HTTP complet validé (collecte, couverture, panne, relance, exports). Le blocage venait de la liaison named pipe Docker; construction Podman directe puis API temporaire localhost dans le namespace systemd. Aucun service global redémarré. Voir docs/VALIDATION.md.
- Persistance réelle : restart API conserve dernier rapport et planification; planification désactivée après vérification.
- Préparation native exécutée avec succès; audit résiduel des outils Slidev documenté dans presentation/DEPENDENCY-AUDIT.md.
- Scripts de reprise : runtime séparé par branche/tag; changements Git refusés si modifications locales.
- Jalon 06 vérifié : deux acceptations volontairement rouges, CLI fonctionnelle avec 168 observations et couverture absente. Solution restaurée : 35 tests Python verts.
- Compose : 14 contrôles navigateur réussis, aucune requête externe du navigateur. Conteneurs arrêtés proprement après les tests; volumes conservés.
- Docker Desktop : backend indisponible; client Podman direct et API temporaire locale utilisés pour les validations. Ne pas dépendre de la liaison named pipe sur ce poste.
- Ports réservés à cette démo : Vue 53170, API 53171, Python 53172, présentation 53173.
- Réparation autorisée explicitement le 18 octobre : Git avait normalisé les fins de ligne des données brutes. Les jalons 02 à 07 conservent maintenant leurs octets exacts et leurs empreintes; tags et branche live mis à jour atomiquement. Ancienne sauvegarde conservée, mais ne pas l'utiliser pour présenter.
- Restauration réelle : six CLI réussies, intégrité des sources vérifiée à chaque jalon; 06 présente deux échecs d'acceptation attendus; 07 passe les 35 tests Python. Preuves dans `docs/validation/`.
- Archive d'images : 1 205 382 656 octets, quatre IDs vérifiés; SHA-256 `3c3c775373607b6f6cec5f2bdea7cc9506bfe4c5182e9df04bfce8ea4690ebfe`.
- Kit : code et histoire Git, images, Compose complet, données/licences, présentation statique, PDF, notes et captures; chaque fichier est inventorié dans `SHA256SUMS.json`. Aucune dépendance npm n'est requise pour sa projection.
- Serveur Python du kit : 24 routes de diapositives, deux interactions et `/presenter/1` vérifiés avec les accès externes bloqués; aucune erreur JavaScript. Preuve `docs/validation/kit-browser.json`.

## Révision pédagogique du 18 octobre

- Introduction de modèle/outils/agent puis dépôt/commit/branche/worktree; worktree explicitement facultatif.
- Démonstrations orientées vers une question et un résultat pour l'audience. Minutages, raccourcis et secours conservés dans les notes et le guide.
- Définitions de session, instructions, skill, contexte, token, API, image Docker et spécification; lexique dans l'aide-mémoire.
- Guides et requêtes alignés, notamment `/plan` dans la fenêtre Agents. Les huit tags historiques sont conservés; utiliser les supports récents de main/du kit pendant les démos.
- Site, PDF de 24 pages et interactions contrôlés. Lien direct `/presenter/17` réparé et notes réellement vérifiées; preuve `docs/validation/pedagogy.json`.

## Préparation sur le portable HQ
La répétition du 19 octobre et la confirmation des politiques/modèles HQ nécessiteront le portable et le compte du présentateur. Elles ne sont pas encore effectuées.

## Révision des personnalisations et nouveautés 1.142

- Point de reprise : main `3e58862`, dépôt propre avant cette révision; huit tags pédagogiques conservés.
- Sources officielles revérifiées le 18 octobre : harnais Copilot/SDK, instructions, migration, plugins, hooks, Canvas, tarifs et HydraFusion.
- Décisions : conserver 18 diapositives + 6 annexes et 45 minutes; expliquer composition et activation sans inventer un ordre universel de priorité. Parcours Copilot en priorité; AGENTS.md pour le socle partagé entre harnais compatibles.
- Réalisé : exemple numérique zéro/absence, sources cliquables, composition animée Python/Vue, sept concepts, capture utilisateur reliée aux portées, graphique avec quatre hypothèses de tâches, Canvas et SDD. Notes, minutage et trois fiches synchronisés; nouvelle fiche `handouts/personnalisations-vscode.md`.
- Site validé : 24 routes, aucune erreur ni ressource externe, aucun débordement; sélection de règles, quatre scénarios de coût, tentatives, réduction des animations et notes présentateur contrôlés.
- PDF : 24 pages inspectées visuellement; liens officiels vérifiés. Les liens d’annexes sont convertis en destinations PDF locales par `fix_pdf_links.py`; export reproductible avec Python/pypdf.
- Révision validée enregistrée : `434d66a`. Le commit de clôture ajoute ce bilan. Assemblage du kit confirmé : 216 fichiers, archive d’images et bundle Git vérifiés, quatre renvois PDF internes fonctionnels sans serveur.
- Prochaine action : copier le kit indiqué dans `artifacts/LIVRAISON.txt` sur le portable et un support distinct; effectuer la répétition et vérifier le compte HQ le 19 octobre. Les supports les plus récents restent sur main; les huit tags pédagogiques sont conservés.

## Révision ciblée : couches bilingues et comparaison simultanée

- Départ : main `359f7f1`, dépôt propre. Demande : clarifier les termes et la place visuelle des couches; comparer les tâches et modèles sans devoir choisir une seule tâche.
- Réalisé : diapositive 10 avec cinq couches bilingues, exemples de fichiers, conditions d’utilisation et flèches vers le contexte du modèle; diapositive 13 avec quatre tâches et cinq modèles visibles simultanément, valeurs, barres comparables et recommandations. Tarifs officiels revérifiés le 18 octobre, hypothèses inchangées.
- Correction du chevauchement signalé : en-têtes raccourcis, volumes dans leur cellule, ligne de conseil redondante retirée, espace réservé aux hypothèses et aux sources. Les cinq états de la diapositive 10 sont également vérifiés.
- Validations réussies : build Slidev; 24 routes sans débordement ni erreur JavaScript, accès externes bloqués; 20 coûts et multiplicateur contrôlés; PDF de 24 pages avec liens officiels et renvois internes, pages 10/13 inspectées. Notes, aide-mémoire et fiches synchronisés. Preuve : `docs/validation/context-cost-comparison.json`.
- Point de reprise : cette révision est enregistrée dans le commit portant « Clarifie les couches de contexte et compare les coûts sans chevauchement ». Le script `package-kit.ps1` assemble ensuite la sauvegarde transportable; son résultat et le SHA du commit sont consignés dans `artifacts/LIVRAISON.txt`. Les huit tags pédagogiques restent inchangés.
- Prochaine action : transférer le kit indiqué par ce reçu, puis répéter sur le portable HQ le 19 octobre.
