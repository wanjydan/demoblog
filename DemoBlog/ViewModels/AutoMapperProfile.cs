using System.Linq;
using AutoMapper;
using DAL.Core;
using DAL.Models;
using DemoBlog.ViewModels.ArticleViewModels;
using DemoBlog.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;

namespace DemoBlog.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserViewModel, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEditViewModel, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<ApplicationUser, UserPatchViewModel>()
                .ReverseMap();

            CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.ResolveUsing(s => s.Users?.Count ?? 0))
                .ReverseMap();
            CreateMap<RoleViewModel, ApplicationRole>();

            CreateMap<IdentityRoleClaim<string>, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionViewModel>()
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionViewModel>()
                .ConvertUsing(s =>
                    Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));

            CreateMap<Article, ArticleViewModel>()
                .ForMember(d => d.Likes, map => map.MapFrom(s => s.ArticleLikes.Count))
                .ForMember(d => d.Author, map => map.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.Tags, map => map.MapFrom(s => s.ArticleTags.Select(at => at.Tag)));

            CreateMap<Article, ArticlePatchViewModel>()
                .ReverseMap();

            CreateMap<ArticleCreateViewModel, Article>()
                .ReverseMap();

            CreateMap<ArticleEditViewModel, Article>()
                .ReverseMap();

            /*CreateMap<Tag, TagViewModel>()
                .ForMember(d => d.Articles, map => map.MapFrom(s => s.ArticleTags.Select(at => at.Article)));*/

            CreateMap<Comment, CommentViewModel>()
                .ForMember(d => d.Author, map => map.MapFrom(s => s.CreatedBy));
        }
    }
}