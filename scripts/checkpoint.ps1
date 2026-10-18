param(
    [Parameter(Mandatory)][ValidateSet('00-socle','01-instructions','02-extraction','03-transformation-bi','04-pilotage','05-docker','06-specification','07-solution')][string]$Tag,
    [string]$Branch
)
. "$PSScriptRoot/common.ps1"
$state = & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" status --porcelain
if ($LASTEXITCODE -ne 0) { throw 'Impossible de lire Git.' }
if ($state) { throw 'Modifications locales présentes. Les conserver dans un commit ou un stash avant de changer de jalon. Aucun fichier modifié.' }
# Arrêt ciblé, sans supprimer les volumes ni les exports. Ce script est chargé avant le checkout.
if (Test-Path $AppHost) { Invoke-Checked aspire @('stop','--apphost',$AppHost,'--non-interactive') }
if ($Branch) {
    & git -c "safe.directory=$($RepoRoot.Replace('\','/'))" show-ref --verify --quiet "refs/heads/$Branch"
    if ($LASTEXITCODE -eq 0) { Invoke-RepoGit @('switch',$Branch) }
    else { Invoke-RepoGit @('switch','-c',$Branch,$Tag) }
    Write-Host "Branche $Branch sélectionnée (une branche existante conserve sa progression)."
}
else { Invoke-RepoGit @('switch','--detach',$Tag) }
Write-Host "Base demandée : $Tag. Les données de travail .runtime et outputs sont conservées."
Write-Host 'Voir docs/CHECKPOINTS.md sur main pour les commandes disponibles à chaque étape.'
