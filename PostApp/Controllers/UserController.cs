using PostApp.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Users;
using PostApp.Middlewares;
using PostApp.Core.Application.DTOs.Account;
using Microsoft.AspNetCore.Authorization;

namespace PostApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AuthenticationResponse _userViewModel;

        public UserController(IUserService service, IHttpContextAccessor httpContextAccessor)
        {
            _userService = service;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        #region Login & Logout
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Index(bool hasError = false, string? message = null)
        {
            var login = new LoginViewModel();

            if (hasError)
            {
                login.HasError = hasError;
                login.Error = message;
            }

            return View(login);
        }

        public async Task<IActionResult> RedirectIndex(string? ReturnUrl)
        {
            return RedirectToRoute(new { controller = "User", action = "Index", hasError = true, message = "You don't have access to this section!" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);

            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();

            HttpContext.Session.Remove("user");

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
        #endregion

        #region Register
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Register()
        {
            return View(new RegisterUserViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
            GenericResponse response = await _userService.RegisterAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(userId, token);

            return View("ConfirmEmail", response);
        }
        #endregion

        #region ForgotPassword
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
            GenericResponse response = await _userService.ForgotPasswordAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
		#endregion

		#region UpdateProfile
		[Authorize]
		public async Task<IActionResult> UpdateProfile(string username)
        {
            UpdateUserViewModel vm = await _userService.GetByUsername(username);
            ViewBag.User = _userViewModel;

            return View(vm);
        }

		[Authorize]
		[HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.User = _userViewModel;
                return View(vm);
            }

            AuthenticationResponse response = await _userService.UpdateAsync(vm, vm.UserName);

            if (response != null && response.HasError != true)
            {
                // Update Session properties
                var user = HttpContext.Session.Get<AuthenticationResponse>("user");
                user.Email = response.Email;
                user.ImageUrl = response.ImageUrl;
                HttpContext.Session.Set<AuthenticationResponse>("user", user);

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                if (response != null && response.Error == "An error has ocurred trying to update your password.")
                {
                    // Update Session properties
                    var user = HttpContext.Session.Get<AuthenticationResponse>("user");
                    user.Email = response.Email;
                    user.ImageUrl = response.ImageUrl;
                    HttpContext.Session.Set<AuthenticationResponse>("user", user);
                    _userViewModel = HttpContext.Session.Get<AuthenticationResponse>("user");
                }

                vm.HasError = response.HasError;
                vm.Error = response.Error;
                ViewBag.User = _userViewModel;
                return View(vm);
            }
        }
        #endregion
    }
}
