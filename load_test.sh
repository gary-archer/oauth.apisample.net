#!/bin/bash

#####################################
# A script to run the basic load test
#####################################

cd "$(dirname "${BASH_SOURCE[0]}")"
cd test
dotnet run -trait "Category=Load" -reporter custom
