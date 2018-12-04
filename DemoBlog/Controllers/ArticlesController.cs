using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Core.Interfaces;
using DAL.Models;
using DemoBlog.Authorization;
using DemoBlog.Extensions;
using DemoBlog.Helpers;
using DemoBlog.Helpers.QueryParameters;
using DemoBlog.Services.Interfaces;
using DemoBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenIddict.Validation;

namespace DemoBlog.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private const string GetArticleActionName = "GetArticle";
        private const string GetArticlesActionName = "GetArticles";
        private readonly IAccountManager _accountManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFileHandler _fileHandler;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ITypeHelperService _typeHelperService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlHelper _urlHelper;

        public ArticlesController(IUnitOfWork unitOfWork, IAccountManager accountManager,
            IAuthorizationService authorizationService, IFileHandler fileHandler, IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService, ITypeHelperService typeHelperService)
        {
            _unitOfWork = unitOfWork;
            _accountManager = accountManager;
            _authorizationService = authorizationService;
            _fileHandler = fileHandler;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        // GET: api/Articles
        [HttpGet(Name = GetArticlesActionName)]
        [HttpHead]
        public IActionResult GetArticles([FromQuery] ArticleQueryParameters queryParameters)
        {
            var result =
                _propertyMappingService.ValidMappingExistsFor<ArticleViewModel, Article>(queryParameters.OrderBy);
            if (!result.Item1)
                foreach (var invalidField in result.Item2)
                    AddError("orderBy", $"{invalidField} is not a valid orderBy parameter");

            result = _typeHelperService.TypeHasProperties<ArticleViewModel>(queryParameters.Fields);
            if (!result.Item1)
                foreach (var invalidField in result.Item2)
                    AddError("fields", $"{invalidField} is not a valid fields parameter");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articles = _unitOfWork.Articles.GetArticles();

            articles = articles.ApplySort(queryParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<ArticleViewModel, Article>());

            if (!string.IsNullOrWhiteSpace(queryParameters.Category))
            {
                var categorySlug = queryParameters.Category.Trim().ToLower();
                articles = articles.Where(a => a.Category.Slug == categorySlug);
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Search))
            {
                var searchQuery = queryParameters.Search.Trim().ToLowerInvariant();
                articles = articles.Where(a => a.Title.ToLowerInvariant().Contains(searchQuery)
                                               || a.Body.ToLowerInvariant().Contains(searchQuery));
            }

            var pagedArticles =
                PagedList<Article>.Create(articles, queryParameters.PageNumber, queryParameters.PageSize);

            var previousPageLink = pagedArticles.HasPrevious
                ? CreateResourceUri(queryParameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = pagedArticles.HasNext
                ? CreateResourceUri(queryParameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = pagedArticles.TotalCount,
                pageSize = pagedArticles.PageSize,
                currentPage = pagedArticles.CurrentPage,
                totalPages = pagedArticles.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var articleVm = Mapper.Map<IEnumerable<ArticleViewModel>>(pagedArticles);
            return Ok(articleVm.ShapeData(queryParameters.Fields));
        }

        // GET: api/Articles/5
        [HttpGet("{id}", Name = GetArticleActionName)]
        public async Task<IActionResult> GetArticle([FromRoute] string id, [FromQuery] string fields)
        {
            if (!string.IsNullOrWhiteSpace(fields))
            {
                var result = _typeHelperService.TypeHasProperties<ArticleViewModel>(fields);
                if (!result.Item1)
                    foreach (var invalidField in result.Item2)
                        AddError("fields", $"{invalidField} is not a valid fields parameter");
            }
            else
            {
                fields = "id,title,slug,image,author";
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Article article;

            if (IsValidGuid(id))
                article = await _unitOfWork.Articles.GetArticle(Guid.Parse(id));
            else
                article = await _unitOfWork.Articles.GetArticleBySlug(id);

            if (article == null)
                return NotFound();

            var articleVm = Mapper.Map<ArticleViewModel>(article);
            return Ok(articleVm.ShapeData(fields));
        }


        // POST: api/Articles
        [HttpPost]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateViewModel article)
        {
            if (ModelState.IsValid)
            {
                if (article == null)
                    return BadRequest($"{nameof(article)} cannot be null");


                var appArticle = Mapper.Map<Article>(article);

                var appCategory = await _unitOfWork.Categories.GetCategory(Guid.Parse(article.CategoryId));
                if (appCategory == null)
                    return NotFound("Category not found");

                appArticle.Category = appCategory;
                appArticle.Slug = await GenarateUniqueSlug(SlugGenerator.GenerateSlug(article.Title));
                appArticle.Image = "/images/featured/not-found.jpg";

                ICollection<Tag> appTags = new List<Tag>();
                foreach (var tagId in article.TagIds)
                    if (IsValidGuid(tagId))
                    {
                        var appTag = await _unitOfWork.Tags.GetTag(Guid.Parse(tagId));
                        if (appTag == null)
                            AddError("TagIds", $"TagId: {tagId} not found!");

                        if (!appTags.Contains(appTag))
                            appTags.Add(appTag);
                    }
                    else
                    {
                        AddError("TagIds", $"TagId: {tagId} is not a valid guid");
                    }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _unitOfWork.Articles.CreateArticle(appArticle, appTags);

                if (result.Item1)
                {
                    var articleVm = await GetArticleViewModelHelper(appArticle.Id);
                    return CreatedAtAction(GetArticleActionName, new {id = articleVm.Id}, articleVm);
                }

                AddError("error", result.Item2);
            }

            return BadRequest(ModelState);
        }


        // PUT: api/Articles/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateArticle([FromRoute] string id, [FromBody] ArticleEditViewModel article)
        {
            if (!IsValidGuid(id))
                return BadRequest("Id in the route is not a valid guid!");

            if (ModelState.IsValid)
            {
                var articleId = Guid.Parse(id);

                var appArticle = await _unitOfWork.Articles.GetArticle(articleId);
                if (appArticle == null)
                    return NotFound(articleId);

                var currentUser = await _accountManager.GetCurrentUserAsync();

                if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy)
                    ).Succeeded)
                    if (currentUser.Id != appArticle.CreatedBy.Id)
                        return StatusCode(403, "Permission to edit this article denied!");

                if (article == null)
                    return BadRequest($"{nameof(article)} cannot be null");

                if (articleId != Guid.Parse(article.Id))
                    return BadRequest("Conflicting article id in parameter and model data");

                var appCategory = await _unitOfWork.Categories.GetCategory(Guid.Parse(article.CategoryId));
                if (appCategory == null)
                    return NotFound("Category not found");

                appArticle.Slug = await GenarateUniqueSlug(SlugGenerator.GenerateSlug(article.Title));
                appArticle.Category = appCategory;

                var artTags = _unitOfWork.ArticleTags.GetArticleTags(articleId);
                if (artTags != null)
                    await _unitOfWork.ArticleTags.DeleteArticleTags(artTags);

                ICollection<Tag> appTags = new List<Tag>();
                foreach (var tagId in article.TagIds)
                    if (IsValidGuid(tagId))
                    {
                        var appTag = await _unitOfWork.Tags.GetTag(Guid.Parse(tagId));
                        if (appTag == null)
                            AddError("TagIds", $"TagId: {tagId} not found!");

                        if (!appTags.Contains(appTag))
                            appTags.Add(appTag);
                    }
                    else
                    {
                        AddError("TagIds", $"TagId: {tagId} is not a valid guid");
                    }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Mapper.Map(article, appArticle);

                var result = await _unitOfWork.Articles.UpdateArticle(appArticle, appTags);

                if (result.Item1)
                    return NoContent();

                AddError("error", result.Item2);
            }

            return BadRequest(ModelState);
        }


        // PATCH: api/Articles/5
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateArticle([FromRoute] string id,
            [FromBody] JsonPatchDocument<ArticlePatchViewModel> patch)
        {
            if (!IsValidGuid(id))
                return BadRequest("Id in the route is not a valid guid!");

            if (ModelState.IsValid)
            {
                var articleId = Guid.Parse(id);

                var appArticle = await _unitOfWork.Articles.GetArticle(articleId);
                if (appArticle == null)
                    return NotFound(articleId);

                var currentUser = await _accountManager.GetCurrentUserAsync();

                if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy)
                    ).Succeeded)
                    if (currentUser.Id != appArticle.CreatedBy.Id)
                        return StatusCode(403, "Permission to edit this article denied!");

                if (patch == null)
                    return BadRequest($"{nameof(patch)} cannot be null");

                var articlePVM = Mapper.Map<ArticlePatchViewModel>(appArticle);
                patch.ApplyTo(articlePVM, ModelState);

                if (ModelState.IsValid)
                {
                    Mapper.Map(articlePVM, appArticle);

                    var result = await _unitOfWork.Articles.UpdateArticle(appArticle);
                    if (result.Item1)
                        return NoContent();

                    AddError("error", result.Item2);
                }
            }

            return BadRequest(ModelState);
        }


        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteArticle([FromRoute] string id)
        {
            if (!IsValidGuid(id))
                return BadRequest("Id in the route is not a valid guid!");

            var articleId = Guid.Parse(id);

            var appArticle = await _unitOfWork.Articles.GetArticle(articleId);
            if (appArticle == null)
                return NotFound(articleId);

            var currentUser = await _accountManager.GetCurrentUserAsync();

            if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy))
                .Succeeded)
                if (currentUser.Id != appArticle.CreatedBy.Id)
                    return Unauthorized();

            var result = await _unitOfWork.Articles.DeleteArticle(appArticle);
            if (!result.Item1)
                AddError("error", result.Item2);

            return NoContent();
        }


        // POST: api/Articles/5/Likes
        [HttpPost("{id}/Likes")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LikeArticle([FromRoute] string id)
        {
            if (!IsValidGuid(id))
                return BadRequest("Id in the route is not a valid guid!");

            var articleId = Guid.Parse(id);

            var appArticle = await _unitOfWork.Articles.GetArticle(articleId);
            if (appArticle == null)
                return NotFound(articleId);

            var currentUser = await _accountManager.GetCurrentUserAsync();

            var articleLike = await _unitOfWork.ArticleLikes.GetArticleLike(appArticle.Id, currentUser.Id);
            if (articleLike != null)
                return BadRequest("You have already liked article this article");

            var result = await _unitOfWork.Articles.LikeArticle(appArticle);

            if (result.Item1)
            {
                var articleVm = await GetArticleViewModelHelper(articleId);
                return CreatedAtAction(GetArticleActionName, new {id = articleVm.Id}, articleVm);
            }

            AddError("error", result.Item2);

            return BadRequest(ModelState);
        }

        // DELETE: api/Articles/5/Likes
        [HttpDelete("{id}/Likes")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UnlikeArticle([FromRoute] string id)
        {
            if (!IsValidGuid(id))
                return BadRequest("Id in the route is not a valid guid!");

            var articleId = Guid.Parse(id);

            var appArticle = await _unitOfWork.Articles.GetArticle(articleId);
            if (appArticle == null)
                return NotFound(articleId);

            var currentUser = await _accountManager.GetCurrentUserAsync();

            var articleLike = await _unitOfWork.ArticleLikes.GetArticleLike(appArticle.Id, currentUser.Id);
            if (articleLike == null)
                return BadRequest("You have not liked article this article");

            var result = await _unitOfWork.Articles.UnlikeArticle(articleLike);

            if (!result.Item1)
                AddError("error", result.Item2);

            return NoContent();
        }


        // POST: api/Articles/5/Images
        [HttpPost("{id}/Images")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadImage([FromRoute] string id, [FromForm] IFormFile image)
        {
            if (!IsValidGuid(id))
                return BadRequest("Id in the route is not a valid guid!");

            var articleId = Guid.Parse(id);

            var appArticle = await _unitOfWork.Articles.GetArticle(articleId);
            if (appArticle == null)
                return NotFound(articleId);

            var currentUser = await _accountManager.GetCurrentUserAsync();

            if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy))
                .Succeeded)
                if (currentUser.Id != appArticle.CreatedBy.Id)
                    return Unauthorized();

            if (image == null || image.Length == 0)
                return BadRequest("image cannot be null");

            var result = await _fileHandler.UploadFile(image, appArticle.Slug);

            if (result.Item1)
            {
                appArticle.Image = $"/images/featured/{result.Item2}";
                result = await _unitOfWork.Articles.UpdateArticle(appArticle);

                if (result.Item1)
                    return NoContent();

                AddError("image", result.Item2);
            }
            else
            {
                AddError("image", result.Item2);
            }

            return BadRequest(ModelState);
        }

        private async Task<ArticleViewModel> GetArticleViewModelHelper(Guid id)
        {
            var article = await _unitOfWork.Articles.GetArticle(id);

            return Mapper.Map<ArticleViewModel>(article);
        }


        private async Task<string> GenarateUniqueSlug(string slug)
        {
            var article = await _unitOfWork.Articles.GetArticleBySlug(slug);
            if (article == null)
                return slug;

            var i = 1;
            var slugWithNumeric = $"{slug}-{i}";

            while (true)
            {
                article = await _unitOfWork.Articles.GetArticleBySlug(slugWithNumeric);
                if (article == null)
                    return slugWithNumeric;

                i++;
                slugWithNumeric = $"{slug}-{i}";
            }
        }

        private void AddError(string key, string message)
        {
            ModelState.AddModelError(key, message);
        }

        private static bool IsValidGuid(string id)
        {
            try
            {
                Guid.Parse(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string CreateResourceUri(ArticleQueryParameters queryParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link(GetArticlesActionName,
                        new
                        {
                            category = queryParameters.Category,
                            fields = queryParameters.Fields,
                            search = queryParameters.Search,
                            orderBy = queryParameters.OrderBy,
                            pageNumber = queryParameters.PageNumber - 1,
                            pageSize = queryParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link(GetArticlesActionName,
                        new
                        {
                            category = queryParameters.Category,
                            fields = queryParameters.Fields,
                            search = queryParameters.Search,
                            orderBy = queryParameters.OrderBy,
                            pageNumber = queryParameters.PageNumber + 1,
                            pageSize = queryParameters.PageSize
                        });
                default:
                    return _urlHelper.Link(GetArticlesActionName,
                        new
                        {
                            category = queryParameters.Category,
                            fields = queryParameters.Fields,
                            search = queryParameters.Search,
                            orderBy = queryParameters.OrderBy,
                            pageNumber = queryParameters.PageNumber,
                            pageSize = queryParameters.PageSize
                        });
            }
        }
    }
}