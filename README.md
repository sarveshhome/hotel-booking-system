create microservices architecture for a hotel booking system with .net core and folder structure and give me all command in visual studio code

 Token-based Authentication
⚡ Clean Architecture
🧪 Integration & Unit Testing
🎯 React Frontend with Vite

```
hotel-booking-system/
├── src/
│   ├── api-gateway/                     # API Gateway Service
│   │   ├── ApiGateway.csproj
│   │   ├── Program.cs
│   │   ├── Dockerfile
│   │   └── ocelot.json
│   │
│   ├── Kafka/                          # Kafka Infrastructure
│   │   ├── Publishers/
│   │   │   ├── Kafka.Publishers.csproj
│   │   │   ├── Configuration/
│   │   │   │   └── KafkaSettings.cs
│   │   │   ├── Interfaces/
│   │   │   │   └── IKafkaProducer.cs
│   │   │   ├── Extensions/
│   │   │   │   └── ServiceCollectionExtensions.cs
│   │   │   └── KafkaProducer.cs
│   │   │
│   │   └── ConsumerHost/
│   │       ├── Kafka.ConsumerHost.csproj
│   │       ├── Configuration/
│   │       │   └── KafkaConsumerSettings.cs
│   │       ├── Interfaces/
│   │       │   └── IMessageHandler.cs
│   │       ├── Extensions/
│   │       │   └── ServiceCollectionExtensions.cs
│   │       └── KafkaConsumerService.cs
│   │
│   ├── services/                       # Microservices
│   │   ├── user-service/              # User Management Service
│   │   │   ├── UserService.csproj
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Data/
│   │   │   └── Dockerfile
│   │   │
│   │   ├── hotel-service/             # Hotel Management Service
│   │   │   ├── HotelService.csproj
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Data/
│   │   │   └── Dockerfile
│   │   │
│   │   ├── room-service/              # Room Management Service
│   │   │   ├── RoomService.csproj
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Data/
│   │   │   └── Dockerfile
│   │   │
│   │   ├── booking-service/           # Booking Management Service
│   │   │   ├── BookingService.csproj
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Data/
│   │   │   └── Dockerfile
│   │   │
│   │   ├── payment-service/           # Payment Processing Service
│   │   │   ├── PaymentService.csproj
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Data/
│   │   │   └── Dockerfile
│   │   │
│   │   ├── notification-service/      # Notification Service
│   │   │   ├── NotificationService.csproj
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Handlers/
│   │   │   └── Dockerfile
│   │   │
│   │   └── rate-service/             # Rate Management Service
│   │       ├── RateService.csproj
│   │       ├── Controllers/
│   │       ├── Models/
│   │       ├── Services/
│   │       ├── Data/
│   │       └── Dockerfile
│   │
│   ├── shared/                       # Shared Libraries
│   │   ├── Shared.csproj
│   │   ├── Constants/
│   │   ├── Models/
│   │   ├── Interfaces/
│   │   └── Extensions/
│   │
│   └── frontend/                     # React Frontend
│       ├── package.json
│       ├── vite.config.js
│       ├── src/
│       ├── public/
│       └── Dockerfile
│
├── tests/                           # Test Projects
│   ├── unit-tests/
│   │   └── Services.Tests/
│   └── integration-tests/
│       └── Api.IntegrationTests/
│
├── infra/                          # Infrastructure
│   ├── kafka/
│   │   └── config/
│   ├── database/
│   │   └── migrations/
│   └── kubernetes/
│       ├── deployments/
│       └── services/
│
├── scripts/                        # Scripts
│   ├── build.sh
│   ├── deploy.sh
│   └── setup-local.sh
│
├── docker-compose.yml              # Docker Compose file
├── docker-compose.override.yml     # Docker Compose Override
├── .gitignore
├── README.md
└── HotelBookingSystem.sln         # Solution File
```

```
hotel-booking-system/
├── 📁 src/
│   ├── 📁 api-gateway/                 # Ocelot API Gateway
│   ├── 📁 services/
│   │   ├── 📁 user-service/            # User management & auth
│   │   ├── 📁 hotel-service/           # Hotel management
│   │   ├── 📁 room-service/            # Room inventory
│   │   ├── 📁 rate-service/            # Pricing & rate cards
│   │   ├── 📁 pricing-service/         # Bill calculation
│   │   ├── 📁 payment-service/         # Payment processing
│   │   └── 📁 booking-service/         # Booking orchestration
│   ├── 📁 shared/                      # Shared contracts & utilities
│   └── 📁 frontend/                    # React/Vite application
├── 📁 infra/
│   ├── 📁 kafka/                       # Kafka setup
│   ├── 📁 database/                    # SQL scripts
│   └── docker-compose.yml              # Docker composition
└── 📁 scripts/                         # Setup scripts
```


# Hotel Booking System

A modern microservices-based hotel booking system built with .NET Core, featuring token-based authentication, clean architecture, and a React frontend.

## 🏗️ Architecture

This project implements a microservices architecture with the following components:

### Microservices
- **API Gateway**: Ocelot gateway for routing and request aggregation
- **User Service**: Handles user management and authentication
- **Hotel Service**: Manages hotel information and details
- **Room Service**: Handles room inventory and availability
- **Rate Service**: Manages pricing and rate cards
- **Pricing Service**: Handles bill calculation
- **Payment Service**: Processes payments
- **Booking Service**: Orchestrates the booking process
- **Frontend**: React application with Vite

## 🚀 Tech Stack

- **.NET Core**: Backend microservices
- **React + Vite**: Frontend development
- **Token-based Authentication**: Secure API access
- **Clean Architecture**: For maintainable and testable code
- **Docker**: Containerization
- **Apache Kafka**: Event messaging
- **SQL Database**: Data persistence

## 🛠️ Prerequisites

- .NET 6.0 SDK or later
- Node.js and npm
- Docker Desktop
- Visual Studio Code or preferred IDE

## ⚙️ Setup and Installation

1. Clone the repository
```bash
git clone [repository-url]
cd hotel-booking-system


