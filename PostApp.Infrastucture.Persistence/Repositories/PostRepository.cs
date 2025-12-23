using Microsoft.EntityFrameworkCore;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Core.Domain.Entities;
using PostApp.Infrastructure.Persistence.Contexts;

namespace PostApp.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationContext _dbContext;

        public PostRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
	}
}
