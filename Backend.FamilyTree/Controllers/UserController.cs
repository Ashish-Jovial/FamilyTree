using Backend.FamilyTree.Repositories;
using Backend.FamilyTree.Services;
using Backend.FamilyTree.SignalRNotifications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.FamilyTree.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IAuthenticationService = Backend.FamilyTree.Services.IAuthenticationService;

namespace Backend.FamilyTree.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILoggingService _loggingService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="notificationHubContext">The SignalR notification hub context.</param>
        public UserController(IRepository<User> userRepository,
            IHubContext<NotificationHub> notificationHubContext,
            INotificationRepository notificationRepository,
            ILoggingService loggingService,
            IAuthenticationService authenticationService,
            ISearchService searchService)
        { 
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _notificationHubContext = notificationHubContext ?? throw new ArgumentNullException(nameof(notificationHubContext));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authenticationService.RegisterUserAsync(model);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("RegisterUserError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authenticationService.LoginUserAsync(model);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("LoginUserError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                return BadRequest("External authentication failed");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var provider = result.Properties.Items[".AuthScheme"];
            var providerUserId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (providerUserId == null || email == null || name == null)
            {
                return BadRequest("External authentication failed");
            }

            try
            {
                var token = await _authenticationService.ExternalLoginAsync(provider, providerUserId, email, name);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("ExternalLoginError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                await _loggingService.LogEventAsync("GetUsers", "Retrieved all users");
                return Ok(users);
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("GetUsersError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a user by its identifier.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns>The user.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                {
                    await _loggingService.LogEventAsync("GetUserNotFound", $"User with ID {id} not found");
                    return NotFound();
                }

                await _loggingService.LogEventAsync("GetUser", $"Retrieved user with ID {id}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("GetUserError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new user and sends a notification to the admin.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The created user.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
                await SendNotificationToAdminAsync(user, "User Created");
                await _loggingService.LogEventAsync("CreateUser", $"Created user with ID {user.UserID}");
                return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("CreateUserError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="user">The user to update.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                await _loggingService.LogEventAsync("UpdateUser", $"Updated user with ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("UpdateUserError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a user by its identifier.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                {
                    await _loggingService.LogEventAsync("DeleteUserNotFound", $"User with ID {id} not found");
                    return NotFound();
                }

                _userRepository.Delete(user);
                await _userRepository.SaveChangesAsync();
                await _loggingService.LogEventAsync("DeleteUser", $"Deleted user with ID {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("DeleteUserError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SearchResultModel>>> Search([FromQuery] SearchRequestModel query)
        {
            try
            {
                var results = await _searchService.SearchAsync(query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                await _loggingService.LogEventAsync("SearchError", ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Sends a notification to the admin when a user is created.
        /// </summary>
        /// <param name="user">The user associated with the event.</param>
        /// <param name="eventMessage">The event message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SendNotificationToAdminAsync(User user, string eventMessage)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = user.UserID,
                UserName = user.UserName,
                EventMessage = eventMessage,
                Timestamp = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
            await _notificationHubContext.Clients.Group("Admins").SendAsync("ReceiveNotification", notification);
        }
    }
}