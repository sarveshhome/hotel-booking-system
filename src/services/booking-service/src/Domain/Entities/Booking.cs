namespace Booking.Service.Domain.Entities;

public class Booking : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty; // Pending, Confirmed, Cancelled, Completed
    public string SpecialRequests { get; set; } = string.Empty;
    public DateTime? CancelledAt { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class Payment : BaseAuditableEntity
{
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty; // CreditCard, DebitCard, PayPal, etc.
    public string PaymentStatus { get; set; } = string.Empty; // Pending, Completed, Failed, Refunded
    public string TransactionId { get; set; } = string.Empty;
    public DateTime? ProcessedAt { get; set; }
    public string FailureReason { get; set; } = string.Empty;
    
    // Foreign key
    public virtual Booking Booking { get; set; } = null!;
}

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModified { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
}

public abstract class BaseEntity
{
    public Guid Id { get; set; }
}
