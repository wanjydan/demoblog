using AutoMapper;
using DAL.Core;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoBlog.ViewModels;

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
                .ConvertUsing(s => Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));
            
            CreateMap<Article, ArticleViewModel>()
                .ForMember(d => d.Author, map => map.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.Tags, map => map.MapFrom(s => s.ArticleTags.Select(at => at.Tag)));

            CreateMap<ArticleCreateViewModel, Article>()
                .ForMember(d => d.Category, map => map.Ignore())
                .ForMember(d => d.ArticleTags, map => map.Ignore())
                .ForMember(d => d.Comments, map => map.Ignore())
                .ForMember(d => d.CreatedBy, map => map.Ignore())
                .ForMember(d => d.UpdatedBy, map => map.Ignore())
                .ForMember(d => d.UpdatedDate, map => map.Ignore())
                .ForMember(d => d.CreatedDate, map => map.Ignore());

            CreateMap<ArticleEditViewModel, Article>()
                .ForMember(d => d.Id, map => map.Ignore())
                .ForMember(d => d.Category, map => map.Ignore())
                .ForMember(d => d.ArticleTags, map => map.Ignore())
                .ForMember(d => d.Comments, map => map.Ignore())
                .ForMember(d => d.CreatedBy, map => map.Ignore())
                .ForMember(d => d.UpdatedBy, map => map.Ignore())
                .ForMember(d => d.UpdatedDate, map => map.Ignore())
                .ForMember(d => d.CreatedDate, map => map.Ignore());

            CreateMap<Tag, TagViewModel>()
                .ForMember(d => d.Articles, map => map.MapFrom(s => s.ArticleTags.Select(at => at.Article)));

            CreateMap<Article, ArticleListViewModel>()
                .ForMember(d => d.AuthorUserName, map => map.MapFrom(s => s.CreatedBy.UserName));

        }
    }
}
