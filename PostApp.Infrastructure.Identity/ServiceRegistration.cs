using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Infrastructure.Identity.Contexts;
using PostApp.Infrastructure.Identity.Entities;
using PostApp.Infrastructure.Identity.Services;

namespace PostApp.Infrastructure.Identity
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceRegistration
    {
        public static void AddIdentityLayer(this IServiceCollection services, IConfiguration config)
        {
            #region "Context Configurations"

            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(opt => opt.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("IdentityConnection");
                services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString, migration => migration.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }

            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/RedirectIndex";
            });

            services.AddAuthentication();
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
    }
}
