using Backend.FamilyTree.SignalRNotifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Backend.FamilyTree.Repositories
{
    public interface IRequestNotificationHub<T> where T : class
    {
        Task<T> CreateRequestAsync(T requestId);
        Task<T> ApproveRequestAsync(T senderId);
        Task SaveChangesAsync();
    }

    public class RequestNotificationHub<T>(FamilyTreeDbContext context, IHubContext<NotificationHub> hubContext) : IRequestNotificationHub<T> where T: class
    {
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;
        private readonly FamilyTreeDbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T> ApproveRequestAsync(T senderId)
        {

            //return await _dbSet.FindAsync(requestId);
            var entity = await _dbSet.FindAsync(senderId);
            await _hubContext.Clients.Clients(senderId.ToString()).SendAsync("ReceiveNotification", "Your request has been approved.");
            return entity is null ? throw new KeyNotFoundException($"Entity with ID '{senderId}' was not found.") : entity;
        }

        public async Task<T> CreateRequestAsync(T requestId)
        {
            await _dbSet.AddAsync(requestId);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New request created");
            return requestId;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
