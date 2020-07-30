#!/bin/sh
set -e
cd `dirname $0`

# Keep template and library version in-sync
dotnet add MyServiceBroker.csproj package OpenServiceBroker.Server --version ${1:-1.0-dev}
dotnet build MyServiceBroker.csproj

# Generate template NuGet package
dotnet restore .template.csproj
dotnet msbuild .template.csproj -v:Quiet -t:Pack -p:Configuration=Release -p:Version=${1:-1.0-dev}
