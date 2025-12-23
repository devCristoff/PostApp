using Microsoft.AspNetCore.Mvc;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using PostApp.Core.Application.ViewModels.Friends;

namespace PostApp.Controllers
{
	[Authorize]
	public class FriendController : Controller
	{
		private readonly IFriendService _friendService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AuthenticationResponse _userViewModel;

		public FriendController(IFriendService friendService, IHttpContextAccessor httpContextAccessor, IUserService userService)
		{
			_friendService = friendService;
			_httpContextAccessor = httpContextAccessor;
			_userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
		}

		public async Task<IActionResult> Index(bool hasError = false, string? message = null)
		{
			ViewBag.User = _userViewModel;
			ViewBag.Friends = await _friendService.GetAllViewModel();
			ViewBag.HasError = hasError;
			ViewBag.Message = message;

			return View(await _friendService.GetAllFriendPost());
		}

		#region Create
		[HttpPost]
		public async Task<IActionResult> Create(SaveFriendViewModel vm)
		{
			vm = await _friendService.Add(vm);
			string Message = vm.Error;

            if (!vm.HasError)
				Message = $"Friend '{vm.Username}' added successfuly!";

            return RedirectToRoute(new { controller = "Friend", action = "Index", hasError = vm.HasError, message = Message });
		}
		#endregion

		#region Delete

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> DeletePost(string friendId)
		{
			await _friendService.Delete(friendId);

			return RedirectToRoute(new { controller = "Friend", action = "Index" });
		}
		#endregion
	}
}
