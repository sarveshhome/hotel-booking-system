namespace Pricing.Service.Infrastructure.Services;

public class AmadeusFlightResponse
{
    public List<FlightOffer> Data { get; set; } = new();
}

public class FlightOffer
{
    public string Id { get; set; } = string.Empty;
    public FlightPrice Price { get; set; } = new();
    public List<Itinerary> Itineraries { get; set; } = new();
}

public class FlightPrice
{
    public string Currency { get; set; } = string.Empty;
    public string Total { get; set; } = string.Empty;
}

public class Itinerary
{
    public string Duration { get; set; } = string.Empty;
    public List<Segment> Segments { get; set; } = new();
}

public class Segment
{
    public Departure Departure { get; set; } = new();
    public Arrival Arrival { get; set; } = new();
    public string CarrierCode { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
}

public class Departure
{
    public string IataCode { get; set; } = string.Empty;
    public DateTime At { get; set; }
}

public class Arrival
{
    public string IataCode { get; set; } = string.Empty;
    public DateTime At { get; set; }
}