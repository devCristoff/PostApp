using PostApp.Core.Application.DTOs.Account;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        
        Task<string> ConfirmAccountAsync(string userId, string token);
        
        Task<GenericResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        
        Task<GenericResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        
        Task<UserProfile> FindByUsernameAsync(string username);
        
        Task<UserProfile> FindByIdAsync(string id);
        
        Task<AuthenticationResponse> UpdateAsync(UpdateProfileRequest request, string id);
        
        Task SingOutAsync();
    }
}