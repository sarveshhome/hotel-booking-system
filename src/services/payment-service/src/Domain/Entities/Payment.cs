namespace Payment.Service.Domain.Entities;

public class Payment : BaseAuditableEntity
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentMethod { get; set; } = string.Empty; // CreditCard, DebitCard, PayPal, etc.
    public string PaymentStatus { get; set; } = string.Empty; // Pending, Processing, Completed, Failed, Refunded
    public string TransactionId { get; set; } = string.Empty;
    public string GatewayResponse { get; set; } = string.Empty;
    public DateTime? ProcessedAt { get; set; }
    public DateTime? FailedAt { get; set; }
    public string FailureReason { get; set; } = string.Empty;
    
    // Payment method details
    public string CardLastFourDigits { get; set; } = string.Empty;
    public string CardType { get; set; } = string.Empty; // Visa, MasterCard, etc.
    public string CardholderName { get; set; } = string.Empty;
    public DateTime? CardExpiryDate { get; set; }
    
    // Refund information
    public decimal? RefundAmount { get; set; }
    public DateTime? RefundedAt { get; set; }
    public string RefundReason { get; set; } = string.Empty;
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
