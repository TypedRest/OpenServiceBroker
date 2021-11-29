#!/usr/bin/env bash
set -e
cd `dirname $0`

# Find dotnet
if command -v dotnet > /dev/null 2> /dev/null; then
    dotnet="dotnet"
else
    dotnet="../0install.sh run --version 6.0.. https://apps.0install.net/dotnet/sdk.xml"
fi

# Keep template and library version in-sync
dotnet add MyServiceBroker.csproj package OpenServiceBroker.Server --version ${1:-1.0-dev}
dotnet build MyServiceBroker.csproj

# Generate template NuGet package
dotnet restore .template.csproj
dotnet msbuild .template.csproj -v:Quiet -t:Pack -p:Configuration=Release -p:Version=${1:-1.0-dev}
