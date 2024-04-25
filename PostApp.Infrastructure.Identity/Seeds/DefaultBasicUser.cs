using Microsoft.AspNetCore.Identity;
using PostApp.Core.Application.Enums;
using PostApp.Infrastructure.Identity.Entities;

namespace PostApp.Infrastructure.Identity.Seeds
{
    public static class DefaulBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new() 
            {
                UserName = "cris",
                Email = "cristoferdelamota0105@gmail.com",
                FirstName = "Cristofer",
                LastName = "De La Mota",
                PhoneNumber = "(829) 694-1107",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123P4$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }
    }
}
