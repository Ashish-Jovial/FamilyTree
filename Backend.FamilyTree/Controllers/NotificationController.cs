using Backend.FamilyTree.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.FamilyTree.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.FamilyTree.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NotificationController(IRequestNotificationHub<Request> requestNotificationHub) : ControllerBase
    {
        private readonly IRequestNotificationHub<Request> _requestNotificationHub = requestNotificationHub;

        // POST api/<NotificationController>
        [HttpPost("send-request")]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] Request request)
        {
            await _requestNotificationHub.CreateRequestAsync(request);
            await _requestNotificationHub.SaveChangesAsync();
            return request;
        }

        // PUT api/<NotificationController>/5
        [HttpPut("approve-request")]
        public async Task<IActionResult> ApproveRequest([FromBody] Request request)
        {
            await _requestNotificationHub.ApproveRequestAsync(request);
            await _requestNotificationHub.SaveChangesAsync();
            return NoContent();
        }
    }
}
