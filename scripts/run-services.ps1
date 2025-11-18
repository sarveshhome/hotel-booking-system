# Hotel Booking System - Run Services Script (PowerShell)
Write-Host "🏨 Starting Hotel Booking System Services..." -ForegroundColor Green

# Navigate to project root
$projectRoot = Split-Path -Parent $PSScriptRoot
Set-Location $projectRoot

# Start Hotel Service
Write-Host "🏨 Starting Hotel Service on port 7001..." -ForegroundColor Yellow
$hotelService = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/services/hotel-service/Hotel.Service.csproj", "--urls", "http://localhost:7001" -PassThru -WindowStyle Hidden

# Wait for hotel service to start
Start-Sleep -Seconds 3

# Start API Gateway
Write-Host "🚪 Starting API Gateway on port 5022..." -ForegroundColor Yellow
$apiGateway = Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/api-gateway/API.Gateway.csproj", "--urls", "http://localhost:5022" -PassThru -WindowStyle Hidden

# Wait for gateway to start
Start-Sleep -Seconds 3

Write-Host ""
Write-Host "✅ Services are running:" -ForegroundColor Green
Write-Host "   🏨 Hotel Service: http://localhost:7001" -ForegroundColor Cyan
Write-Host "   🏨 Hotel Service Swagger: http://localhost:7001/swagger" -ForegroundColor Cyan
Write-Host "   🚪 API Gateway: http://localhost:5022" -ForegroundColor Cyan
Write-Host "   🚪 API Gateway Swagger: http://localhost:5022/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop all services" -ForegroundColor Yellow

# Function to cleanup processes
function Stop-Services {
    Write-Host "🛑 Stopping services..." -ForegroundColor Red
    if ($hotelService -and !$hotelService.HasExited) {
        $hotelService.Kill()
    }
    if ($apiGateway -and !$apiGateway.HasExited) {
        $apiGateway.Kill()
    }
}

# Register cleanup on script exit
Register-EngineEvent -SourceIdentifier PowerShell.Exiting -Action { Stop-Services }

# Wait for user input to stop
try {
    while ($true) {
        Start-Sleep -Seconds 1
        if ($hotelService.HasExited -or $apiGateway.HasExited) {
            Write-Host "⚠️ One or more services have stopped unexpectedly" -ForegroundColor Red
            break
        }
    }
}
catch {
    Stop-Services
}
finally {
    Stop-Services
}