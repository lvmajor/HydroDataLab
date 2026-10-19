# Synchroniser HydroDataLab entre les deux postes

Dépôt privé : **https://github.com/lvmajor/HydroDataLab**. Le compte utilisé sur le portable doit y avoir accès. Git peut ouvrir son écran de connexion lors du premier accès. Si GitHub CLI est installé, `gh auth login --hostname github.com --git-protocol https --web` permet aussi de se connecter; ne jamais insérer un jeton dans l’URL.

GitHub contient `main`, `codex/preparation-demo-06` et les huit tags `00-socle` à `07-solution`. La branche locale `demo-live` n’est pas publiée. Le ZIP déjà transféré reste utile pour les images Docker et la présentation de secours. `artifacts/`, `node_modules/`, `.venv/`, `.runtime/` et les sites compilés ne sont pas synchronisés par Git.

## 1. Connecter le dépôt déjà restauré sur le portable HQ

Dans PowerShell, adapter seulement le chemin `$Depot`. C’est le sous-dossier **HydroDataLab** contenant `.git`, créé à l’étape 4 du guide du ZIP, et non la racine du ZIP ni la copie d’exercice. Arrêter les serveurs de présentation et de démo dans leurs terminaux avant de mettre leurs fichiers à jour. Les lancer de nouveau après la synchronisation.

Le bloc vérifie d’abord les changements locaux. Il conserve l’ancien lien au bundle sous le nom `kit`, sans déplacer les fichiers ou remplacer l’historique. Il peut être rejoué si `origin` pointe déjà vers le bon dépôt.

```powershell
$Depot = 'C:\Formation\HydroDataLab-HQ\HydroDataLab'
$Url = 'https://github.com/lvmajor/HydroDataLab.git'
$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $Depot
if (!(Test-Path -LiteralPath '.git')) { throw 'Ouvrir le depot Git restaure, pas la racine du ZIP.' }
$modifications = @(git status --porcelain)
if ($LASTEXITCODE -ne 0) { throw 'Impossible de lire le depot Git.' }
if ($modifications.Count) { throw 'Modifications locales presentes : les enregistrer avant de continuer.' }

$remotes = @(git remote)
if ($remotes -contains 'origin') {
    $ancienOrigin = git remote get-url origin
    if ($LASTEXITCODE -ne 0) { throw 'Impossible de lire origin.' }
    if ($ancienOrigin -ne $Url) {
        if ($ancienOrigin -notmatch '\.bundle$') { throw 'origin ne pointe pas vers le bundle attendu : examiner git remote -v.' }
        if ($remotes -contains 'kit') { throw 'Le nom kit existe deja : examiner git remote -v.' }
        git remote rename origin kit
        if ($LASTEXITCODE -ne 0) { throw 'Renommage du distant interrompu.' }
    }
}
if (@(git remote) -notcontains 'origin') {
    git remote add origin $Url
    if ($LASTEXITCODE -ne 0) { throw 'Ajout du distant interrompu.' }
}
git fetch origin --tags
if ($LASTEXITCODE -ne 0) { throw 'Recuperation interrompue : verifier connexion et acces au depot.' }
git switch main
if ($LASTEXITCODE -ne 0) { throw 'Impossible de selectionner main.' }
git branch --set-upstream-to=origin/main main
if ($LASTEXITCODE -ne 0) { throw 'Configuration du suivi interrompue.' }
git pull --ff-only
if ($LASTEXITCODE -ne 0) { throw 'Historique divergent : conserver les deux versions et examiner les commits.' }
git status --short --branch
git remote -v
```

**Attendu :** `main` suit `origin/main`, aucun changement local, `origin` désigne GitHub et `kit` désigne encore le bundle. Si les historiques divergent, le bloc s’arrête sans fusion ni réécriture. Ne pas utiliser de reset destructif ou de push forcé pour contourner ce contrôle.

Si le dépôt n’a pas encore été restauré, cloner directement GitHub dans une destination inexistante, puis reprendre la préparation des dépendances du guide HQ :

```powershell
git clone https://github.com/lvmajor/HydroDataLab.git 'C:\Formation\HydroDataLab-HQ\HydroDataLab'
```

## 2. Lancer la présentation actualisée depuis ce dépôt

Le site compilé à la racine du ZIP reste une copie figée. Pour travailler sur la version GitHub, lancer Slidev **depuis le dépôt**. Arrêter d’abord l’ancien serveur sur le port 53173 avec `Ctrl+C` dans son terminal.

Première préparation, avec accès réseau :

```powershell
Set-Location -LiteralPath $Depot
$env:PLAYWRIGHT_SKIP_BROWSER_DOWNLOAD = '1'
npm.cmd --prefix presentation ci
if ($LASTEXITCODE -ne 0) { throw 'Installation des dependances interrompue.' }
npm.cmd --prefix presentation run dev
```

Audience : **http://localhost:53173/1**. Présentateur : **http://localhost:53173/presenter/1**. Garder le même navigateur, profil, nom d’hôte et port pour synchroniser les interactions. En édition, Slidev recharge les modifications.

Avant la séance, arrêter le serveur d’édition avec `Ctrl+C`, puis préparer et servir la version compilée :

```powershell
npm.cmd --prefix presentation run build
if ($LASTEXITCODE -ne 0) { throw 'Compilation interrompue.' }
./scripts/start-presentation.ps1
```

Si vous modifiez les diapositives, actualiser aussi le PDF avec `npm.cmd --prefix presentation run export` après avoir préparé Chrome/Edge et Python avec `pypdf`, comme indiqué dans `presentation/README.md`.

## 3. Recevoir les modifications d’un autre poste

Dans le dépôt principal propre, sur `main`, avant de recommencer à travailler :

```powershell
git status --short --branch
git pull --ff-only
```

Si `git status` montre des modifications, les enregistrer d’abord. Si le pull indique une divergence, conserver les commits et résoudre la divergence avant de poursuivre. Après changement de `presentation/package-lock.json`, rejouer `npm.cmd --prefix presentation ci`; avant projection, refaire le build. Pour les dépendances de l’application, utiliser les scripts de préparation du projet.

## 4. Envoyer les retouches de présentation

Sur le poste où les retouches ont été faites, après validation du site et actualisation du PDF :

```powershell
git status --short --branch
git diff
git add -- presentation/slides.md handouts/aide-memoire.md
git diff --cached
git commit -m 'Ajuster la presentation'
git push origin main
```

La commande `git add` est un exemple : ajouter les fichiers réellement modifiés, y compris les composants ou le PDF si nécessaire. Relire les changements sélectionnés. En cas de rejet du push, conserver les commits locaux et examiner les changements distants. Éviter de retoucher le même fichier simultanément sur les deux postes.

## 5. Conserver l’exercice séparé de la solution

`main` contient la présentation actuelle et la solution. `codex/preparation-demo-06` conserve la couverture Python à implémenter. **Ne pas fusionner main dans la branche d’exercice** pour mettre à jour les slides : cela pourrait apporter la solution.

Pour créer une nouvelle copie d’exercice dans un dossier inexistant :

```powershell
git clone --branch codex/preparation-demo-06 https://github.com/lvmajor/HydroDataLab.git 'C:\Formation\HydroDataLab-HQ\HydroDataLab-exercice'
```

Dans cette copie, créer sa branche de répétition (`git switch -c codex/repetition-02`, avec un nom libre), puis suivre le conducteur des démos de `main`. Une copie d’exercice déjà modifiée doit être conservée. Les tags pédagogiques restent des repères historiques fixes.

Références : [Git remote](https://git-scm.com/docs/git-remote), [Git pull et --ff-only](https://git-scm.com/docs/git-pull), [authentification GitHub CLI](https://cli.github.com/manual/gh_auth_login).
