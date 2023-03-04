using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Common;
using SignalR.Models;
using SignalR.Selfhosted.Hubs;
using System.Threading.Tasks;

namespace SignalR.Selfhosted.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("all")]
        public async Task<IActionResult> SendAll()
        {
            await _hubContext.Clients.All
                .SendCoreAsync(Constants.NotificationCreatedEvent,
                    new object[]{
                        new NotificationMessageModel
                        {
                            Id = 1,
                            Content = "Some Content",
                            Title = "Some Title"
                        }
                    });

            return Ok("Sent to all connected clients.");
        }

        [HttpPost("{groupName}")]
        public async Task<IActionResult> SendToGroup([FromRoute] string groupName)
        {
            await _hubContext.Clients
                .Groups(groupName)
                .SendCoreAsync(Constants.NotificationCreatedEvent,
                    new object[]{
                        new NotificationMessageModel
                        {
                            Id = 1,
                            Content = "Some Content",
                            Title = "Some Title"
                        }
                    });

            return Ok($"Sent to all connected clients of {groupName} group.");
        }
    }
}