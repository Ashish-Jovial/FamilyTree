using Backend.FamilyTree.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models.FamilyTree.Models;

namespace Backend.FamilyTree.SignalRNotifications
{
    [Authorize]
    public class NotificationHub: Hub
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationHub(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task SendNotification(string userId, string userName, string message)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                UserName = userName,
                EventMessage = message,
                Timestamp = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            await Clients.User(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}
