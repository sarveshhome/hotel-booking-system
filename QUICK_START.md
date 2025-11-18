# Quick Start Guide

## 🚀 Running the Services

### Option 1: Using Scripts (Recommended)

**macOS/Linux:**
```bash
./scripts/run-services.sh
```

**Windows:**
```powershell
.\scripts\run-services.ps1
```

### Option 2: Manual Commands

**Terminal 1 - Hotel Service:**
```bash
cd src/services/hotel-service
dotnet run --urls "http://localhost:7001"
```

**Terminal 2 - API Gateway:**
```bash
cd src/api-gateway  
dotnet run --urls "http://localhost:5022"
```

### Option 3: VS Code Tasks

1. Open Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`)
2. Type "Tasks: Run Task"
3. Select "Run All Services"

## 📋 Service URLs

| Service | URL | Swagger |
|---------|-----|---------|
| Hotel Service | http://localhost:7001 | http://localhost:7001/swagger |
| API Gateway | http://localhost:5022 | http://localhost:5022/swagger |

## 🔧 API Gateway Routes

- **Hotels**: `GET/POST/PUT/DELETE /api/hotels/*` → Hotel Service (localhost:7001)
- **Amadeus**: `GET/POST/PUT/DELETE /api/amadeus/*` → Hotel Service (localhost:7001)
- **Users**: `GET/POST/PUT/DELETE /api/users/*` → User Service (when running)

## 📖 Swagger Documentation

The API Gateway Swagger UI aggregates documentation from:
- ✅ API Gateway endpoints
- ✅ Hotel Service endpoints (via proxy)

Access the unified Swagger UI at: **http://localhost:5022/swagger**

## 🛑 Stopping Services

Press `Ctrl+C` in the terminal running the script to stop all services.