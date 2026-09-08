#!/bin/bash

######################################################
# A script to run integration tests and output results
######################################################

cd "$(dirname "${BASH_SOURCE[0]}")"
cd test
dotnet run -trait "Category=Integration" -reporter custom
