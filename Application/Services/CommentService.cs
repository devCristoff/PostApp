using AutoMapper;
using Microsoft.AspNetCore.Http;
using PostApp.Core.Application.Interfaces.Repositories;
using PostApp.Core.Application.Interfaces.Services;
using PostApp.Core.Application.ViewModels.Comments;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _userService = userService;
        }

		public async Task Add(SaveCommentViewModel vm)
		{
			Comment comment = _mapper.Map<Comment>(vm);

			await _commentRepository.AddAsync(comment);
		}

		public async Task<List<CommentViewModel>> GetCommentByPost(int idPost)
        {
            var commentList = await _commentRepository.GetAllAsync();

			List<CommentViewModel> commentVmList = commentList.Where(comment => comment.PostId == idPost).Select(comment => new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                Parent = comment.Parent,
                UserId = comment.UserId,
                Created = comment.Created
            }).ToList();

            //commentVmList.ForEach(async comment => comment.User = await _userService.GetById(comment.UserId));

            foreach (var item in commentVmList)
            {
                item.User = await _userService.GetById(item.UserId);
            }

            return commentVmList; 
		}
    }
}
