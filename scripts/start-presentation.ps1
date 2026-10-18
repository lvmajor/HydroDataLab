param([switch]$Edit)
. "$PSScriptRoot/common.ps1"
Write-Host 'Présentation : http://localhost:53173 — notes présentateur : /presenter'
$mode = 'preview'
if ($Edit) { $mode = 'dev' }
if (!$Edit -and !(Test-Path 'presentation/dist/index.html')) { throw 'Exécuter prepare.ps1 ou npm run build --prefix presentation avant de présenter.' }
Invoke-Checked npm.cmd @('run',$mode,'--prefix','presentation')
