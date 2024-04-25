using PostApp.Core.Application.ViewModels.Friends;
using PostApp.Core.Application.ViewModels.Posts;
using PostApp.Core.Application.ViewModels.Users;

namespace PostApp.Core.Application.Interfaces.Services
{
    public interface IFriendService
	{
		Task<SaveFriendViewModel> Add(SaveFriendViewModel viewModel);
		Task<List<UserViewModel>> GetAllViewModel();
		Task<List<PostViewModel>> GetAllFriendPost();
		Task Delete(string friendId);
	}
}
