using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.Helpers;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Friends;
using PostApp.Core.Application.ViewModels.Posts;
using PostApp.Core.Application.ViewModels.Users;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Services
{
	public class FriendService : IFriendService
	{
		private readonly IFriendRepository _friendRepository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AuthenticationResponse _userViewModel;
		private readonly IUserService _userService;
		private readonly IPostService _postService;

		public FriendService(IFriendRepository friendRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService, IPostService postService)
		{
			_friendRepository = friendRepository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
			_userService = userService;
			_postService = postService;
		}

		#region Add
		public async Task<SaveFriendViewModel> Add(SaveFriendViewModel vm)
		{
			var user = await _userService.FindFriend(vm.Username);

			if (user != null && user.UserName != _userViewModel.UserName)
			{
				List<Friend> friendList = await _friendRepository.GetAllAsync();

				if (!friendList.Exists(friend => friend.UserId == _userViewModel.Id && friend.FriendId == user.Id))
				{
					Friend friend = new()
					{
						UserId = _userViewModel.Id,
						FriendId = user.Id
					};
					await _friendRepository.AddAsync(friend);

					vm.HasError = false;
				}
                else
                {
					vm.HasError = true;
					vm.Error = $"User '{vm.Username}' is already your friend";
				}

            }
            else if(user != null && user.UserName == _userViewModel.UserName)
            {
				vm.HasError = true;
				vm.Error = $"You can not add yourself as a friend!";
			}
            else
            {
				vm.HasError = true;
				vm.Error = $"User '{vm.Username}' is not registered";
			}

			return vm;
		}
		#endregion

		#region Get
		public async Task<List<UserViewModel>> GetAllViewModel()
		{
			List<Friend> friendList = await _friendRepository.GetAllAsync();

			List<UserViewModel> users = new();

			friendList = friendList.Where(friend => friend.UserId == _userViewModel.Id).ToList();

            foreach (var friend in friendList)
            {
				users.Add(await _userService.GetById(friend.FriendId));
			}

			return users;
		}

		public async Task<List<PostViewModel>> GetAllFriendPost()
		{

			List<UserViewModel> friends = await GetAllViewModel();

			List<PostViewModel> posts = new();


            foreach (var friend in friends)
            {
				posts.AddRange(await _postService.GetAllFriendPost(friend.Id));
			}

			return posts.OrderByDescending(post => post.Created).ToList();
		}
		#endregion

		#region Delete
		public async Task Delete(string friendId)
		{
			Friend friend = await _friendRepository.GetByIdAsync(_userViewModel.Id, friendId);

			await _friendRepository.DeleteAsync(friend);
		}
		#endregion
	}
}
