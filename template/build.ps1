Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

# Keep template and library version in-sync
dotnet add MyServiceBroker.csproj package OpenServiceBroker.Server --version $Version
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}
dotnet build MyServiceBroker.csproj
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

# Generate template NuGet package
dotnet restore .template.csproj
dotnet msbuild .template.csproj -v:Quiet -t:Pack -p:Configuration=Release -p:Version=$Version
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

popd
