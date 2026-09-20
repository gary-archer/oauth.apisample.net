#!/bin/bash

######################################################
# A script to run integration tests and output results
######################################################

cd "$(dirname "${BASH_SOURCE[0]}")"

#
# Build the test code
#
dotnet build

#
# On Linux ensure that the API has permissions to listen on a port below 1024
#
if [ "$(uname -s)" == 'Linux' ]; then
  sudo setcap 'cap_net_bind_service=+ep' ./test/bin/Debug/net10.0/test
fi

#
# Run the tests
#
cd test
dotnet run -trait "Category=Integration" -reporter custom
