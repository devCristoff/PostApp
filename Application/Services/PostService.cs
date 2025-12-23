using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostApp.Core.Application.Helpers;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Posts;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
		private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICommentService commentService, IUserService userService) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _commentService = commentService;
			_userService = userService;
		}
		#region Add
		public override async Task Add(SavePostViewModel vm)
		{
            if (vm.File != null)
            {
				vm.ImageUrl = await FileHelper.UploadImage(vm.File, $"{_userViewModel.UserName}/Post");
            }

            if (vm.VideoUrl != null)
            {
				vm.VideoUrl = vm.VideoUrl.Contains("/watch?v=") ? vm.VideoUrl.Replace("/watch?v=", "/") : vm.VideoUrl;
            }

			await base.Add(vm);
		}
		#endregion

		#region Get
		public override async Task<SavePostViewModel> GetByIdSaveViewModel(int id)
        {
            SavePostViewModel vm = await base.GetByIdSaveViewModel(id);

            if (vm.VideoUrl != null)
            {
                vm.VideoUrl = vm.VideoUrl.Contains("/embed/") ? vm.VideoUrl : vm.VideoUrl.Replace("youtube.com/", "youtube.com/embed/");
            }

            return vm;
        }

		public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
		{
			var postList = await _postRepository.GetAllAsync();

			List<PostViewModel> postVmList = postList.Where(post => post.UserId == _userViewModel.Id).Select(post => new PostViewModel
			{
				Id = post.Id,
				Message = post.Message,
				VideoUrl = post.VideoUrl,
				ImageUrl = post.ImageUrl,
				Created = post.Created,
				UserId = post.UserId
			}).ToList();

			foreach (var item in postVmList)
			{
				if (item.VideoUrl != null)
				{
					item.VideoUrl = item.VideoUrl.Contains("/embed/") ? item.VideoUrl : item.VideoUrl.Replace("youtube.com/", "youtube.com/embed/");
				}
				item.User = await _userService.GetById(item.UserId);
				item.Comments = await _commentService.GetCommentByPost(item.Id);
			}

			return postVmList.OrderByDescending(post => post.Created).ToList();
			//post.
		}

		public async Task<List<PostViewModel>> GetAllFriendPost(string friendId)
		{
			var postList = await _postRepository.GetAllAsync();

			List<PostViewModel> postVmList = postList.Where(post => post.UserId == friendId).Select(post => new PostViewModel
			{
				Id = post.Id,
				Message = post.Message,
				VideoUrl = post.VideoUrl,
				ImageUrl = post.ImageUrl,
				Created = post.Created,
				UserId = post.UserId,
			}).ToList();

			foreach (var item in postVmList)
			{
				if (item.VideoUrl != null)
				{
					item.VideoUrl = item.VideoUrl.Contains("/embed/") ? item.VideoUrl : item.VideoUrl.Replace("youtube.com/", "youtube.com/embed/");
				}
				
				item.User = await _userService.GetById(item.UserId);
				item.Comments = await _commentService.GetCommentByPost(item.Id);
			}

			return postVmList.OrderByDescending(post => post.Created).ToList();
			//post.
		}
		#endregion

		#region Update
		public override async Task Update(SavePostViewModel vm, int id)
        {
            if (vm.ImageUrl != null)
            {
				vm.ImageUrl = await FileHelper.UploadImage(vm.File, $"{_userViewModel.UserName}/Post", true, vm.ImageUrl);
			}

            if (vm.VideoUrl != null)
            {
                vm.VideoUrl = vm.VideoUrl.Contains("/watch?v=") ? vm.VideoUrl.Replace("/watch?v=", "/") : vm.VideoUrl;
            }

            await base.Update(vm, id);
        }
		#endregion

		#region Delete
		public override async Task Delete(int id)
		{
			var post = await _postRepository.GetByIdAsync(id);

			if (post.ImageUrl != null)
			{
				await FileHelper.DeleteImage(post.ImageUrl);
			}

			await base.Delete(id);
		}
		#endregion

	}
}
