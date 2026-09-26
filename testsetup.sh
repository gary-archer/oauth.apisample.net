#!/bin/bash

#############################################################
# A script to build and run the API with a test configuration
#############################################################

cd "$(dirname "${BASH_SOURCE[0]}")"

#
# Use a configuration that points to a mock authorization server
#
cp deployment/environments/test/api.config.json ./api.config.json

#
# Create development SSL certificates if required
#
./certs/create.sh
if [ $? -ne 0 ]; then
  exit 1
fi

#
# Run the API
#
./run_api.sh
