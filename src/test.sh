#!/bin/sh
set -e
cd `dirname $0`

# Needs multiple .NET SDKs
dotnet test --no-build --logger junit --configuration Release UnitTests/UnitTests.csproj
