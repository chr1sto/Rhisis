#!/usr/bin/env bash

#
# Author: Filipe GOMES PEIXOTO <gomespeixoto.filipe@gmail.com>
# Title: Rhisis deployment script
# Description :
# This script builds the Rhisis solution in release mode and
# deploy it automatically to the pre-production server.
#

# Global variables
DIST_DIRECTORY=dist

# Build the distribution folder
./build-dist.sh

# Package everything
tar -cvzf package.tgz dist/

# Send package via SSH
# TODO