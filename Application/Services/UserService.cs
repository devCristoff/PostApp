using AutoMapper;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.Helpers;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Users;

namespace PostApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        #region Login & Logout
        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);

            return userResponse;
        }

        public async Task SignOutAsync()
        {
            await _accountService.SingOutAsync();
        }
        #endregion

        #region Register & Confirm email
        public async Task<GenericResponse> RegisterAsync(RegisterUserViewModel vm, string origin)
        {
            vm.ImageUrl = await FileHelper.UploadImage(vm.File, $"{vm.UserName}/Profile");

            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);

            return await _accountService.RegisterBasicUserAsync(registerRequest, origin);
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }
        #endregion

        #region Forgot Password
        public async Task<GenericResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);

            return await _accountService.ForgotPasswordAsync(forgotRequest, origin);
        }
		#endregion

		#region GetByUsername & GetById
		public async Task<UpdateUserViewModel> GetByUsername(string username)
		{
			UserProfile userProfile = await _accountService.FindByUsernameAsync(username);

			UpdateUserViewModel vm = _mapper.Map<UpdateUserViewModel>(userProfile);

			return vm;
		}

		public async Task<UserViewModel> GetById(string id)
		{
			UserProfile userProfile = await _accountService.FindByIdAsync(id);

			UserViewModel vm = _mapper.Map<UserViewModel>(userProfile);

			return vm;
		}

		public async Task<UserProfile> FindFriend(string username)
		{
			return await _accountService.FindByUsernameAsync(username);
		}
		#endregion

		#region UpdateProfile
		public async Task<AuthenticationResponse> UpdateAsync(UpdateUserViewModel vm, string username)
        {
            vm.ImageUrl = await FileHelper.UploadImage(vm.File, $"{username}/Profile", true, vm.ImageUrl);
            UpdateProfileRequest updateRequest = _mapper.Map<UpdateProfileRequest>(vm);

            return await _accountService.UpdateAsync(updateRequest, username);
        }
        #endregion
    }
}
