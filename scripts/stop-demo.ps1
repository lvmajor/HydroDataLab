. "$PSScriptRoot/common.ps1"
Invoke-Checked aspire @('stop','--apphost',$AppHost,'--non-interactive')
Write-Host 'Application arrêtée. Historiques et exports conservés dans .runtime (par branche et moteur).'
