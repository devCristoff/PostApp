using PostApp.Core.Application.ViewModels.Posts;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
		Task<List<PostViewModel>> GetAllViewModelWithInclude();
		Task<List<PostViewModel>> GetAllFriendPost(string friendId);
	}
}