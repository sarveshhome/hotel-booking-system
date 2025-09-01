using MediatR;
using User.Service.Domain.Entities;
using User.Service.Application.Common.Interfaces;
using User.Service.Application.Features.Users;
using Shared.Contracts.Users;

namespace User.Service.Application.Features.Users.Commands.CreateUser;
public record CreateUserCommand : IRequest<Guid>
{
    public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;

    public CreateUserCommandHandler(IApplicationDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Created = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _eventBus.PublishAsync(new UserCreatedEvent
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return user.Id;
    }
}