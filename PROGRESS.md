# HydroDataLab — journal de reprise

Mis à jour : 18 octobre 2026. Formation cible : mardi 20 octobre 2026.

## État
- [x] Plan accepté : public BI/Python, formation française, web local + PDF.
- [ ] Données publiques capturées et provenance conservée.
- [ ] ETL Python, tests, CLI et adaptateur HTTP.
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
Initialiser Git et les contrats communs, puis développer les composants en parallèle.

## Vérifications
Aucune validation d'application exécutée à ce stade.

## Préparation sur le portable HQ
La répétition du 19 octobre et la confirmation des politiques/modèles HQ nécessiteront le portable et le compte du présentateur. Elles ne sont pas encore effectuées.
