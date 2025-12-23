using PostApp.Core.Application.Interfaces.Repositories.CRUD;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Interfaces.Repositories
{
    public interface ICommentRepository : ICreate<Comment>
    {
		Task<List<Comment>> GetAllAsync();
	}
}
