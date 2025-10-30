namespace NotificationService.Models;

public class NotificationRequest
{
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
}

public enum NotificationType
{
    Email,
    SMS,
    PushNotification
}