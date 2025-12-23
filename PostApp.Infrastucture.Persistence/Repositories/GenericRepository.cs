using Microsoft.EntityFrameworkCore;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Infrastructure.Persistence.Contexts;

namespace PostApp.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _dbContext;

        public GenericRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task AddAsync(Entity entity)
        {
            await _dbContext.Set<Entity>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            Entity entry = await _dbContext.Set<Entity>().FindAsync(id);

            _dbContext.Entry(entry).CurrentValues.SetValues(entity);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _dbContext.Set<Entity>().ToListAsync(); //Deferred execution
        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Entity>().FindAsync(id);
        }

        public virtual async Task DeleteAsync(Entity entity)
        {
            _dbContext.Set<Entity>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
