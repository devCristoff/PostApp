using Microsoft.EntityFrameworkCore;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Core.Domain.Entities;
using PostApp.Infrastructure.Persistence.Contexts;

namespace PostApp.Infrastructure.Persistence.Repositories
{
	public class FriendRepository : IFriendRepository
	{
		private readonly ApplicationContext _dbContext;

        public FriendRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

		public virtual async Task AddAsync(Friend friend)
		{
			await _dbContext.Set<Friend>().AddAsync(friend);

			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<Friend>> GetAllAsync()
		{
			return await _dbContext.Set<Friend>().ToListAsync();
		}

		public async Task<Friend> GetByIdAsync(string userId, string friendId)
		{
			return await _dbContext.Set<Friend>().FirstOrDefaultAsync(friend => friend.UserId == userId && friend.FriendId == friendId);
		}

		public async Task DeleteAsync(Friend friend)
		{
			_dbContext.Set<Friend>().Remove(friend);

			await _dbContext.SaveChangesAsync();
		}
	}
}
