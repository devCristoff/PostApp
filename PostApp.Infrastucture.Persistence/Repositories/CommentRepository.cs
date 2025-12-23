using Microsoft.EntityFrameworkCore;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Core.Domain.Entities;
using PostApp.Infrastructure.Persistence.Contexts;

namespace PostApp.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _dbContext;

        public CommentRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

		public async Task AddAsync(Comment comment)
		{
			await _dbContext.Set<Comment>().AddAsync(comment);

			await _dbContext.SaveChangesAsync();
		}
		
		public async Task<List<Comment>> GetAllAsync()
		{
			return await _dbContext.Set<Comment>().ToListAsync(); 
		}
	}
}
