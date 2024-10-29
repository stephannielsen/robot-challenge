#!/bin/bash

OUTPUT_DIR="paths"
NUM_PATHS=${1:-10}

mkdir -p "$OUTPUT_DIR"

random_direction() {
    directions=("north" "east" "south" "west")
    echo "${directions[RANDOM % ${#directions[@]}]}"
}

# steps are between 1 and 100_000
random_steps() {
    echo $((RANDOM % 100000 + 1))
}

# coordinate range is -100_000 to 100_000
random_coordinate() {
    echo $((RANDOM % 200001 - 100000))
}

# Generate paths with random number of commands with random steps in it starting from random location
# Path is not allowed to go out of bounds of the grid
for ((i=1; i<=NUM_PATHS; i++)); do
    start_x=$(random_coordinate)
    start_y=$(random_coordinate)
    commands=()
    
    num_commands=$((RANDOM % 10000 + 1))
    
    current_x=$start_x
    current_y=$start_y

    for ((j=0; j<num_commands; j++)); do
        direction=$(random_direction)
        steps=$(random_steps)

        new_x=$current_x
        new_y=$current_y

        case $direction in
            "north")
                new_y=$((current_y + steps))
                ;;
            "east")
                new_x=$((current_x + steps))
                ;;
            "south")
                new_y=$((current_y - steps))
                ;;
            "west")
                new_x=$((current_x - steps))
                ;;
        esac
        
        if [ $new_x -ge -100000 ] && [ $new_x -le 100000 ] && [ $new_y -ge -100000 ] && [ $new_y -le 100000 ]; then
            commands+=("{\"direction\":\"$direction\",\"steps\":$steps}")
            current_x=$new_x
            current_y=$new_y
        fi
    done
    
    # Print the path as JSON and save it
    json="{\"start\":{\"x\":$start_x,\"y\":$start_y},\"commands\":["$(IFS=, ; echo "${commands[*]}")"]}"
    
    echo "$json" > "$OUTPUT_DIR/path_$i.json"
done
