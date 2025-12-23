using Microsoft.AspNetCore.Identity;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.Enums;
using PostApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.DTOs.Email;
using PostApp.Core.Application.Helpers;

namespace PostApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        #region Login & Logout
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No accounts registered with {request.UserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.UserName}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account not confirmed for {request.UserName}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.ImageUrl = user.ImageUrl;

            //var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            //response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SingOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion

        #region Register & Confirm Email
        public async Task<GenericResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            GenericResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                ImageUrl = request.ImageUrl,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                var verificationUri = await SendVerificationEmailUrl(user, origin);
                await _emailService.SendAsync(new EmailRequest()
                {
                    To = user.Email,
                    Subject = "Confirm Registration",
                    Body = $"Please confirm your account visiting this URL {verificationUri}",
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error has ocurred trying to register the user.";
                return response;
            }

            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "No accounts registered with this user.";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed '{user.UserName}'. You can now use the app.";
            }
            else
            {
                return $"An error occured while confirming {user.UserName}";
            }
        }
        #endregion

        #region ForgotPassword
        public async Task<GenericResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            GenericResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No accounts registered with {request.Username}";
                return response;
            }

            // Update Password
            string newPassword = await PasswordHelper.UpdatePassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error has ocurred while reset password.";
                return response;
            }

            var Uri = new Uri(origin);

            await _emailService.SendAsync(new EmailRequest()
            {
                To = user.Email,
                Subject = "New Password",
                Body = $"This it your new password {newPassword}, visit {Uri} and Login.",
            });

            return response;
        }
        #endregion

        #region UpdateProfile
        public async Task<AuthenticationResponse> UpdateAsync(UpdateProfileRequest request, string username)
        {
            AuthenticationResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByNameAsync(username);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.ImageUrl = request.ImageUrl;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (request.Password != null)
                {
                    // Update Password
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, token, request.Password);

                    if (!result.Succeeded)
                    {
                        response.Email = user.Email;
                        response.ImageUrl = user.ImageUrl;
                        response.HasError = true;
                        response.Error = $"An error has ocurred trying to update your password.";
                        return response;
                    }
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error has ocurred trying to update the user {user.UserName}.";
                return response;
            }

            response.Email = user.Email;
            response.ImageUrl = user.ImageUrl;

            return response;
        }
        #endregion

        #region GetByUsername & GetById
        public async Task<UserProfile> FindByUsernameAsync(string username)
        {
			UserProfile updateProfile = new();

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
				updateProfile.Id = user.Id;
				updateProfile.UserName = user.UserName;
				updateProfile.FirstName = user.FirstName;
				updateProfile.LastName = user.LastName;
				updateProfile.PhoneNumber = user.PhoneNumber;
				updateProfile.Email = user.Email;
				updateProfile.ImageUrl = user.ImageUrl;

                return updateProfile;
			}

            return null;
        }

		public async Task<UserProfile> FindByIdAsync(string id)
		{
			UserProfile userProfile = new();

			var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
				userProfile.Id = user.Id;
				userProfile.UserName = user.UserName;
				userProfile.FirstName = user.FirstName;
				userProfile.LastName = user.LastName;
				userProfile.PhoneNumber = user.PhoneNumber;
				userProfile.Email = user.Email;
				userProfile.ImageUrl = user.ImageUrl;

				return userProfile;
			}

            return null;
		}
		#endregion

		#region Helpers
		private async Task<string> SendVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUrl = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", code);

            return verificationUrl;
        }
        #endregion
    }
}
