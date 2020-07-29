#!/bin/bash
set -e
cd `dirname $0`

rm -rf ../artifacts/Documentation
mkdir -p ../artifacts/Documentation

VERSION=${1:-1.0-dev} 0install run https://apps.0install.net/devel/doxygen.xml
