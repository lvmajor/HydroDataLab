param([ValidateSet('Start','Stop','Status','Logs')][string]$Action = 'Start', [string]$DockerContext, [string]$DockerHost, [ValidateSet('docker','podman')][string]$Engine = 'docker')
. "$PSScriptRoot/common.ps1"
$options = @()
if ($DockerContext -and $DockerHost) { throw 'Choisir DockerContext ou DockerHost, pas les deux.' }
if ($Engine -eq 'podman' -and ($DockerContext -or $DockerHost)) { throw 'Les paramètres Docker ne sont pas utilisés avec Engine podman.' }
if ($DockerContext) { $options += @('--context',$DockerContext) }
if ($DockerHost) { $options += @('--host',$DockerHost) }
$options += @('compose','-p','hydrodatalab-training','--env-file','artifacts/compose/images.env','-f','artifacts/compose/docker-compose.yaml')
if (!(Test-Path 'artifacts/compose/images.env')) { throw 'Exécuter prepare-docker.ps1 avant le lancement.' }
switch ($Action) {
    'Start' {
        Invoke-Checked aspire @('stop','--apphost',$AppHost,'--non-interactive')
        Invoke-Checked $Engine ($options + @('up','-d','--pull','never','--no-build','--wait','--wait-timeout','60'))
        Write-Host 'Vue : http://localhost:53170 — dashboard : http://localhost:53174'
    }
    'Stop' { Invoke-Checked $Engine ($options + @('stop','--timeout','30')) }
    'Status' { Invoke-Checked $Engine ($options + @('ps')) }
    'Logs' { Invoke-Checked $Engine ($options + @('logs','--tail','80')) }
}
