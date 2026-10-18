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
- [ ] Assemblage final du kit de transfert hors ligne.
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
Assembler le kit de transfert avec le nouveau bundle, le code, Compose, les images, le diaporama statique et le PDF. Les six jalons ETL du bundle réparé ont été exécutés dans une copie restaurée, avec les dépendances en mode hors ligne.

Dernier jalon technique validé : `07-solution` = `4a462eec09f6f2cc62b4ec0ca36fa4a6a033bd78`. `demo-live` pointe sur `06-specification` = `114d57d137c4064ab9617bb736e187421c83afdc`. Les commits de livraison suivants portent sur les scripts de transport et la documentation.

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

## Préparation sur le portable HQ
La répétition du 19 octobre et la confirmation des politiques/modèles HQ nécessiteront le portable et le compte du présentateur. Elles ne sont pas encore effectuées.
