#!/usr/bin/env bash
set -e
cd `dirname $0`

# Find dotnet
if command -v dotnet > /dev/null 2> /dev/null; then
    dotnet="dotnet"
else
    dotnet="../0install.sh run --version 11..!11.1 https://apps.0install.net/dotnet/sdk.xml"
fi

echo "Build binaries"
$dotnet msbuild -v:Quiet -restore -t:Build -p:Configuration=Release -p:Version=${1:-1.0.0-pre} ${CI+-p:ContinuousIntegrationBuild=True}
