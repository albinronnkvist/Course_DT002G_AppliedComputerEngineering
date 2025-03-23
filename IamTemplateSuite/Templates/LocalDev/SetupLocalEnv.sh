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

echo "🧹 Clearing NuGet cache..."
dotnet nuget locals all --clear

echo "📂 Creating local NuGet feed at: $LOCAL_FEED_PATH"
mkdir -p "$LOCAL_FEED_PATH"

echo "🔁 Removing existing source (if any)..."
dotnet nuget remove source "$NUGET_SOURCE_NAME" &>/dev/null || true

echo "➕ Adding local NuGet source..."
dotnet nuget add source "$LOCAL_FEED_PATH" --name "$NUGET_SOURCE_NAME"

echo "🔍 Current NuGet sources:"
dotnet nuget list source


PROJECTS=(
  "../Templates/Platform/PlatformPulumi/Source/TOrgName.TPlatformPulumiRepositoryName.Core/TOrgName.TPlatformPulumiRepositoryName.Core.csproj"
  "../Templates/Platform/PlatformPulumiAzure/Source/TOrgName.TPlatformPulumiAzureRepositoryName.Core/TOrgName.TPlatformPulumiAzureRepositoryName.Core.csproj"
  "../Templates/IAM/IamEntraId/Source/TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract/TOrgName.TIamEntraIdRepositoryName.Pulumi.Contract.csproj"
  "../Templates/IAM/IamAzure/Source/TOrgName.TIamAzureRepositoryName.Pulumi.Contract/TOrgName.TIamAzureRepositoryName.Pulumi.Contract.csproj"
  "../Templates/Containerization/Containerization/Source/TOrgName.TContainerizationRepositoryName.Pulumi.Contract/TOrgName.TContainerizationRepositoryName.Pulumi.Contract.csproj"
  # Add more projects here if needed
)

echo "🏗️  Building specified projects with DefaultFalse=true..."
for csproj in "${PROJECTS[@]}"; do
  if [[ -f "$csproj" ]]; then
    echo "➡️  Building: $csproj"
    dotnet build "$csproj"
  else
    echo "⚠️  Skipping: $csproj (not found)"
  fi
done

echo "✅ Setup complete: Local NuGet development environment is ready."