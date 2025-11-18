using System.Text.Json;

namespace Pricing.Service.Infrastructure.Services;

public interface IAmadeusApiService
{
    Task<string> GetAccessTokenAsync();
    Task<AmadeusHotelResponse> SearchHotelsAsync(string cityCode, string checkIn, string checkOut);
    Task<AmadeusFlightResponse> SearchFlightsAsync(string origin, string destination, string departureDate, string returnDate, int adults, int max = 5);
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

        _cachedToken = tokenResponse.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60);

        return _cachedToken;
    }

    public async Task<AmadeusHotelResponse> SearchHotelsAsync(string cityCode, string checkIn, string checkOut)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var url = $"/v3/shopping/hotel-offers?cityCode={cityCode}&checkInDate={checkIn}&checkOutDate={checkOut}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AmadeusHotelResponse>(json);
    }

    public async Task<AmadeusFlightResponse> SearchFlightsAsync(string origin, string destination, string departureDate, string returnDate, int adults, int max = 5)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var url = $"/v2/shopping/flight-offers?originLocationCode={origin}&destinationLocationCode={destination}&departureDate={departureDate}&returnDate={returnDate}&adults={adults}&max={max}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AmadeusFlightResponse>(json);
    }

    public async Task<AmadeusHotelCityResponse> SearchHotelsByCityAsync(string cityCode)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var url = $"/v1/reference-data/locations/hotels/by-city?cityCode={cityCode}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AmadeusHotelCityResponse>(json);
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

public class AmadeusHotelResponse
{
    public List<HotelOffer> Data { get; set; } = new();
}

public class HotelOffer
{
    public string Id { get; set; } = string.Empty;
    public Hotel Hotel { get; set; } = new();
    public List<Offer> Offers { get; set; } = new();
}

public class Hotel
{
    public string HotelId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CityCode { get; set; } = string.Empty;
}

public class Offer
{
    public string Id { get; set; } = string.Empty;
    public Price Price { get; set; } = new();
}

public class Price
{
    public string Currency { get; set; } = string.Empty;
    public decimal Total { get; set; }
}