# Livraisons et stockage

Le dossier `artifacts` n’est pas versionné : il contient les kits transportables et les sauvegardes. L’application et les sources de la formation restent dans Git.

## Audit du 19 octobre 2026

Avant ménage : **11,13 Go** (10,37 Gio). Huit kits complets répétaient la même archive de quatre images Docker, **1,205 Go** par copie. Le code et le PDF n’expliquaient qu’une petite part de ce volume. Ces copies avaient été produites après les révisions pour faciliter la reprise; conserver huit jeux identiques d’images était inutile.

Le présentateur a autorisé la suppression des sept kits du 18 octobre. Le contrôle des chemins et de la sauvegarde conservée précède leur suppression. **8,57 Go libérés**; **2,56 Go restants** avant les nouvelles petites sauvegardes. Le détail exact est dans [storage-audit.json](validation/storage-audit.json).

Sont conservés : le kit du 19 octobre, l’archive Docker source, Compose, tous les bundles et ZIP Git. Les anciens bundles précédant la réparation des données ne doivent pas servir à présenter; leur statut reste signalé dans `artifacts/LIVRAISON.txt`.

## Quelle commande utiliser?

- Transfert allégé vers le portable HQ : `node presentation/scripts/build-hq-guide.mjs`, construire le site courant, enregistrer dans Git, puis `python scripts/package-transfer.py`. Produit un ZIP unique avec guide, présentation, PDF, fiches, données/provenance, un bundle Git et les quatre images consolidées. Les anciennes sauvegardes, caches et dépendances installées sont exclus; le TAR est compressé directement sans copie intermédiaire. Lire `packaging/hq/LIRE-MOI-HQ.html` pour la préparation du portable. Le ZIP voisin de son fichier `.sha256` est le livrable à transférer; la validation est enregistrée dans `.zip.validation.json`.
- Correction du support ou des fiches : construire, vérifier, enregistrer dans Git, puis `./scripts/refresh-kit.ps1 -KitPath artifacts/kit-hq-20261019-101734`. L’archive Docker est vérifiée mais ne bouge pas. Une sauvegarde Git, le support et l’inventaire sont mis à jour dans le kit existant.
- Nouvelle version de l’application ou des images : produire et vérifier les images, puis `./scripts/package-kit.ps1` pour une livraison complète distincte.
- Simple point de reprise du code : commit Git; `./scripts/backup.ps1` si une sauvegarde transportable est nécessaire.

Le rafraîchissement conserve les petits anciens bundles et les fichiers compilés portant un ancien hash. La page d’entrée référence les ressources actuelles. Il ne duplique pas l’archive de 1,21 Go. Il peut être relancé après une interruption, une fois le dépôt vérifié et enregistré. Une archive complète distincte n’est donc plus nécessaire à chaque correction de slide.

Garder une copie du kit validé sur un autre support avant la séance : plusieurs copies sur le même disque ne protègent pas d’une panne du disque.
