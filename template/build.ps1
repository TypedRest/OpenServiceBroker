Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

pushd content
dotnet add . package OpenServiceBroker.Server --version $Version
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}
dotnet build
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}
popd

nuget pack -Version $Version -OutputDirectory ..\artifacts\Release -NoPackageAnalysis
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

popd
