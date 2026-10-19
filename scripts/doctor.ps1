param([switch]$WithDocker, [string]$DockerContext)
$ErrorActionPreference = 'Stop'
# Keep the bootstrap message ASCII: Windows PowerShell 5.1 misreads BOM-less UTF-8.
Write-Host "PowerShell $($PSVersionTable.PSVersion) - $($PSVersionTable.PSEdition)"
if ($PSVersionTable.PSVersion.Major -lt 7) {
    throw 'PowerShell 7 requis pour ce projet. Ouvrir pwsh, puis relancer ./scripts/doctor.ps1.'
}
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
if ($WithDocker -and (Get-Command docker -ErrorAction SilentlyContinue)) {
    if ($DockerContext) { & docker --context $DockerContext version --format '{{.Server.Version}}' }
    else { & docker version --format '{{.Server.Version}}' }
}
Write-Host 'Vérifier dans VS Code les modèles autorisés par le compte HQ. Ce script ne modifie aucun réglage.'
