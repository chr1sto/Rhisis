#!/usr/bin/env bash

#
# Author: Filipe GOMES PEIXOTO <gomespeixoto.filipe@gmail.com>
# Title: Rhisis release build script
# Description :
# This script builds the Rhisis solution in release mode and
# creates the release folder named "dist".
#

# Global variables
DIST_DIRECTORY=dist
DIST_DIRECTORY_BINARIES=$DIST_DIRECTORY/bin

#
# Create the dist folder and subfolders
#
function create_rhisis_file {
    FILE_PATH=$DIST_DIRECTORY/$1

    echo "#!/bin/bash" > FILE_PATH
    echo "cd \${0%/*}" >> FILE_PATH
    echo "dotnet $2" >> FILE_PATH
    chmod +x FILE_PATH
}

# Delete dist folder if exists
if [ -d "$DIST_DIRECTORY" ]; then
  rm -rf $DIST_DIRECTORY
fi

# Create dist folder
mkdir $DIST_DIRECTORY
mkdir $DIST_DIRECTORY_BINARIES
mkdir $DIST_DIRECTORY_BINARIES/login
mkdir $DIST_DIRECTORY_BINARIES/cluster
mkdir $DIST_DIRECTORY_BINARIES/world
mkdir $DIST_DIRECTORY_BINARIES/tools
mkdir $DIST_DIRECTORY_BINARIES/tools/cli

# Build Rhisis solution release mode
./build/build.sh

# Copy binaries to dist/bin folders
cp src/Rhisis.Login/bin/Release/netcoreapp2.0/* $DIST_DIRECTORY_BINARIES/login
cp src/Rhisis.Cluster/bin/Release/netcoreapp2.0/* $DIST_DIRECTORY_BINARIES/cluster
cp src/Rhisis.World/bin/Release/netcoreapp2.0/* $DIST_DIRECTORY_BINARIES/world
cp src/tools/Rhisis.CLI/bin/Release/netcoreapp2.0/* $DIST_DIRECTORY_BINARIES/tools/cli

# Create executables
create_rhisis_file "rhisis-login" "bin/login/Rhisis.Login.dll"
create_rhisis_file "rhisis-cluster" "bin/cluster/Rhisis.Cluster.dll"
create_rhisis_file "rhisis-world" "bin/world/Rhisis.World.dll"
create_rhisis_file "rhisis-cli" "bin/tools/cli/Rhisis.CLI.dll"
