using PostApp.Core.Application.Helpers;
using PostApp.Core.Application.DTOs.Account;

namespace PostApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _contextAccessor;
        
        public ValidateUserSession(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            return userViewModel != null;
        }
    }
}
