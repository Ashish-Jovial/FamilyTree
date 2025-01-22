using Microsoft.EntityFrameworkCore;
using Models.FamilyTree.Models;

namespace Backend.FamilyTree.Repositories
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> GetAllAsync();
        Task<Log> GetByIdAsync(Guid id); // Updated to match the implementation
        Task AddAsync(Log log);
        void Update(Log log);
        void Delete(Log log);
        Task SaveChangesAsync();
    }

    public class LogRepository : ILogRepository
    {
        private readonly FamilyTreeDbContext _context;

        public LogRepository(FamilyTreeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            return await _context.Logs.ToListAsync();
        }

        public async Task<Log?> GetByIdAsync(Guid id)
        {
            return await _context.Logs.FindAsync(id);
        }

        public async Task AddAsync(Log log)
        {
            await _context.Logs.AddAsync(log);
        }

        public void Update(Log log)
        {
            _context.Logs.Update(log);
        }

        public void Delete(Log log)
        {
            _context.Logs.Remove(log);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
