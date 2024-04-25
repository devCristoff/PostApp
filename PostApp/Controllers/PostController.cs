using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Posts;

namespace PostApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {   
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

		#region Create
		[HttpPost]
        public async Task<IActionResult> Create(SavePostViewModel vm)
        {
            await _postService.Add(vm);

			return RedirectToRoute(new { controller = "Home", action = "Index" });
		}
		#endregion

		#region Edit
		public async Task<IActionResult> Edit(int id)
		{
			SavePostViewModel post = await _postService.GetByIdSaveViewModel(id);

			return View("EditPost", post);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(SavePostViewModel vm)
		{
			await _postService.Update(vm, vm.Id);

			return RedirectToRoute(new { controller = "Home", action = "Index" });
		}
		#endregion

		#region Delete
		public async Task<IActionResult> Delete(int id)
		{
			SavePostViewModel post = await _postService.GetByIdSaveViewModel(id);

			return View("Delete", post);
		}

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> DeletePost(int id)
		{
			await _postService.Delete(id);

			return RedirectToRoute(new { controller = "Home", action = "Index" });
		}
		#endregion
	}
}
