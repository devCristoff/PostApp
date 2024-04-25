using PostApp.Core.Application.Interfaces.Repositories.CRUD;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Interfaces.Repositories
{
	public interface IFriendRepository : 
		ICreate<Friend>,
		IDelete<Friend>
	{
		Task<Friend> GetByIdAsync(string userId, string friendId);
		Task<List<Friend>> GetAllAsync();
	}
}
