namespace Shared.Contracts.Payments;

public class PaymentProcessedEvent
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}

public class PaymentFailedEvent
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string FailureReason { get; set; } = string.Empty;
    public DateTime FailedAt { get; set; }
}

public class PaymentRefundedEvent
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public decimal RefundAmount { get; set; }
    public string RefundReason { get; set; } = string.Empty;
    public DateTime RefundedAt { get; set; }
}
