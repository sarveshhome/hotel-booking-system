# Hotel Service

A microservice for managing hotels in the hotel booking system.

## Features

- **Create Hotels**: Add new hotels with comprehensive details
- **List Hotels**: Retrieve hotels with filtering options
- **Hotel Management**: Manage hotel information, rooms, and amenities
- **Event Publishing**: Publishes events when hotels are created/updated

## Architecture

The service follows Clean Architecture principles:

```
src/
├── Domain/Entities/           # Domain models (Hotel, Room, HotelAmenity)
├── Application/               # Application layer with CQRS
│   ├── Common/Interfaces/     # Application interfaces
│   └── Features/Hotels/       # Hotel-specific features
│       ├── Commands/          # Create, Update, Delete operations
│       └── Queries/           # Read operations
├── Infrastructure/            # Infrastructure concerns
│   └── Persistence/          # Database context and configurations
└── Controllers/               # API endpoints
```

## API Endpoints

### POST /api/hotels
Create a new hotel.

**Request Body:**
```json
{
  "name": "Hotel Name",
  "description": "Hotel description",
  "address": "Hotel address",
  "city": "City",
  "state": "State",
  "country": "Country",
  "postalCode": "Postal code",
  "phoneNumber": "Phone number",
  "email": "Email address",
  "website": "Website URL",
  "starRating": 5,
  "imageUrl": "Image URL",
  "latitude": 25.7617,
  "longitude": -80.1918
}
```

### GET /api/hotels
Get all hotels with optional filtering.

**Query Parameters:**
- `city`: Filter by city
- `country`: Filter by country
- `minStarRating`: Minimum star rating
- `isActive`: Filter by active status

### GET /api/hotels/{id}
Get a specific hotel by ID (placeholder implementation).

## Database

The service uses Entity Framework Core with SQL Server. The database includes:

- **Hotels**: Main hotel information
- **Rooms**: Hotel rooms with pricing and availability
- **HotelAmenities**: Hotel facilities and services

## Events

The service publishes the following events:

- `HotelCreatedEvent`: When a new hotel is created
- `HotelUpdatedEvent`: When a hotel is updated
- `HotelDeletedEvent`: When a hotel is deleted

## Running the Service

1. Ensure SQL Server is running
2. Update the connection string in `appsettings.json`
3. Run the service: `dotnet run`
4. The service will be available at `https://localhost:7001`

## Dependencies

- .NET 9.0
- Entity Framework Core
- MediatR (CQRS)
- AutoMapper
- FluentValidation
- Confluent.Kafka (for event publishing)

## Testing

Use the `Hotel.Service.http` file to test the API endpoints with REST Client or similar tools.
