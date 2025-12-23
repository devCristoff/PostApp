using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.ViewModels.Users;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        
        Task<string> ConfirmEmailAsync(string userId, string token);
        
        Task<GenericResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        
        Task<GenericResponse> RegisterAsync(RegisterUserViewModel vm, string origin);
        
        Task<UpdateUserViewModel> GetByUsername(string username);

        Task<UserViewModel> GetById(string id);

        Task<UserProfile> FindFriend(string username);

		Task<AuthenticationResponse> UpdateAsync(UpdateUserViewModel vm, string username);
        
        Task SignOutAsync();
    }
}