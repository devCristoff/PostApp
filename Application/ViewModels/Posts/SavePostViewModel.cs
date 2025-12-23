using Microsoft.AspNetCore.Http;

namespace PostApp.Core.Application.ViewModels.Posts
{
    public class SavePostViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
		public string Message { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
		public IFormFile? File { get; set; }
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
    }
}
