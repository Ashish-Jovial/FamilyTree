using Microsoft.EntityFrameworkCore;

namespace Backend.FamilyTree.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }

    public class Repository<T>(FamilyTreeDbContext context) : IRepository<T> where T : class
    {
        private readonly FamilyTreeDbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T> GetByIdAsync(Guid id)
        {
            //return await _dbSet.FindAsync(id);
            var entity = await _dbSet.FindAsync(id);
            return entity is null ? throw new KeyNotFoundException($"Entity with ID '{id}' was not found.") : entity;
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }

}
