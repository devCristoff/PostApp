namespace PostApp.Core.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int Parent { get; set; }
        public string UserId { get; set; }
		public DateTime Created { get; set; }

		//Navigation Properties
		public Post Post { get; set; }
    }
}
