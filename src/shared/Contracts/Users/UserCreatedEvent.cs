namespace Shared.Contracts.Users;
public class UserCreatedEvent
{
    public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
}

public class UserRegisteredEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public DateTime RegisteredAt { get; set; }
}