using Microsoft.AspNetCore.Authorization;
using PostApp.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.Interfaces.Services;

namespace PostApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AuthenticationResponse _userViewModel;

		public HomeController(IPostService postService, IHttpContextAccessor httpContextAccessor)
        {
            _postService = postService;
			_httpContextAccessor = httpContextAccessor;
			_userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
		}

        public async Task<IActionResult> Index()
        {
            ViewBag.User = _userViewModel;

            return View(await _postService.GetAllViewModelWithInclude());
        }
	}
}
