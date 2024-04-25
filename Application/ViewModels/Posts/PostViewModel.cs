using PostApp.Core.Application.ViewModels.Comments;
using PostApp.Core.Application.ViewModels.Users;

namespace PostApp.Core.Application.ViewModels.Posts
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
		public DateTime Created { get; set; }

		//Navigation Properties
		public List<CommentViewModel> Comments { get; set; }
        public UserViewModel User { get; set; }
    }
}
