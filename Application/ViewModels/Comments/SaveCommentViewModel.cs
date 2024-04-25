namespace PostApp.Core.Application.ViewModels.Comments
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int Parent { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}