using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostApp.Infrastructure.Persistence.Repositories;
using PostApp.Infrastructure.Persistence.Contexts;
using PostApp.Core.Application.Interfaces.Repositories;

namespace PostApp.Infrastructure.Persistence
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration config)
        {
            #region "Context Configurations"

            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("Default");
                services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString, migration => migration.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)), ServiceLifetime.Scoped);
            }

            #endregion

            #region "Repositories"
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IFriendRepository, FriendRepository>();
            #endregion
        }
    }
}
