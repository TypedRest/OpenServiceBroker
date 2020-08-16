#!/bin/sh
set -e
cd `dirname $0`

src/build.sh ${1:-1.0-dev}
src/test.sh
template/build.sh ${1:-1.0-dev}
#doc/build.sh ${1:-1.0-dev}
