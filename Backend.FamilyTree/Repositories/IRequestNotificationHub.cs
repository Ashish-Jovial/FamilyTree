using Backend.FamilyTree.SignalRNotifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Backend.FamilyTree.Repositories
{
    public interface IRequestNotificationHub<T> where T : class
    {
        Task<T> CreateRequestAsync(T senderId);
        Task<T> ApproveRequestAsync(T requestId);
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

        public async Task<T> CreateRequestAsync(T request)
        {
            await _dbSet.AddAsync(request);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New request created");
            return request;
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

//public interface IRequestService<Request> where Request : class
//{
//    Task<Request> CreateRequestAsync(Guid senderId, Guid receiverId, string message);
//    Task<Request> ApproveRequestAsync(Guid requestId);
//}

//public class RequestService<T> : IRequestService
//{
//    private readonly IRepository<User> _repository;
//    private readonly IHubContext<NotificationHub> _hubContext;

//    public RequestService(IRepository<User> repository, IHubContext<NotificationHub> hubContext)
//    {
//        _repository = repository;
//        _hubContext = hubContext;
//    }

//    public async Task<Request> CreateRequestAsync(Guid senderId, Guid receiverId, string message)
//    {
//        var request = await _repository.GetByIdAsync(senderId);
//        if (request == null)
//        {
//            request = null;
//        }
//        else
//        {
//            //await _repository.CreateRequestAsync(request);

//            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New request created");
//            await _repository.SaveChangesAsync(request);
//        }

//        return request;
//    }

//    public async Task<Request> ApproveRequestAsync(Guid requestId)
//    {
//        var request = await _repository.GetByIdAsync(requestId);
//        if (request == null)
//        {
//            return null;
//        }

//        //await _repository.ApproveRequestAsync(request);
//        await _repository.SaveChangesAsync();
//        await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Request approved");
//        return request;
//    }
//}