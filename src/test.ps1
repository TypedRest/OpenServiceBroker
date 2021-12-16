$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

# Needs multiple .NET SDKs
dotnet test --no-build --logger junit --configuration Release UnitTests\UnitTests.csproj

popd
