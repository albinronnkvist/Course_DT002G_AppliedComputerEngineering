#!/bin/bash

set -e

# 📍 Get the current directory and define the local NuGet feed path
SCRIPT_DIR="$(pwd)"
if [[ $(basename "$SCRIPT_DIR") != "LocalDev" ]]; then
  echo "❌ Please run this script from within the LocalDev folder."
  exit 1
fi

LOCAL_FEED_PATH="$SCRIPT_DIR/LocalNuGetFeed"
NUGET_SOURCE_NAME="LocalNuGetFeed"

echo "🗑️ Removing NuGet source: $NUGET_SOURCE_NAME"
dotnet nuget remove source "$NUGET_SOURCE_NAME" &>/dev/null || true

echo "🧹 Clearing NuGet cache..."
dotnet nuget locals all --clear

echo "🗑️  Removing local NuGet feed at: $LOCAL_FEED_PATH"
rm -rf "$LOCAL_FEED_PATH"

echo "🗑️ Local NuGet development environment destroyed."