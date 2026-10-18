$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Set-Location -LiteralPath $root
Write-Host 'HydroDataLab — diagnostic des prérequis'
foreach ($tool in @('git','dotnet','aspire','node','npm.cmd','uv')) {
    $command = Get-Command $tool -ErrorAction SilentlyContinue
    if ($command) { Write-Host ('OK  ' + $tool + ' : ' + $command.Source) }
    else { Write-Warning ('Absent : ' + $tool) }
}
& dotnet --version
& aspire --version
& node --version
& uv --version
Write-Host 'Docker est facultatif pour le mode natif.'
if (Get-Command docker -ErrorAction SilentlyContinue) { & docker version --format '{{.Server.Version}}' }
Write-Host 'Vérifier dans VS Code les modèles autorisés par le compte HQ. Ce script ne modifie aucun réglage.'
