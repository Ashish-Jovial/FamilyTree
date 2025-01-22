using Backend.FamilyTree.Repositories;
using Models.FamilyTree.Models;

namespace Backend.FamilyTree.Services
{
    public interface ILoggingService
    {
        Task LogEventAsync(string eventType, string message);
    }

    public class LoggingService : ILoggingService
    {
        private readonly ILogRepository _logRepository;

        public LoggingService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task LogEventAsync(string eventType, string message)
        {
            var log = new Log
            {
                LogId = Guid.NewGuid(),
                EventType = eventType,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await _logRepository.AddAsync(log);
            await _logRepository.SaveChangesAsync();
        }
    }
}
