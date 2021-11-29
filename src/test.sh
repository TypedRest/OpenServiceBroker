#!/bin/sh
set -e
cd `dirname $0`

# Find dotnet
if command -v dotnet > /dev/null 2> /dev/null; then
    dotnet="dotnet"
else
    dotnet="../0install.sh run --version 6.0..!6.1 https://apps.0install.net/dotnet/core-sdk.xml"
fi

# Unit tests
$dotnet test --no-build --logger junit --configuration Release UnitTests/UnitTests.csproj
