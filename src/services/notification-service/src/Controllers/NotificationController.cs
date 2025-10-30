using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Notifications;

namespace NotificationService.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(ILogger<NotificationController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        _logger.LogInformation("Sending notification to {Recipient}", request.Recipient);
        // TODO: Implement notification sending logic
        return Ok(new { message = "Notification queued for delivery" });
    }
}