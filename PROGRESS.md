# HydroDataLab — journal de reprise

Mis à jour : 18 octobre 2026. Formation cible : mardi 20 octobre 2026.

## État
- [x] Plan accepté : public BI/Python, formation française, web local + PDF.
- [x] Données publiques capturées et provenance conservée (17 octobre : 168 observations).
- [x] ETL Python, tests, CLI et adaptateur HTTP.
- [ ] API .NET/Quartz/SQLite, Vue et Aspire.
- [ ] Parcours Docker et comparaison avec le mode natif.
- [ ] 24 diapositives, PDF, notes et aide-mémoire.
- [ ] Tags pédagogiques, scripts de reprise et sauvegarde Git.
- [ ] Vérification navigateur et répétition technique.

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
Intégrer les trois services dans Aspire, vérifier le parcours HTTP, puis publier et tester Compose.

## Vérifications
- Python : 24 tests réussis; lecture réelle de la source publique vérifiée.
- API : compilation .NET 10 et 8 tests SQLite réussis.
- Vue : compilation et tests réussis; contrôle visuel en cours.
- Slidev : 24 diapositives, compilation et export PDF produits; contrôle visuel en cours.
- Git : dernier commit enregistré `37408e7`, tag `01-instructions` (socle `3203f1c`).
- Docker Desktop : backend ne démarre pas. Le contexte existant `podman-machine-default` répond; tests de conteneur via ce contexte explicite.
- Ports réservés à cette démo : Vue 53170, API 53171, Python 53172, présentation 53173.

## Préparation sur le portable HQ
La répétition du 19 octobre et la confirmation des politiques/modèles HQ nécessiteront le portable et le compte du présentateur. Elles ne sont pas encore effectuées.
