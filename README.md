create microservices architecture for a hotel booking system with .net core and folder structure and give me all command in visual studio code

 Token-based Authentication
⚡ Clean Architecture
🧪 Integration & Unit Testing
🎯 React Frontend with Vite


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


