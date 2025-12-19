#!/bin/bash

# Cross-Platform Build Script for Photo Organizer
# Run this script from the project root directory

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Define supported runtime identifiers
RIDS=(
    "win-x64"        # Windows 64-bit
    "win-x86"        # Windows 32-bit
    "win-arm64"      # Windows ARM64
    "osx-x64"        # macOS Intel
    "osx-arm64"      # macOS Apple Silicon
    "linux-x64"      # Linux 64-bit
    "linux-arm64"    # Linux ARM64
    "linux-musl-x64" # Linux musl 64-bit (Alpine)
    "linux-musl-arm64" # Linux musl ARM64
)

OUTPUT_DIR="./publish"

# Function to build for a specific RID
build_for_rid() {
    local rid=$1
    echo -e "${GREEN}Building for $rid...${NC}"

    local output_path="$OUTPUT_DIR/$rid"

    # Create output directory if it doesn't exist
    mkdir -p "$output_path"

    # Build and publish
    if dotnet publish -c Release -r "$rid" --self-contained --output "$output_path"; then
        echo -e "${GREEN}Successfully built for $rid${NC}"

        # Determine executable name
        local exe_name="photo-organizer"
        if [[ $rid == win-* ]]; then
            exe_name="photo-organizer.exe"
        fi

        local exe_path="$output_path/$exe_name"
        if [[ -f "$exe_path" ]]; then
            local size_mb=$(du -m "$exe_path" | cut -f1)
            echo -e "${CYAN}Output: $exe_path (${size_mb} MB)${NC}"
        fi
    else
        echo -e "${RED}Failed to build for $rid${NC}"
        exit 1
    fi
}

# Function to show usage
show_usage() {
    echo -e "${YELLOW}Usage:${NC}"
    echo -e "  ${BLUE}./build.sh all${NC}                    # Build for all platforms"
    echo -e "  ${BLUE}./build.sh win-x64${NC}               # Build for specific platform"
    echo -e "  ${BLUE}./build.sh linux-x64 ./my-output${NC}  # Custom output directory"
    echo ""
    echo -e "${YELLOW}Supported platforms:${NC}"
    for rid in "${RIDS[@]}"; do
        echo -e "  ${BLUE}- $rid${NC}"
    done
}

# Main logic
if [[ $# -eq 0 ]]; then
    show_usage
    exit 1
fi

TARGET_RID=$1
OUTPUT_DIR=${2:-"./publish"}

if [[ "$TARGET_RID" == "all" ]]; then
    echo -e "${YELLOW}Building for all supported platforms...${NC}"
    for rid in "${RIDS[@]}"; do
        build_for_rid "$rid"
    done
    echo -e "${GREEN}All builds completed!${NC}"
else
    # Check if the provided RID is supported
    if [[ " ${RIDS[*]} " =~ " $TARGET_RID " ]]; then
        build_for_rid "$TARGET_RID"
    else
        echo -e "${RED}Unsupported RID: $TARGET_RID${NC}"
        echo -e "${YELLOW}Supported RIDs: ${RIDS[*]}${NC}"
        exit 1
    fi
fi