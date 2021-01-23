#!/usr/bin/env bash
set -e
cd `dirname $0`

dotnet msbuild -v:Quiet -t:Restore -t:Build -p:Configuration=Release -p:Version=${1:-1.0-dev}
