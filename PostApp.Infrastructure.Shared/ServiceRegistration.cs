using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Domain.Settings;
using PostApp.Infrastructure.Shared.Services;

namespace PostApp.Infrastructure.Shared
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            #region "Service"
            services.AddTransient<IEmailService, EmailService>();
            #endregion
        }
    }
}
