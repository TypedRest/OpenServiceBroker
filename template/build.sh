#!/bin/bash
set -e
cd `dirname $0`

pushd content
dotnet add . package OpenServiceBroker.Server --version ${1:-1.0-dev}
dotnet build
popd

nuget pack -Version ${1:-1.0-dev} -OutputDirectory ../artifacts/Release -NoPackageAnalysis
