param([ValidateSet('docker','podman')][string]$Engine = 'docker', [string]$DockerContext, [string]$DockerHost)
. "$PSScriptRoot/common.ps1"
$dockerOptions = @()
if ($DockerContext -and $DockerHost) { throw 'Choisir DockerContext ou DockerHost, pas les deux.' }
if ($Engine -eq 'podman' -and ($DockerContext -or $DockerHost)) { throw 'Les paramètres Docker ne sont pas utilisés avec Engine podman.' }
if ($DockerContext) { $dockerOptions += @('--context',$DockerContext) }
if ($DockerHost) { $dockerOptions += @('--host',$DockerHost) }
Invoke-Checked $Engine ($dockerOptions + @('version','--format','{{.Server.Version}}'))
# Arrêter seulement cet AppHost : Windows verrouille son exécutable pendant la publication.
Invoke-Checked aspire @('stop','--apphost',$AppHost,'--non-interactive')
Invoke-Checked aspire @('publish','--apphost',$AppHost,'-o','artifacts/compose','--non-interactive')
$buildOptions = @('--load')
if ($Engine -eq 'podman') { $buildOptions = @('--format','docker') }
Invoke-Checked $Engine ($dockerOptions + @('build') + $buildOptions + @('-t','docker.io/library/hydrodatalab-etl:formation','-f','etl/Dockerfile','.'))
Invoke-Checked $Engine ($dockerOptions + @('build') + $buildOptions + @('-t','docker.io/library/hydrodatalab-api:formation','-f','src/Api/Dockerfile','.'))
Invoke-Checked $Engine ($dockerOptions + @('build') + $buildOptions + @('-t','docker.io/library/hydrodatalab-web:formation','web'))
Invoke-Checked $Engine ($dockerOptions + @('pull','mcr.microsoft.com/dotnet/aspire-dashboard:13.6.1'))
@('PYTHON_IMAGE=docker.io/library/hydrodatalab-etl:formation','API_IMAGE=docker.io/library/hydrodatalab-api:formation','WEB_IMAGE=docker.io/library/hydrodatalab-web:formation') |
    Set-Content -LiteralPath 'artifacts/compose/images.env' -Encoding ascii
Invoke-Checked $Engine ($dockerOptions + @('compose','-p','hydrodatalab-training','--env-file','artifacts/compose/images.env','-f','artifacts/compose/docker-compose.yaml','config','--quiet'))
Write-Host 'Images et modèle prêts. Lancer scripts/docker-demo.ps1 avec le même DockerContext.'
