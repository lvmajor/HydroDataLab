# Audit des dépendances de la présentation

Vérifié le 18 octobre 2026 avec `npm audit --json`, les métadonnées du registre npm et les avis GitHub. Cet audit concerne le dossier `presentation/`, pas les dépendances de l’application Vue `web/`.

## Correction appliquée

`overrides.dompurify = 3.4.17` dans `package.json`, avec mise à jour du lock. Cela remplace DOMPurify 3.4.8 apporté par Monaco; Mermaid utilise aussi 3.4.17. Le correctif reste dans la même version majeure. Slidev reste en 53.0.0.

L’audit passe de **19 entrées signalées (9 moderate, 10 high)** à **17 (6 moderate, 11 high)**. Ce décompte inclut les packages parents affectés par propagation et ne représente pas 17 failles indépendantes. La classification d’un parent peut changer avec la résolution de l’arbre; il ne faut pas lire ces nombres comme un score de sécurité. Rapports intégraux conservés dans `exports/qa/npm-audit-before.json` et `npm-audit-after.json`.

## Risques qui restent

| Dépendance source | Alerte et état du correctif | Portée dans ce projet |
|---|---|---|
| `braces@3.0.3` | Déni de service par motifs profondément imbriqués; aucune version corrigée publiée au contrôle | Chemins `fast-glob/micromatch` et `chokidar`; construction et surveillance des fichiers locaux |
| `sprintf-js@1.0.3` | Déni de service par précision de format non bornée; aucune version corrigée publiée | Chemin `gray-matter → js-yaml → argparse`, utilisé par l’outillage Markdown |
| `image-size@1.2.1` | Boucles infinies dans des parseurs d’images; correctif disponible en 2.x, hors plage majeure demandée par le parent | Dépendance de `pptxgenjs`; export PowerPoint non utilisé et non livré dans cette formation |

Les entrées supplémentaires de l’audit sont les parents de ces chaînes. Aucune dérogation d’audit n’a été ajoutée pour les cacher. Le correctif forcé proposé par npm rétrograde Slidev vers une autre version majeure; il n’a pas été appliqué. Un remplacement majeur image-size n’a pas non plus été imposé à PptxGenJS.

Ces composants restent installés : l’absence d’un chemin d’usage dans la formation ne corrige pas la dépendance. L’impact attendu est surtout une interruption des outils de construction/édition face à des entrées malveillantes. Le diaporama livré ne demande aucune importation de Markdown, de motif ou d’image fournis par le public. Préférer `npm run preview` pour projeter le build déjà contrôlé, ou le PDF. Réévaluer l’audit avant d’étendre l’outil à des contenus non fiables ou à un service partagé.

## Vérifications locales

- `npm ls dompurify --all` confirme 3.4.17 pour Monaco et Mermaid.
- `npm run build` réussit après mise à jour.
- Le script `dev` ne contient ni `--remote` ni `--tunnel`. `slidev --help` indique que `--remote` active l’écoute publique; un démarrage réel a été observé sur **::1:53173 uniquement**, puis arrêté.
- `preview` utilise explicitement **127.0.0.1:53173**.
- La validation navigateur après build est consignée dans `exports/qa/browser-report.json`.
- Aucun contenu de diapositive n’a changé pendant ce correctif; le PDF contrôlé précédemment reste valable.

## Sources

- [Braces — avis GitHub](https://github.com/advisories/GHSA-vfj7-8cjw-p6xm)
- [sprintf-js — avis GitHub](https://github.com/advisories/GHSA-hp3w-g68c-fv3c)
- [image-size JXL/HEIF — avis GitHub](https://github.com/advisories/GHSA-5p2g-fcmc-qvqq)
- [image-size ICNS — avis GitHub](https://github.com/advisories/GHSA-w3rx-r6r6-pgpr)
- [DOMPurify — avis GitHub](https://github.com/advisories/GHSA-55q2-fjhq-7xh7)

Les versions disponibles ont été contrôlées avec `npm view <package> version dist-tags --json`; l’état du registre peut évoluer.
