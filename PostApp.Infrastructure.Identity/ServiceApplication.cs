using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PostApp.Infrastructure.Identity.Entities;
using PostApp.Infrastructure.Identity.Seeds;

namespace PostApp.Infrastructure.Identity
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceApplication
    {
        public static async Task AddIdentitySeeds(this IServiceProvider services)
        {
            #region "Identity Seeds"
            using(var scope = services.CreateScope())
            {
                var serviceScope = scope.ServiceProvider;

                try
                {
                    var userManager = serviceScope.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceScope.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(roleManager);
                    await DefaulBasicUser.SeedAsync(userManager);
                }
                catch (Exception ex) { }
            }
            #endregion
        }
    }
}
