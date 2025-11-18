#!/bin/bash

# Hotel Booking System - Run Services Script
echo "🏨 Starting Hotel Booking System Services..."

# Function to kill processes on script exit
cleanup() {
    echo "🛑 Stopping services..."
    kill $HOTEL_PID $GATEWAY_PID 2>/dev/null
    exit
}

# Set up trap to cleanup on script exit
trap cleanup SIGINT SIGTERM EXIT

# Navigate to project root
cd "$(dirname "$0")/.."

# Start Hotel Service
echo "🏨 Starting Hotel Service on port 7001..."
cd src/services/hotel-service
dotnet run --project Hotel.Service.csproj --urls "http://localhost:7001" &
HOTEL_PID=$!

# Wait a moment for hotel service to start
sleep 3

# Start API Gateway
echo "🚪 Starting API Gateway on port 5022..."
cd ../../../src/api-gateway
dotnet run --project API.Gateway.csproj --urls "http://localhost:5022" &
GATEWAY_PID=$!

# Wait a moment for gateway to start
sleep 3

echo ""
echo "✅ Services are running:"
echo "   🏨 Hotel Service: http://localhost:7001"
echo "   🏨 Hotel Service Swagger: http://localhost:7001/swagger"
echo "   🚪 API Gateway: http://localhost:5022"
echo "   🚪 API Gateway Swagger: http://localhost:5022/swagger"
echo ""
echo "Press Ctrl+C to stop all services"

# Wait for background processes
wait