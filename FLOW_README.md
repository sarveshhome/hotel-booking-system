# Hotel Booking System - Complete Flow

This document describes the complete flow from **Hotel Search** → **Booking** → **Payment** in the hotel booking system.

## 🏗️ System Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Hotel Service │    │ Pricing Service │    │ Booking Service │    │ Payment Service │
│                 │    │                 │    │                 │    │                 │
│ • Search Hotels │───▶│ • Dynamic Rules │───▶│ • Create Booking│───▶│ • Process Payment│
│ • View Rooms    │    │ • Price Calc    │    │ • Validate      │    │ • Handle Cards  │
│ • Check Pricing │    │ • Seasonal      │    │ • Calculate     │    │ • Publish Events│
└─────────────────┘    └─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🔄 Complete Flow

### 1. Hotel Search (Hotel Service)
- **Endpoint**: `GET /api/hotels`
- **Features**:
  - Search hotels by city, country, star rating
  - View hotel details, rooms, and amenities
  - Check room availability and pricing

### 2. Dynamic Pricing (Pricing Service)
- **Endpoint**: `POST /api/pricing/calculate`
- **Features**:
  - Complex pricing rule engine
  - Seasonal, demand-based, and special event pricing
  - Real-time price calculations with multiple factors
  - Publishes `PriceCalculatedEvent`

### 3. Create Booking (Booking Service)
- **Endpoint**: `POST /api/bookings`
- **Process**:
  - Validates room availability
  - Integrates with pricing service for accurate costs
  - Calculates total amount based on pricing rules
  - Creates booking record
  - Publishes `BookingCreatedEvent`

### 4. Process Payment (Payment Service)
- **Endpoint**: `POST /api/payments/process`
- **Process**:
  - Validates payment information
  - Simulates payment gateway processing
  - Creates payment record
  - Publishes `PaymentProcessedEvent` or `PaymentFailedEvent`

## 🚀 API Endpoints

### Hotel Service (Port 7001)
```http
# Search hotels
GET /api/hotels?city=Miami&minStarRating=4

# Create hotel
POST /api/hotels
{
  "name": "Grand Hotel & Spa",
  "city": "Miami Beach",
  "starRating": 5
}
```

### Booking Service (Port 7002)
```http
# Create booking
POST /api/bookings
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "hotelId": "456e7890-e89b-12d3-a456-426614174001",
  "roomId": "789e0123-e89b-12d3-a456-426614174002",
  "checkInDate": "2024-06-15T14:00:00Z",
  "checkOutDate": "2024-06-18T11:00:00Z",
  "numberOfGuests": 2
}
```

### Pricing Service (Port 7004)
```http
# Calculate price
POST /api/pricing/calculate
{
  "hotelId": "456e7890-e89b-12d3-a456-426614174001",
  "roomTypeId": "789e0123-e89b-12d3-a456-426614174002",
  "checkInDate": "2024-06-15T14:00:00Z",
  "checkOutDate": "2024-06-18T11:00:00Z",
  "numberOfGuests": 2,
  "specialEvent": "Summer",
  "weatherCondition": "Sunny",
  "currentOccupancyRate": 0.85
}

# Create pricing rule
POST /api/pricing/rules
{
  "hotelId": "456e7890-e89b-12d3-a456-426614174001",
  "ruleType": "Seasonal",
  "name": "Peak Season Pricing",
  "basePriceModifier": 1.25
}
```

### Payment Service (Port 7003)
```http
# Process payment
POST /api/payments/process
{
  "bookingId": "123e4567-e89b-12d3-a456-426614174003",
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "amount": 899.97,
  "paymentMethod": "CreditCard",
  "cardNumber": "4111111111111111",
  "cardholderName": "John Doe",
  "cardExpiryMonth": "12",
  "cardExpiryYear": "2025"
}
```

## 📊 Data Flow

### 1. Hotel Search Flow
```
User Request → Hotel Service → Database → Hotel List → User
```

### 2. Pricing Flow
```
User Request → Pricing Service → Pricing Rules → Price Calculation → User
```

### 3. Booking Flow
```
User Request → Booking Service → Hotel Service (validate) → Pricing Service → Database → Event → User
```

### 4. Payment Flow
```
User Request → Payment Service → Payment Gateway → Database → Event → User
```

## 🔌 Event Publishing

### Events Published
- **Hotel Service**: `HotelCreatedEvent`, `HotelUpdatedEvent`, `HotelDeletedEvent`
- **Pricing Service**: `PricingRuleCreatedEvent`, `PriceCalculatedEvent`
- **Booking Service**: `BookingCreatedEvent`
- **Payment Service**: `PaymentProcessedEvent`, `PaymentFailedEvent`, `PaymentRefundedEvent`

### Event Flow
```
Hotel Created → Pricing Rule Created → Price Calculated → Booking Created → Payment Processed
     ↓              ↓                      ↓                ↓              ↓
Hotel Service → Pricing Service → Pricing Service → Booking Service → Payment Service
```

## 🗄️ Database Schema

### Hotel Service
- **Hotels**: Hotel information, location, ratings
- **Rooms**: Room details, pricing, availability
- **HotelAmenities**: Facilities and services

### Booking Service
- **Bookings**: Reservation details, dates, amounts
- **Payments**: Payment records linked to bookings

### Pricing Service
- **PricingRules**: Complex pricing rules and conditions
- **RoomTypes**: Room type definitions and base prices
- **SeasonalPricing**: Seasonal price adjustments
- **DynamicPricing**: Real-time pricing data

### Payment Service
- **Payments**: Payment processing, card details, status

## 🧪 Testing the Flow

### 1. Start All Services
```bash
# Terminal 1 - Hotel Service
cd src/services/hotel-service
dotnet run

# Terminal 2 - Pricing Service
cd src/services/pricing-service
dotnet run

# Terminal 3 - Booking Service  
cd src/services/booking-service
dotnet run

# Terminal 4 - Payment Service
cd src/services/payment-service
dotnet run
```

### 2. Test Complete Flow
```bash
# 1. Create a hotel
curl -X POST https://localhost:7001/api/hotels -H "Content-Type: application/json" -d '{"name":"Test Hotel","city":"Test City"}'

# 2. Create pricing rules
curl -X POST https://localhost:7004/api/pricing/rules -H "Content-Type: application/json" -d '{"hotelId":"456","ruleType":"Seasonal","name":"Peak Season","basePriceModifier":1.25}'

# 3. Calculate price
curl -X POST https://localhost:7004/api/pricing/calculate -H "Content-Type: application/json" -d '{"hotelId":"456","checkInDate":"2024-06-15","checkOutDate":"2024-06-18","numberOfGuests":2}'

# 4. Create a booking
curl -X POST https://localhost:7002/api/bookings -H "Content-Type: application/json" -d '{"userId":"123","hotelId":"456","roomId":"789","checkInDate":"2024-06-15","checkOutDate":"2024-06-18","numberOfGuests":2}'

# 5. Process payment
curl -X POST https://localhost:7003/api/payments/process -H "Content-Type: application/json" -d '{"bookingId":"123","userId":"123","amount":299.99,"paymentMethod":"CreditCard","cardNumber":"4111111111111111"}'
```

## 🔧 Configuration

### Connection Strings
- **Hotel Service**: `HotelServiceDb`
- **Pricing Service**: `PricingServiceDb`
- **Booking Service**: `BookingServiceDb`
- **Payment Service**: `PaymentServiceDb`

### Ports
- **Hotel Service**: 7001
- **Pricing Service**: 7004
- **Booking Service**: 7002
- **Payment Service**: 7003

## 🚧 Next Steps

### Immediate Improvements
1. **Real Hotel Service Integration**: Replace mock service with actual HTTP calls
2. **Event Bus Implementation**: Connect to Kafka/RabbitMQ for real event publishing
3. **Authentication**: Add JWT authentication to all services
4. **Validation**: Implement FluentValidation rules

### Future Enhancements
1. **Pricing Service**: Dynamic pricing based on demand
2. **Notification Service**: Email/SMS confirmations
3. **Review Service**: Hotel and booking reviews
4. **Analytics Service**: Booking patterns and insights

## 📝 Notes

- All services use **.NET 9.0** and **Entity Framework Core**
- **CQRS pattern** with MediatR for command/query separation
- **Clean Architecture** principles throughout
- **Event-driven architecture** for service communication
- **Mock implementations** for testing (can be replaced with real services)

## 🐛 Troubleshooting

### Common Issues
1. **Port Conflicts**: Ensure ports 7001-7003 are available
2. **Database Connection**: Verify SQL Server is running
3. **Build Errors**: Run `dotnet restore` and `dotnet build` from solution root

### Logs
- Check console output for each service
- Event publishing logs show in console (placeholder implementation)
- Database operations logged via Entity Framework
