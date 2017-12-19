#!/usr/bin/env bash

#
# Author: Filipe GOMES PEIXOTO <gomespeixoto.filipe@gmail.com>
# Title: Rhisis build script
# Description :
# This script builds and tests the Rhisis solution.
#

dotnet restore
dotnet build src/Rhisis.Core/ -f netstandard2.0 --configuration Release
dotnet build src/Rhisis.Database/ -f netstandard2.0 --configuration Release
dotnet build src/tools/Rhisis.CLI/ -f netcoreapp2.0 --configuration Release
dotnet build src/Rhisis.Login/ -f netcoreapp2.0 --configuration Release
dotnet build src/Rhisis.Cluster/ -f netcoreapp2.0 --configuration Release
dotnet build src/Rhisis.World/ -f netcoreapp2.0 --configuration Release