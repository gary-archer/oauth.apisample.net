#!/bin/bash

#####################################
# A script to run the basic load test
#####################################

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
# Run the load test
#
cd test
dotnet run -trait "Category=Load" -reporter custom
