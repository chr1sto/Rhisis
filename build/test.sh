#!/usr/bin/env bash

#
# Author: Filipe GOMES PEIXOTO <gomespeixoto.filipe@gmail.com>
# Title: Rhisis build script
# Description :
# This script tests the Rhisis solution.
#

dotnet restore
dotnet test test/Rhisis.Core.Test/