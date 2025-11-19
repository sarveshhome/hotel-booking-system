#!/bin/bash

echo "🚀 Starting Hotel Service with memory optimizations..."

# Set memory limits
export DOTNET_GCHeapHardLimit=0x40000000  # 1GB
export DOTNET_GCConserveMemory=1
export DOTNET_EnableWriteXorExecute=0

# Set environment
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_URLS="http://localhost:5001"

cd /Users/sarveshkumar/Practice/NetCore/hotel-booking-system/src/services/hotel-service

echo "Memory limit: 1GB"
echo "URL: http://localhost:5001"
echo "Swagger: http://localhost:5001/swagger"

dotnet run --no-launch-profile