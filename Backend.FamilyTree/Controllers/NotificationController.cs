using Backend.FamilyTree.Repositories;
using Backend.FamilyTree.SignalRNotifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.FamilyTree.Models;
using System;
using System.Threading.Tasks;

namespace Backend.FamilyTree.Controllers
{
    /// <summary>
    /// Controller for managing notifications.
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IRequestNotificationHub<Request> _requestNotificationHub;
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="requestNotificationHub">The request notification hub.</param>
        public NotificationController(IRequestNotificationHub<Request> requestNotificationHub, IHubContext<NotificationHub> notificationHubContext)
        {
            _requestNotificationHub = requestNotificationHub ?? throw new ArgumentNullException(nameof(requestNotificationHub));
            _notificationHubContext = notificationHubContext ?? throw new ArgumentNullException(nameof(notificationHubContext));
        }

        /// <summary>
        /// Creates a new request and sends a notification.
        /// </summary>
        /// <param name="request">The request to create.</param>
        /// <returns>The created request.</returns>
        [HttpPost("send-request")]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _requestNotificationHub.CreateRequestAsync(request);
                await _requestNotificationHub.SaveChangesAsync();
                await SendNotificationAsync(request, "Request Created");
                return CreatedAtAction(nameof(CreateRequest), new { id = request.RequestId }, request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Approves a request and sends a notification.
        /// </summary>
        /// <param name="request">The request to approve.</param>
        /// <returns>No content.</returns>
        [HttpPut("approve-request")]
        public async Task<IActionResult> ApproveRequest([FromBody] Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _requestNotificationHub.ApproveRequestAsync(request);
                await _requestNotificationHub.SaveChangesAsync();
                await SendNotificationAsync(request, "Request Approved");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a notification for a specific event.
        /// </summary>
        /// <param name="request">The request associated with the event.</param>
        /// <param name="eventMessage">The event message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SendNotificationAsync(Request request, string eventMessage)
        {
            var notificationMessage = new
            {
                RequestId = request.RequestId,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Message = request.Message,
                Status = request.Status,
                EventMessage = eventMessage,
                Timestamp = DateTime.UtcNow
            };

            await _notificationHubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessage);
        }
    }
}