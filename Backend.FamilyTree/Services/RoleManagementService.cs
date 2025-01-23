using Backend.FamilyTree.Repositories;
using Models.FamilyTree.Models;

namespace Backend.FamilyTree.Services
{
    public interface IRoleManagementService
    {
        Task RequestRoleChangeAsync(Guid userId, string requestedRole);
        Task ApproveRoleChangeAsync(Guid requestId);
    }
    public class RoleManagementService : IRoleManagementService
    {
        private readonly IRepository<RoleChangeRequest> _roleChangeRequestRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRoles> _userRoleRepository;
        private readonly ILoggingService _loggingService;

        public RoleManagementService(IRepository<RoleChangeRequest> roleChangeRequestRepository, IRepository<User> userRepository, IRepository<UserRoles> userRoleRepository, ILoggingService loggingService)
        {
            _roleChangeRequestRepository = roleChangeRequestRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _loggingService = loggingService;
        }

        public async Task RequestRoleChangeAsync(Guid userId, string requestedRole)
        {
            var roleChangeRequest = new RoleChangeRequest
            {
                RequestId = Guid.NewGuid(),
                UserId = userId,
                RequestedRole = requestedRole,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _roleChangeRequestRepository.AddAsync(roleChangeRequest);
            await _roleChangeRequestRepository.SaveChangesAsync();

            await _loggingService.LogEventAsync("RoleChangeRequest", $"User {userId} requested role change to {requestedRole}");
        }

        public async Task ApproveRoleChangeAsync(Guid requestId)
        {
            var request = await _roleChangeRequestRepository.GetByIdAsync(requestId);
            if (request == null || request.Status != "Pending")
            {
                throw new Exception("Invalid role change request");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var role = await _userRoleRepository.GetAllAsync();
            var userRole = role.FirstOrDefault(r => r.RoleName == request.RequestedRole);
            if (userRole == null)
            {
                throw new Exception("Role not found");
            }

            user.Roles.Clear();
            user.Roles.Add(userRole);

            request.Status = "Approved";
            request.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            _roleChangeRequestRepository.Update(request);

            await _userRepository.SaveChangesAsync();
            await _roleChangeRequestRepository.SaveChangesAsync();

            await _loggingService.LogEventAsync("RoleChangeApproved", $"User {user.UserID} role changed to {request.RequestedRole}");
        }
    }
}
