using Microsoft.EntityFrameworkCore;
using Models.FamilyTree.Models;
using System.Drawing.Printing;

namespace Backend.FamilyTree.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification> GetByIdAsync(Guid id);
        Task AddAsync(Notification notification);
        void Update(Notification notification);
        void Delete(Notification notification);
        Task SaveChangesAsync();
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly FamilyTreeDbContext _context;

        public NotificationRepository(FamilyTreeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return notification ?? throw new KeyNotFoundException("Notification not found");
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
        }

        public void Delete(Notification notification)
        {
            _context.Notifications.Remove(notification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
