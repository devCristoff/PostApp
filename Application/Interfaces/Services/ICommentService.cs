using PostApp.Core.Application.Interfaces.Services.CRUD;
using PostApp.Core.Application.ViewModels.Comments;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface ICommentService : ICreate<SaveCommentViewModel>
    {
		Task<List<CommentViewModel>> GetCommentByPost(int idPost);
	}
}