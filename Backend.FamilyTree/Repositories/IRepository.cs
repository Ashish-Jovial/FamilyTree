using Microsoft.EntityFrameworkCore;

namespace Backend.FamilyTree.Repositories
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SaveChangesAsync();
    }

    /// <summary>
    /// Generic repository implementation for CRUD operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly FamilyTreeDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(FamilyTreeDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity ?? throw new KeyNotFoundException($"Entity with ID '{id}' was not found.");
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

// AsNoTracking: Used AsNoTracking for the GetAllAsync method to improve performance for read-only operations.
// IDisposable Implementation: Implemented IDisposable in the Repository class to ensure the DbContext is properly disposed of.

