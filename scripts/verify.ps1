param([switch]$WithHttp, [ValidateSet('python','dotnet')][string]$EtlEngine)
. "$PSScriptRoot/common.ps1"
$EtlEngine = Resolve-EtlEngine $EtlEngine
if ($EtlEngine -eq 'python') {
    New-Item -ItemType Directory -Force 'etl/.test-output' | Out-Null
    $pythonTests = @('run','--project','etl','--frozen','pytest','etl/tests')
    if (Test-Path 'specs/test_coverage_acceptance.py') { $pythonTests += 'specs/test_coverage_acceptance.py' }
    Invoke-Checked uv ($pythonTests + @('--basetemp','etl/.test-output/verify'))
} else { Invoke-Checked dotnet @('test','tests/Etl.Tests','--no-restore','--verbosity','quiet') }
Invoke-Checked dotnet @('test','tests/Api.Tests','--no-restore','--verbosity','quiet')
Invoke-Checked npm.cmd @('run','test','--prefix','web')
Invoke-Checked npm.cmd @('run','build','--prefix','web')
if ($WithHttp) { & "$PSScriptRoot/verify-http.ps1" }
Write-Host 'Vérifications réussies. Au tag 06-specification, les tests de couverture sont volontairement rouges.'
