using Grooveyard.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Grooveyard.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _notificationService.GetNotificationsAsync(userId);

            return Ok(notifications.OrderByDescending(x => x.CreatedAt));
        }

        [HttpPost("notifications/read")]
        public async Task<IActionResult> MarkNotificationAsRead([FromBody]List<string> ids)
        {
            var notification = await _notificationService.MarkNotificationAsRead(ids);

            if (notification)
                return Ok();

            return NotFound();
        }


    }



}
