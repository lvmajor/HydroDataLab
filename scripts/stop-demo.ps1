. "$PSScriptRoot/common.ps1"
Invoke-Checked aspire @('stop','--apphost',$AppHost,'--non-interactive')
Write-Host 'Application arrêtée. Historique et exports conservés dans .runtime/main.'
