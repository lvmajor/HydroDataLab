# HydroDataLab — journal de reprise

Mis à jour : 18 octobre 2026. Formation cible : mardi 20 octobre 2026.

## État
- [x] Plan accepté : public BI/Python, formation française, web local + PDF.
- [x] Données publiques capturées et provenance conservée (17 octobre : 168 observations).
- [x] ETL Python, tests, CLI et adaptateur HTTP.
- [x] API .NET/Quartz/SQLite, Vue et Aspire, parcours natif intégré.
- [x] Parcours Docker Compose et comparaison avec le mode natif.
- [x] 24 diapositives, PDF, notes et aide-mémoire contrôlés.
- [ ] Tags pédagogiques, scripts de reprise et sauvegarde Git.
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
Produire le bundle Git, vérifier une restauration réelle et terminer l'archive des images. La version finale Python a été restaurée après validation du jalon 06 volontairement incomplet.

## Vérifications
- Python : 35 tests réussis; lecture réelle de la source publique vérifiée; date CLI, idempotence, changements d'heure et interruptions couverts.
- API : compilation .NET 10 et 8 tests SQLite réussis.
- Vue : compilation et 11 tests réussis; parcours Edge complet et mobile validé, captures dans docs/screenshots.
- HTTP natif : collecte, scénario manquant 95,83 %, échec/relance, trois exports vérifiés.
- Slidev : 24 diapositives, aucun débordement ni requête externe; interactions vérifiées, PDF produit.
- Git : jalons 00 à 06 enregistrés; `05-docker` = `b01c5c4`, `06-specification` = `9f92fe3`. Branche `demo-live` créée depuis 06. Solution 07 en cours d'enregistrement.
- Image ETL finale testée sans réseau : CSV identique, Parquet et JSON métier équivalents au mode natif.
- Aspire : publication Compose réussie; images API, Vue, ETL et dashboard téléchargées/construites.
- 16:16 : Compose sain et parcours HTTP complet validé (collecte, couverture, panne, relance, exports). Le blocage venait de la liaison named pipe Docker; construction Podman directe puis API temporaire localhost dans le namespace systemd. Aucun service global redémarré. Voir docs/VALIDATION.md.
- Persistance réelle : restart API conserve dernier rapport et planification; planification désactivée après vérification.
- Préparation native exécutée avec succès; audit résiduel des outils Slidev documenté dans presentation/DEPENDENCY-AUDIT.md.
- Scripts de reprise : runtime séparé par branche/tag; changements Git refusés si modifications locales.
- Jalon 06 vérifié : deux acceptations volontairement rouges, CLI fonctionnelle avec 168 observations et couverture absente. Solution restaurée : 35 tests Python verts.
- Compose : 14 contrôles navigateur réussis, aucune requête externe du navigateur. Conteneurs arrêtés proprement après les tests; volumes conservés.
- Docker Desktop : backend ne démarre pas. Le contexte existant `podman-machine-default` répond; tests de conteneur via ce contexte explicite.
- Ports réservés à cette démo : Vue 53170, API 53171, Python 53172, présentation 53173.

## Préparation sur le portable HQ
La répétition du 19 octobre et la confirmation des politiques/modèles HQ nécessiteront le portable et le compte du présentateur. Elles ne sont pas encore effectuées.
