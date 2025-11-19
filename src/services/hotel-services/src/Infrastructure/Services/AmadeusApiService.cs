using System.Text.Json;

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

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", _settings.ClientId),
            new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await _httpClient.PostAsync("/v1/security/oauth2/token", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<AmadeusTokenResponse>(json);

        _cachedToken = tokenResponse?.AccessToken ?? string.Empty;
        _tokenExpiry = DateTime.UtcNow.AddSeconds((tokenResponse?.ExpiresIn ?? 3600) - 60);

        return _cachedToken;
    }

    public async Task<AmadeusHotelCityResponse> SearchHotelsByCityAsync(string cityCode)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var url = $"/v1/reference-data/locations/hotels/by-city?cityCode={cityCode}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AmadeusHotelCityResponse>(json) ?? new AmadeusHotelCityResponse();
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
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
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