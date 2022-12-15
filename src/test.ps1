$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

# Needs multiple .NET SDKs
dotnet test --no-build --logger trx --configuration Release UnitTests\UnitTests.csproj

popd
