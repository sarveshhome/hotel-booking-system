# Amadeus API Token Flow Implementation

## Overview
The Amadeus API service implements OAuth 2.0 client credentials flow for authentication. Here's how it works:

## Token Acquisition Flow

### 1. Configuration
```json
{
  "Amadeus": {
    "BaseUrl": "https://test.api.amadeus.com",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

### 2. Token Request
```csharp
public async Task<string> GetAccessTokenAsync()
{
    // Check if cached token is still valid
    if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiry)
        return _cachedToken;

    // Clear authorization header for token request
    _httpClient.DefaultRequestHeaders.Authorization = null;
    
    // Prepare form data
    var content = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("client_id", _settings.ClientId),
        new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
        new KeyValuePair<string, string>("grant_type", "client_credentials")
    });

    // Make token request to absolute URL
    var tokenUrl = $"{_settings.BaseUrl}/v1/security/oauth2/token";
    var response = await _httpClient.PostAsync(tokenUrl, content);
    
    // Parse response and cache token
    var json = await response.Content.ReadAsStringAsync();
    var tokenResponse = JsonSerializer.Deserialize<AmadeusTokenResponse>(json, options);
    
    _cachedToken = tokenResponse?.AccessToken ?? string.Empty;
    _tokenExpiry = DateTime.UtcNow.AddSeconds((tokenResponse?.ExpiresIn ?? 3600) - 60);
    
    return _cachedToken;
}
```

### 3. Using Token for API Calls
```csharp
public async Task<AmadeusHotelCityResponse> SearchHotelsByCityAsync(string cityCode)
{
    // Get valid token (from cache or new request)
    var token = await GetAccessTokenAsync();
    
    // Set authorization header for API request
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    // Make API request with absolute URL
    var url = $"{_settings.BaseUrl}/v1/reference-data/locations/hotels/by-city?cityCode={cityCode}";
    var response = await _httpClient.GetAsync(url);
    
    // Process response...
}
```

## Key Features

### 1. Token Caching
- Tokens are cached in memory with expiration tracking
- New tokens are only requested when the current one expires
- 60-second buffer before expiration to avoid edge cases

### 2. Error Handling
- Graceful fallback to mock data when API is unavailable
- Detailed error logging for debugging
- Proper HTTP status code handling

### 3. URL Management
- Uses absolute URLs for all requests
- Separates token endpoint from API endpoints
- Configurable base URL for different environments

### 4. HttpClient Configuration
```csharp
// In Program.cs
builder.Services.AddHttpClient<IAmadeusApiService, AmadeusApiService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "HotelBookingSystem/1.0");
});
```

## Testing Endpoints

### Test Token Acquisition
```http
GET http://localhost:5068/api/amadeus/token/test
```

### Test Hotel Search (uses token internally)
```http
GET http://localhost:5068/api/amadeus/hotels/city/NYC
```

## Token Response Format
```json
{
  "access_token": "your-access-token-here",
  "expires_in": 1799,
  "token_type": "Bearer"
}
```

## Best Practices Implemented

1. **Singleton Pattern**: AmadeusSettings registered as singleton
2. **Dependency Injection**: HttpClient injected via DI container
3. **Configuration Binding**: Settings bound from appsettings.json
4. **Error Resilience**: Fallback to mock data when API fails
5. **Token Reuse**: Cached tokens to minimize API calls
6. **Proper Headers**: Authorization header management
7. **Absolute URLs**: No dependency on HttpClient BaseAddress

## Environment Setup

For development, you can use the provided test credentials or replace them with your own Amadeus API credentials in `appsettings.json`.

The service will automatically fall back to mock data if the API is unavailable or credentials are invalid, ensuring the application continues to function during development.