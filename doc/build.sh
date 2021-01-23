#!/usr/bin/env bash
set -e
cd `dirname $0`

echo "Downloading references to other documentation..."
curl -sS -o typedrest-dotnet.tag https://dotnet.typedrest.net/typedrest-dotnet.tag

rm -rf ../artifacts/Documentation
mkdir -p ../artifacts/Documentation

VERSION=${1:-1.0-dev} ../0install.sh run https://apps.0install.net/devel/doxygen.xml
