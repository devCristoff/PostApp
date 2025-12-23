using PostApp.Core.Domain.Common;

namespace PostApp.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }

        //Navigation Properties
        public ICollection<Comment> Comments { get; set; }
    }
}
