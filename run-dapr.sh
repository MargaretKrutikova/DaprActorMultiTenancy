#!/bin/bash

PLACEMENT_PORT=50005

dapr_run () {
   dapr run --resources-path ./dapr/components --placement-host-address 127.0.0.1:${PLACEMENT_PORT} \
      --app-id $1 --app-port $2 --dapr-http-port $3 --dapr-grpc-port $4 --log-level debug --enable-api-logging &
   sleep 1
}

dapr_run test-app-1 5011 3501 54600
dapr_run test-app-2 5013 3502 54601

wait
