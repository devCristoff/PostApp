using Microsoft.Extensions.DependencyInjection;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.Services;
using System.Reflection;

namespace PostApp.Core.Application
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region "Service"
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFriendService, FriendService>();
            #endregion
        }
    }
}
