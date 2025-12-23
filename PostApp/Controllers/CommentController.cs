using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Comments;

namespace PostApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create(SaveCommentViewModel vm, string location)
        {
            await _commentService.Add(vm);

			return RedirectToRoute(new { controller = location, action = "Index" });

		}
		#endregion
	}
}
