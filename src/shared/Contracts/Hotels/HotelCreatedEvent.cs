namespace Shared.Contracts.Hotels;

public class HotelCreatedEvent
{
    public Guid HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int StarRating { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class HotelUpdatedEvent
{
    public Guid HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int StarRating { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class HotelDeletedEvent
{
    public Guid HotelId { get; set; }
    public DateTime DeletedAt { get; set; }
}
