using AutoMapper;
using PostApp.Core.Application.DTOs.Account;
using PostApp.Core.Application.ViewModels.Comments;
using PostApp.Core.Application.ViewModels.Friends;
using PostApp.Core.Application.ViewModels.Posts;
using PostApp.Core.Application.ViewModels.Users;
using PostApp.Core.Domain.Entities;

namespace PostApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(dest => dest.HasError, option => option.Ignore())
                .ForMember(dest => dest.Error, option => option.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, RegisterUserViewModel>()
                .ForMember(dest => dest.File, option => option.Ignore())
                .ForMember(dest => dest.Error, option => option.Ignore())
                .ForMember(dest => dest.HasError, option => option.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(dest => dest.HasError, option => option.Ignore())
                .ForMember(dest => dest.Error, option => option.Ignore())
                .ReverseMap();

            CreateMap<UpdateProfileRequest, UpdateUserViewModel>()
                .ForMember(dest => dest.UserName, option => option.Ignore())
                .ForMember(dest => dest.File, option => option.Ignore())
                .ReverseMap();

			CreateMap<UserProfile, UpdateUserViewModel>()
				.ForMember(dest => dest.File, option => option.Ignore())
				.ForMember(dest => dest.HasError, option => option.Ignore())
				.ForMember(dest => dest.Error, option => option.Ignore())
				.ForMember(dest => dest.Password, option => option.Ignore())
				.ForMember(dest => dest.ConfirmPassword, option => option.Ignore())
				.ReverseMap()
				.ForMember(origin => origin.Id, option => option.Ignore());

			CreateMap<UserProfile, UserViewModel>()
                .ReverseMap();
			#endregion

			#region Post
			CreateMap<Post, PostViewModel>()
				.ForMember(dest => dest.User, option => option.Ignore())
				.ForMember(dest => dest.Comments, option => option.Ignore())
				.ReverseMap()
				.ForMember(origin => origin.Comments, option => option.Ignore())
				.ForMember(origin => origin.CreatedBy, option => option.Ignore())
				.ForMember(origin => origin.LastModified, option => option.Ignore())
				.ForMember(origin => origin.LastModifiedBy, option => option.Ignore());

			CreateMap<Post, SavePostViewModel>()
				.ForMember(dest => dest.File, option => option.Ignore())
                .ReverseMap()
				.ForMember(origin => origin.Comments, option => option.Ignore())
				.ForMember(origin => origin.LastModified, option => option.Ignore())
				.ForMember(origin => origin.LastModifiedBy, option => option.Ignore());
			#endregion

			#region Comment
			CreateMap<Comment, CommentViewModel>()
				.ForMember(dest => dest.User, option => option.Ignore())
				.ReverseMap()
				.ForMember(origin => origin.Post, option => option.Ignore());

			CreateMap<Comment, SaveCommentViewModel>()
                .ReverseMap()
                .ForMember(origin => origin.Post, option => option.Ignore());
            #endregion

            #region Friend
            CreateMap<Friend, FriendViewModel>()
                .ReverseMap();
			#endregion
		}
	}
}
