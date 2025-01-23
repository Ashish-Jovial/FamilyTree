using Backend.FamilyTree.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Models.FamilyTree.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.FamilyTree.Services
{
    public interface IAuthenticationService
    {
        Task RegisterUserAsync(UserRegistrationModel model);
        Task<string> LoginUserAsync(UserLoginModel model);
        Task<string> ExternalLoginAsync(string provider, string providerUserId, string email, string name);
    }
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILoggingService _loggingService;
        private readonly INotificationRepository _notificationRepository;

        public AuthenticationService(IRepository<User> userRepository, ILoggingService loggingService, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _loggingService = loggingService;
            _notificationRepository = notificationRepository;
        }

        public async Task RegisterUserAsync(UserRegistrationModel model)
        {
            var users = await _userRepository.GetAllAsync();
            if (users.Any(u => u.UserName == model.UserName))
            {
                throw new Exception("Username already exists");
            }

            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                PasswordSalt = hmac.Key,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Roles = new List<UserRoles> { new UserRoles { RoleName = "user" } } // Default role
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            await _loggingService.LogEventAsync("RegisterUser", $"User {user.UserName} registered successfully");

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = user.UserID,
                UserName = user.UserName,
                EventMessage = "User Registered",
                Timestamp = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task<string> LoginUserAsync(UserLoginModel model)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.UserName == model.UserName);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    throw new Exception("Invalid username or password");
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("yoursecretkey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(r => r.RoleName)))
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> ExternalLoginAsync(string provider, string providerUserId, string email, string name)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Provider == provider && u.ProviderUserId == providerUserId);

            if (user == null)
            {
                user = new User
                {
                    UserID = Guid.NewGuid(),
                    UserName = name,
                    Email = email,
                    Provider = provider,
                    ProviderUserId = providerUserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Roles = new List<UserRoles> { new UserRoles { RoleName = "user" } } // Default role
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                await _loggingService.LogEventAsync("ExternalLogin", $"User {user.UserName} logged in with {provider}");

                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    UserId = user.UserID,
                    UserName = user.UserName,
                    EventMessage = $"User logged in with {provider}",
                    Timestamp = DateTime.UtcNow
                };

                await _notificationRepository.AddAsync(notification);
                await _notificationRepository.SaveChangesAsync();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("yoursecretkey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(r => r.RoleName)))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}