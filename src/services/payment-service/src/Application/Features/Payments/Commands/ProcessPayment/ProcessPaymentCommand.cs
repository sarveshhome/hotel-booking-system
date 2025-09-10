using MediatR;
using Payment.Service.Domain.Entities;
using Payment.Service.Application.Common.Interfaces;
using Shared.Contracts.Payments;

namespace Payment.Service.Application.Features.Payments.Commands.ProcessPayment;

public record ProcessPaymentCommand : IRequest<Guid>
{
    public Guid BookingId { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public string CardNumber { get; init; } = string.Empty;
    public string CardholderName { get; init; } = string.Empty;
    public string CardExpiryMonth { get; init; } = string.Empty;
    public string CardExpiryYear { get; init; } = string.Empty;
    public string Cvv { get; init; } = string.Empty;
}

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;

    public ProcessPaymentCommandHandler(IApplicationDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Extract last 4 digits from card number
        var lastFourDigits = request.CardNumber.Length >= 4 
            ? request.CardNumber.Substring(request.CardNumber.Length - 4) 
            : string.Empty;

        // Determine card type (simplified logic)
        var cardType = DetermineCardType(request.CardNumber);

        // Simulate payment processing
        var isPaymentSuccessful = await SimulatePaymentProcessing(request, cancellationToken);

        var payment = new Domain.Entities.Payment
        {
            BookingId = request.BookingId,
            UserId = request.UserId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = isPaymentSuccessful ? "Completed" : "Failed",
            TransactionId = Guid.NewGuid().ToString(),
            GatewayResponse = isPaymentSuccessful ? "SUCCESS" : "DECLINED",
            ProcessedAt = isPaymentSuccessful ? DateTime.UtcNow : null,
            FailedAt = !isPaymentSuccessful ? DateTime.UtcNow : null,
            FailureReason = !isPaymentSuccessful ? "Card declined" : string.Empty,
            CardLastFourDigits = lastFourDigits,
            CardType = cardType,
            CardholderName = request.CardholderName,
            CardExpiryDate = ParseExpiryDate(request.CardExpiryMonth, request.CardExpiryYear),
            Created = DateTime.UtcNow,
            CreatedBy = request.UserId.ToString()
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish payment event
        if (isPaymentSuccessful)
        {
            await _eventBus.PublishAsync(new PaymentProcessedEvent
            {
                PaymentId = payment.Id,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                ProcessedAt = payment.ProcessedAt!.Value
            }, cancellationToken);
        }
        else
        {
            await _eventBus.PublishAsync(new PaymentFailedEvent
            {
                PaymentId = payment.Id,
                BookingId = payment.BookingId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                FailureReason = payment.FailureReason,
                FailedAt = payment.FailedAt!.Value
            }, cancellationToken);
        }

        return payment.Id;
    }

    private async Task<bool> SimulatePaymentProcessing(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Simulate network delay
        await Task.Delay(1000, cancellationToken);
        
        // Simple validation - reject cards ending with '0000'
        return !request.CardNumber.EndsWith("0000");
    }

    private string DetermineCardType(string cardNumber)
    {
        if (cardNumber.StartsWith("4"))
            return "Visa";
        if (cardNumber.StartsWith("5"))
            return "MasterCard";
        if (cardNumber.StartsWith("3"))
            return "American Express";
        return "Unknown";
    }

    private DateTime? ParseExpiryDate(string month, string year)
    {
        if (int.TryParse(month, out var monthNum) && int.TryParse(year, out var yearNum))
        {
            return new DateTime(yearNum, monthNum, 1);
        }
        return null;
    }
}
