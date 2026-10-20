# Personnaliser avec mesure et rester à jour

Sélection préparée le **19 octobre 2026** pour des analystes BI/Python dans VS Code et GitHub Copilot. Diapositives 11, 12 et 22–24. Aucun skill tiers n’est installé par cette révision du support.

## Notre recommandation « vanille »

Commencer avec les fonctions natives, les commandes du projet, quelques consignes lisibles et des tests. Ajouter une personnalisation lorsque son utilité est précise et répétée. Conserver sa source et sa version, relire les changements, puis mesurer le résultat sur une petite tâche.

Cette recommandation porte sur l’entretien : multiplier les intégrations multiplie les compatibilités et permissions à suivre. L’exemple récent est la [migration des personnalisations vers le harnais Copilot](https://code.visualstudio.com/docs/agent-customization/migrate-customizations). Cela ne démontre pas que toutes les extensions nuisent à la qualité ni que la configuration native reste immuable.

Un skill bien ciblé peut réduire les répétitions. Son contenu se charge progressivement; compter les skills installés ne suffit pas à prédire les tokens consommés. Le format ouvert facilite la réutilisation, mais les outils et emplacements pris en charge varient selon le harnais. [Documentation Microsoft](https://code.visualstudio.com/docs/agent-customization/agent-skills).

Une comparaison utile : la même petite tâche, les mêmes critères, les mêmes données, avec puis sans l’ajout. Observer les erreurs, les reprises, le temps et le coût réellement mesurés. Désactiver un ajout devenu redondant avec une fonction native. Un plugin maintenu peut aussi simplifier la distribution de plusieurs capacités.

## Sélection selon la tâche

| Besoin | Ressource et origine | Essai concret | Statut dans cette formation |
|---|---|---|---|
| Valider un ETL Python | [validate-etl](../.github/skills/validate-etl/SKILL.md), procédure du dépôt | Vérifier unités, tests et exports | Fourni avec HydroDataLab; invocation volontaire |
| Vérifier l’interface Vue | [playwright-cli](https://github.com/microsoft/playwright-cli), Microsoft | Parcourir collecte → résultat et prendre une capture | Candidat pour le navigateur; les tests de notre support utilisent déjà Playwright, pas la preuve d’une installation de ce skill dans Copilot HQ |
| Orchestrer et diagnostiquer | [Skills Aspire](https://aspire.dev/get-started/ai-coding-agents/), documentation officielle | Démarrer puis inspecter santé et journaux | Adapté à notre architecture; configuration à vérifier sur le poste HQ |
| Développer l’API .NET | [dotnet/skills](https://github.com/dotnet/skills), équipe .NET | Consulter les collections dotnet-aspnetcore ou dotnet-test | À choisir selon le composant; inutile de charger tout le catalogue pour une tâche Python |
| Clarifier puis tester | [mattpocock/skills](https://github.com/mattpocock/skills), Matt Pocock | grill-me pour le besoin; tdd pour test en échec → correction → amélioration | **En exploration : le présentateur commence à les découvrir; non validés dans notre démo** |

Playwright est un outil; sa CLI peut être accompagnée d’un skill. Aspire fournit des skills et propose un serveur MCP facultatif. Le dépôt .NET distribue des collections via des plugins; un plugin n’est pas lui-même un skill. TDD signifie *Test-Driven Development*, développement piloté par les tests.

### Adoption : un indice, pas un vote de la majorité

Le répertoire [skills.sh](https://skills.sh/) a été consulté. Il affiche environ **189,6 k installations** pour [playwright-cli](https://skills.sh/microsoft/playwright-cli/playwright-cli), et **1,4 M pour grill-me / 1,1 M pour tdd** dans son classement au 19 octobre. Le dépôt Microsoft affiche 14,3 k étoiles, celui de Matt Pocock 296,9 k et dotnet/skills 5,7 k. Ces compteurs évoluent, ne désignent pas nécessairement des utilisateurs distincts et ne prouvent ni sécurité ni efficacité. Le nombre d’installations comparable n’a pas été établi pour les collections Aspire et .NET; leur sélection repose sur leur origine et leur pertinence pour ce projet. Aucun consensus universel « must have » n’est affirmé.

### Avant de les adopter

Lire le `SKILL.md`, les scripts appelés, les prérequis et les permissions. Confirmer le harnais Copilot et les emplacements actuellement pris en charge dans la documentation VS Code. Les recettes d’un autre harnais ne sont pas automatiquement transposables. Essayer une seule capacité à la fois, dans un projet de test, avec les accès autorisés. Ne pas installer les catalogues en bloc pendant la formation.

Chemins documentés à examiner avant toute installation :

- Playwright : après installation autorisée de la CLI, `playwright-cli install --skills`; prérequis et commandes dans le [README Microsoft](https://github.com/microsoft/playwright-cli#installing-skills).
- Aspire : `aspire agent init`, depuis le projet, selon la [référence de la commande](https://aspire.dev/reference/cli/commands/aspire-agent-init/).
- .NET : sélectionner une collection et suivre la section correspondant à VS Code dans le [dépôt officiel](https://github.com/dotnet/skills#installation); vérifier le menu Customizations du harnais utilisé.
- Matt Pocock : lire les instructions et prérequis du [dépôt de l’auteur](https://github.com/mattpocock/skills), puis n’essayer que le skill choisi. Les procédures peuvent s’intégrer à un suivi d’issues : examiner leurs effets avant invocation.

## Veille et apprentissage

- [Nouveautés VS Code](https://code.visualstudio.com/updates) : lire changements, préversions et migrations.
- [Changelog Copilot](https://github.blog/changelog/label/copilot/) : suivre outils, modèles et retraits.
- [Microsoft Learn](https://learn.microsoft.com/en-us/training/modules/github-copilot-agent-mode/) : un exercice guidé, à adapter aux menus de la version installée.
- [Awesome Copilot](https://github.com/github/awesome-copilot) : exemples communautaires à relire et tester.
- [GitHub Community — Copilot](https://github.com/community/community/discussions/categories/copilot) : échanges et retours d’expérience.
- [Aspire](https://aspire.dev/) et [Spec Kit](https://github.github.com/spec-kit/) : approfondir orchestration et développement piloté par spécifications.

Routine proposée : 15 minutes par semaine pour repérer une évolution utile, puis une expérimentation limitée. Les nouvelles fonctionnalités doivent répondre à un besoin de votre travail.

## Ajouter les ressources internes HQ

Modifier [presentation/resources/links.json](../presentation/resources/links.json), section `internal`. Chaque entrée a trois champs : `label`, `url`, `description`. Remplacer l’URL vide par le lien Teams, SharePoint ou intranet fourni par votre organisation; aucune adresse interne n’a été inventée. Garder trois entrées courtes. Si `url` reste vide, le support affiche « Lien interne à ajouter » sans lien cliquable.

Depuis la racine : `npm --prefix presentation run build`, puis `npm --prefix presentation run export`. Recharger les deux vues du navigateur. Utiliser ensuite la mise à jour légère du kit décrite dans le README de la présentation. Aucun lien interne n’est téléchargé ou consulté automatiquement par le support.
