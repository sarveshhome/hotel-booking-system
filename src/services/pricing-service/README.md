# Pricing Service

A sophisticated microservice for managing dynamic pricing in the hotel booking system. This service handles complex pricing rules, seasonal adjustments, demand-based pricing, and real-time price calculations.

## 🚀 Features

### Core Pricing Capabilities
- **Dynamic Pricing Rules**: Create and manage complex pricing rules
- **Seasonal Pricing**: Adjust prices based on dates and seasons
- **Demand-Based Pricing**: Modify prices based on hotel occupancy
- **Special Event Pricing**: Handle pricing for holidays and events
- **Weather-Based Pricing**: Adjust prices based on weather conditions
- **Real-Time Calculation**: Calculate prices with multiple rule combinations

### Advanced Features
- **Priority-Based Rules**: Higher priority rules override lower ones
- **Day-of-Week Restrictions**: Apply rules only on specific days
- **Stay Duration Rules**: Minimum/maximum stay requirements
- **Occupancy Rules**: Guest count-based pricing
- **Price Constraints**: Set minimum and maximum price limits

## 🏗️ Architecture

The service follows Clean Architecture principles with a sophisticated pricing engine:

```
src/
├── Domain/Entities/           # Core pricing models
│   ├── PricingRule           # Main pricing rule entity
│   ├── RoomType              # Room type definitions
│   ├── SeasonalPricing       # Seasonal price adjustments
│   └── DynamicPricing        # Real-time pricing data
├── Application/               # Application layer
│   ├── Common/Interfaces/    # Service contracts
│   └── Features/Pricing/     # CQRS commands and queries
├── Infrastructure/            # Infrastructure concerns
│   ├── Persistence/          # Database context
│   └── Services/             # Pricing engine implementation
└── Controllers/               # API endpoints
```

## 📊 Pricing Rule Types

### 1. **Seasonal Rules**
- Date range-based pricing
- Peak/off-peak season adjustments
- Holiday period pricing

### 2. **Demand Rules**
- Occupancy-based pricing
- Dynamic multipliers
- Real-time adjustments

### 3. **Special Event Rules**
- Holiday pricing
- Festival pricing
- Conference pricing

### 4. **Base Rules**
- Standard pricing
- Default multipliers
- Foundation pricing

## 🔌 API Endpoints

### Create Pricing Rule
```http
POST /api/pricing/rules
```
**Request Body:**
```json
{
  "hotelId": "456e7890-e89b-12d3-a456-426614174001",
  "ruleType": "Seasonal",
  "name": "Peak Season Pricing",
  "basePriceModifier": 1.25,
  "startDate": "2024-06-01T00:00:00Z",
  "endDate": "2024-08-31T23:59:59Z"
}
```

### Calculate Price
```http
POST /api/pricing/calculate
```
**Request Body:**
```json
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
```

### Get Pricing Rules
```http
GET /api/pricing/rules/{id}
GET /api/pricing/hotels/{hotelId}/pricing?date=2024-06-15
```

## 💰 Pricing Calculation Logic

### Rule Application Order
1. **Base Price**: Start with room type base price
2. **Priority Sorting**: Apply rules by priority (highest first)
3. **Multipliers**: Apply base price modifiers
4. **Adjustments**: Add/subtract fixed amounts
5. **Constraints**: Enforce minimum/maximum prices
6. **Special Conditions**: Apply event/weather/occupancy rules

### Example Calculation
```
Base Price: $100.00
Seasonal Rule (1.25x): $100.00 × 1.25 = $125.00
Demand Rule (1.15x): $125.00 × 1.15 = $143.75
Special Event (+$20): $143.75 + $20.00 = $163.75
Final Price: $163.75
```

## 🗄️ Database Schema

### PricingRule
- **Core Fields**: HotelId, RoomTypeId, RuleType, Name
- **Pricing**: BasePriceModifier, FixedPriceAdjustment, Min/Max Price
- **Conditions**: Date ranges, stay duration, occupancy, day restrictions
- **Advanced**: Special events, weather, occupancy thresholds

### RoomType
- **Basic Info**: Name, Description, Capacity
- **Pricing**: BasePrice, Currency
- **Status**: IsActive flag

### SeasonalPricing
- **Season Info**: SeasonName, StartDate, EndDate
- **Pricing**: PriceMultiplier, Description

### DynamicPricing
- **Real-time Data**: Date, OccupancyRate, FinalPrice
- **Multipliers**: Demand, Weather, Event multipliers

## 🔄 Integration Points

### Input Sources
- **Hotel Service**: Hotel and room information
- **Booking Service**: Occupancy data and booking patterns
- **External APIs**: Weather data, event calendars

### Output Events
- **PricingRuleCreatedEvent**: When new rules are created
- **PriceCalculatedEvent**: When prices are calculated
- **PricingRuleUpdatedEvent**: When rules are modified

## 🧪 Testing

### Test Scenarios
1. **Basic Pricing**: Simple rule application
2. **Multiple Rules**: Complex rule combinations
3. **Date Restrictions**: Seasonal and date-based rules
4. **Conditional Rules**: Weather, events, occupancy
5. **Priority Handling**: Rule precedence testing

### Test Data
```bash
# Create seasonal rule
curl -X POST https://localhost:7004/api/pricing/rules \
  -H "Content-Type: application/json" \
  -d '{"hotelId":"123","ruleType":"Seasonal","name":"Peak Season","basePriceModifier":1.25}'

# Calculate price
curl -X POST https://localhost:7004/api/pricing/calculate \
  -H "Content-Type: application/json" \
  -d '{"hotelId":"123","checkInDate":"2024-06-15","checkOutDate":"2024-06-18","numberOfGuests":2}'
```

## 🚧 Next Steps

### Immediate Improvements
1. **Real-time Data**: Connect to weather and event APIs
2. **Machine Learning**: Implement ML-based pricing optimization
3. **A/B Testing**: Test different pricing strategies
4. **Analytics**: Track pricing performance and revenue impact

### Future Enhancements
1. **Competitive Pricing**: Monitor competitor prices
2. **Revenue Management**: Advanced yield management
3. **Personalization**: Customer-specific pricing
4. **Predictive Pricing**: Forecast optimal prices

## 🔧 Configuration

### Connection Strings
- **Database**: `PricingServiceDb`
- **Port**: 7004

### Dependencies
- **.NET 9.0**: Latest framework
- **Entity Framework Core**: Data persistence
- **MediatR**: CQRS implementation
- **AutoMapper**: Object mapping

## 📝 Notes

- **Rule Priority**: Higher numbers = higher priority
- **Date Handling**: All dates are in UTC
- **Price Precision**: 4 decimal places for multipliers, 2 for amounts
- **Performance**: Database indexes on frequently queried fields
- **Scalability**: Designed for high-volume pricing calculations

## 🐛 Troubleshooting

### Common Issues
1. **Rule Conflicts**: Check rule priorities and date ranges
2. **Price Anomalies**: Verify rule combinations and constraints
3. **Performance**: Monitor database query performance
4. **Data Consistency**: Ensure hotel and room type data integrity

### Debugging
- Enable detailed logging for rule application
- Check rule applicability filters
- Verify date and condition matching
- Review priority ordering
