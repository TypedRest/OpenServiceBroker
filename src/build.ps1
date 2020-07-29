Param ($Version = "1.0-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

if ($env:CI) { $ci = "-p:ContinuousIntegrationBuild=True" }
dotnet msbuild -v:Quiet -t:Restore -t:Build $ci -p:Configuration=Release -p:Version=$Version
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

popd
