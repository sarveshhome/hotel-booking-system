using System.Text.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Hotel.Service.Infrastructure.Services;

public interface IAmadeusApiService
{
    Task<string> GetAccessTokenAsync();
    Task<AmadeusHotelCityResponse> SearchHotelsByCityAsync(string cityCode);
}

public class AmadeusApiService : IAmadeusApiService
{
    private readonly HttpClient _httpClient;
    private readonly AmadeusSettings _settings;
    private string? _cachedToken;
    private DateTime _tokenExpiry;

    public AmadeusApiService(HttpClient httpClient, AmadeusSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiry)
            return _cachedToken;

        try
        {
            // Clear any existing authorization header for token request
            _httpClient.DefaultRequestHeaders.Authorization = null;
            
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            // Use absolute URL for token endpoint
            var tokenUrl = $"{_settings.BaseUrl}/v1/security/oauth2/token";
            var response = await _httpClient.PostAsync(tokenUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Token request failed: {response.StatusCode} - {errorContent}");
                return "mock-token-for-development";
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
            
            var tokenResponse = JsonSerializer.Deserialize<AmadeusTokenResponse>(json, options);

            _cachedToken = tokenResponse?.AccessToken ?? string.Empty;
            _tokenExpiry = DateTime.UtcNow.AddSeconds((tokenResponse?.ExpiresIn ?? 3600) - 60);

            Console.WriteLine($"Token obtained successfully, expires at: {_tokenExpiry}");
            return _cachedToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting access token: {ex.Message}");
            return "mock-token-for-development";
        }
    }

    public async Task<AmadeusHotelCityResponse> SearchHotelsByCityAsync(string cityCode)
    {
        try
        {
            var token = await GetAccessTokenAsync();
            
            // Set authorization header for API requests
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Use absolute URL for API endpoint
            var url = $"{_settings.BaseUrl}/v1/reference-data/locations/hotels/by-city?cityCode={cityCode}";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Hotel search failed: {response.StatusCode} - {errorContent}");
                return GetMockHotelData(cityCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            
            return JsonSerializer.Deserialize<AmadeusHotelCityResponse>(json, options) ?? new AmadeusHotelCityResponse();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching hotels: {ex.Message}");
            return GetMockHotelData(cityCode);
        }
    }

    private AmadeusHotelCityResponse GetMockHotelData(string cityCode)
    {
        return new AmadeusHotelCityResponse
        {
            Data = new List<HotelLocation>
            {
                new HotelLocation
                {
                    Type = "location",
                    SubType = "hotel",
                    Name = $"Mock Hotel in {cityCode}",
                    DetailedName = $"Mock Luxury Hotel in {cityCode}",
                    Id = Guid.NewGuid().ToString(),
                    IataCode = cityCode,
                    GeoCode = new GeoCode { Latitude = 40.7128, Longitude = -74.0060 },
                    Address = new Address
                    {
                        CityName = cityCode,
                        CityCode = cityCode,
                        CountryName = "Mock Country",
                        CountryCode = "MC"
                    }
                },
                new HotelLocation
                {
                    Type = "location",
                    SubType = "hotel",
                    Name = $"Mock Business Hotel in {cityCode}",
                    DetailedName = $"Mock Business Hotel in {cityCode}",
                    Id = Guid.NewGuid().ToString(),
                    IataCode = cityCode,
                    GeoCode = new GeoCode { Latitude = 40.7589, Longitude = -73.9851 },
                    Address = new Address
                    {
                        CityName = cityCode,
                        CityCode = cityCode,
                        CountryName = "Mock Country",
                        CountryCode = "MC"
                    }
                }
            }
        };
    }
}

public class AmadeusSettings
{
    public string BaseUrl { get; set; } = "https://test.api.amadeus.com";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

public class AmadeusTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;
}

public class AmadeusHotelCityResponse
{
    public List<HotelLocation> Data { get; set; } = new();
}

public class HotelLocation
{
    public string Type { get; set; } = string.Empty;
    public string SubType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DetailedName { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Self { get; set; } = string.Empty;
    public string TimeZoneOffset { get; set; } = string.Empty;
    public string IataCode { get; set; } = string.Empty;
    public GeoCode GeoCode { get; set; } = new();
    public Address Address { get; set; } = new();
}

public class GeoCode
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Address
{
    public string CityName { get; set; } = string.Empty;
    public string CityCode { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string StateCode { get; set; } = string.Empty;
}