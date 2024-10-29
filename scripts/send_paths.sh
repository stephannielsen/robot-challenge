#!/bin/bash

# Check for port
if [ $# -ne 1 ]; then
    echo "Usage: $0 <port>"
    exit 1
fi

port=$1
path_dir="paths"

# Send POST request per path via curl to API
for json_file in "$path_dir"/*.json; do
    echo "Sending $json_file to localhost:$port..."
    curl -X POST -H "Content-Type: application/json" -d @"$json_file" "http://localhost:$port/tibber-developer-test/enter-path"
done

echo "All done."