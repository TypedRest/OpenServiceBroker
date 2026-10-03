$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

# Needs multiple .NET SDKs
dotnet test --no-build --configuration Release

popd
