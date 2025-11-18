namespace Pricing.Service.Infrastructure.Services;

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