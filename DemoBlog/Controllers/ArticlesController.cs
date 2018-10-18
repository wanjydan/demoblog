using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DemoBlog.ViewModels;
using AutoMapper;
using DAL.Core.Interfaces;
using DAL.Models;
using DemoBlog.Authorization;
using DemoBlog.Helpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation;
using DemoBlog.Helpers;

namespace DemoBlog.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountManager _accountManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IImageHandler _imageHandler;
        private const string GetArticleActionName = "GetArticle";

        public ArticlesController(IUnitOfWork unitOfWork, IAccountManager accountManager, IAuthorizationService authorizationService, IImageHandler imageHandler)
        {
            _unitOfWork = unitOfWork;
            _accountManager = accountManager;
            _authorizationService = authorizationService;
            _imageHandler = imageHandler;
        }

        // GET: api/Articles
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ArticleListViewModel>))]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _unitOfWork.Articles.GetArticles();
            var articleVM = Mapper.Map<IEnumerable<ArticleListViewModel>>(articles);
            return Ok(articleVM);
        }

        // GET: api/Articles/5
        [HttpGet("{idOrSlug}", Name = GetArticleActionName)]
        [ProducesResponseType(200, Type = typeof(ArticleViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetArticle([FromRoute] string idOrSlug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Article article;

            try
            {
                Guid id = Guid.Parse(idOrSlug);
                article = await _unitOfWork.Articles.GetArticle(id);
            }
            catch (Exception)
            {
                article = await _unitOfWork.Articles.GetArticleBySlug(idOrSlug);
            }

            if (article == null)
                return NotFound();

            var articleVM = Mapper.Map<ArticleViewModel>(article);
            return Ok(articleVM);
        }

        
        // PUT: api/Articles/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateArticle([FromRoute] Guid id, [FromBody] ArticleEditViewModel article)
        {
            ApplicationUser currentUser = await _accountManager.GetCurrentUser();
            Article appArticle = await _unitOfWork.Articles.GetArticle(id);

            if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy)).Succeeded)
                if(currentUser.Id != appArticle.CreatedBy.Id)
                    return Unauthorized();



            if (ModelState.IsValid)
            {
                if (article == null)
                    return BadRequest($"{nameof(article)} cannot be null");

                if (id != article.Id)
                    return BadRequest("Conflicting article id in parameter and model data");
                
                if (appArticle == null)
                    return NotFound(id);

                Category appCategory = await GetCategoryHelper(article.CategoryId);
                if (appCategory == null)
                    return NotFound("Category not found");

                appArticle.Slug = await GenarateUniqueSlug(SlugGenerator.GenerateSlug(article.Title));
                appArticle.Category = appCategory;

                var artTags = GetArticleTagsHelper(id);
                if (artTags != null)
                    await _unitOfWork.ArticleTags.DeleteArticleTags(artTags);
                
                ICollection<Tag> appTags = new List<Tag>();
                foreach (var tagId in article.TagIds)
                {
                    Tag appTag = await GetTagHelper(tagId);
                    if (appTag == null)
                        return NotFound("Tag " + tagId + " not found");
                    appTags.Add(appTag);
                }
                
                Mapper.Map(article, appArticle);

                var result = await _unitOfWork.Articles.UpdateArticle(appArticle, appTags);

                if (result.Item1)
                    return NoContent();

                ModelState.AddModelError(string.Empty, result.Item2);
            }
            return BadRequest(ModelState);
        }

        // POST: api/Articles
        [HttpPost]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(201, Type = typeof(ArticleViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateViewModel article)
        {
            if (ModelState.IsValid)
            {
                if (article == null)
                    return BadRequest($"{nameof(article)} cannot be null");


                Article appArticle = Mapper.Map<Article>(article);

                Category appCategory = await GetCategoryHelper(article.CategoryId);
                if (appCategory == null)
                    return NotFound("Category not found");

                appArticle.Category = appCategory;
                appArticle.Slug = await GenarateUniqueSlug(SlugGenerator.GenerateSlug(article.Title));
                appArticle.Image = "/images/featured/not-found.jpg";

                ICollection<Tag> appTags = new List<Tag>();
                foreach (var tagId in article.TagIds)
                {
                    Tag appTag = await GetTagHelper(tagId);
                    if (appTag == null)
                        return NotFound("Tag " + tagId + " not found");
                    appTags.Add(appTag);
                }

                var result = await _unitOfWork.Articles.CreateArticle(appArticle, appTags);

                if (result.Item1)
                {
                    ArticleViewModel articleVM = await GetArticleViewModelHelper(appArticle.Id);
                    return CreatedAtAction(GetArticleActionName, new { id = articleVM.Id }, articleVM);
                }

                ModelState.AddModelError(string.Empty, result.Item2);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(ArticleViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteArticle([FromRoute] Guid id)
        {
            ArticleViewModel articleVM = null;
            Article appArticle = await _unitOfWork.Articles.GetArticle(id);
            ApplicationUser currentUser = await _accountManager.GetCurrentUser();

            if (appArticle != null)
                articleVM = await GetArticleViewModelHelper(id);


            if (articleVM == null)
                return NotFound(id);
            
            if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy)).Succeeded)
                if (currentUser.Id != appArticle.CreatedBy.Id)
                    return Unauthorized();

            var result = await _unitOfWork.Articles.DeleteArticle(appArticle);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst deleting article: " +
                                    string.Join(", ", result.Item2));


            return Ok(articleVM);
        }

        
        // POST: api/Articles/5/Likes
        [HttpPost("{id}/Likes")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(ArticleViewModel))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LikeArticle([FromRoute] Guid id)
        {
            ArticleViewModel articleVM = null;
            Article appArticle = await _unitOfWork.Articles.GetArticle(id);
            ApplicationUser currentUser = await _accountManager.GetCurrentUser();

            if (appArticle != null)
                articleVM = await GetArticleViewModelHelper(id);

            if (articleVM == null)
                return NotFound(id);

            var articleLike = await GetArticleLikeHelper(appArticle.Id, currentUser.Id);
            if (articleLike != null)
                return BadRequest("You have already liked article " + appArticle.Id);

            var result = await _unitOfWork.Articles.LikeArticle(appArticle);

            if (result.Item1)
                return CreatedAtAction(GetArticleActionName, new { id = articleVM.Id }, articleVM);

            ModelState.AddModelError(string.Empty, result.Item2);

            return BadRequest(ModelState);
        }

        // DELETE: api/Articles/5/Likes
        [HttpDelete("{id}/Likes")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(200, Type = typeof(ArticleViewModel))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UnlikeArticle([FromRoute] Guid id)
        {
            ArticleViewModel articleVM = null;
            Article appArticle = await _unitOfWork.Articles.GetArticle(id);
            ApplicationUser currentUser = await _accountManager.GetCurrentUser();

            if (appArticle != null)
                articleVM = await GetArticleViewModelHelper(id);

            if (articleVM == null)
                return NotFound(id);

            var articleLike = await GetArticleLikeHelper(appArticle.Id, currentUser.Id);
            if (articleLike == null)
                return BadRequest("You have not liked article " + appArticle.Id);

            var result = await _unitOfWork.Articles.UnlikeArticle(articleLike);

            if (!result.Item1)
                throw new Exception("The following errors occurred whilst unliking the article: " +
                                    string.Join(", ", result.Item2));


            return Ok(articleVM);
        }


        // POST: api/Articles/5/Images
        [HttpPost("{id}/Images")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
        [ProducesResponseType(201, Type = typeof(ArticleViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UploadImage([FromRoute] Guid id, IFormFile image)
        {
            ArticleViewModel articleVM = null;
            ApplicationUser currentUser = await _accountManager.GetCurrentUser();
            Article appArticle = await _unitOfWork.Articles.GetArticle(id);

            if (image == null || image.Length == 0)
                return BadRequest("image cannot be null");

            if (appArticle != null)
                articleVM = await GetArticleViewModelHelper(id);


            if (articleVM == null)
                return NotFound(id);

            if (!(await _authorizationService.AuthorizeAsync(User, currentUser.Id, Policies.ManageAllArticlesPolicy)).Succeeded)
                if (currentUser.Id != appArticle.CreatedBy.Id)
                    return Unauthorized();

            var result = await _imageHandler.UploadImage(image, appArticle.Slug);

            if (result.Item1)
            {
                appArticle.Image = $"/images/featured/{result.Item2}";
                result = await _unitOfWork.Articles.UpdateArticle(appArticle);

                if (result.Item1)
                    return CreatedAtAction(GetArticleActionName, new { id = articleVM.Id }, articleVM);

                ModelState.AddModelError(string.Empty, result.Item2);
            }
            else
                ModelState.AddModelError(string.Empty, result.Item2);

            return BadRequest(ModelState);
        }

        private async Task<ArticleViewModel> GetArticleViewModelHelper(Guid id)
        {
            var article = await _unitOfWork.Articles.GetArticle(id);
            if (article != null)
                return Mapper.Map<ArticleViewModel>(article);


            return null;
        }

        private async Task<Article> GetArticleHelper(Guid id)
        {
            var article = await _unitOfWork.Articles.GetArticle(id);
            if (article != null)
                return article;


            return null;
        }

        private async Task<Category> GetCategoryHelper(Guid id)
        {
            var category = await _unitOfWork.Categories.GetCategory(id);
            if (category != null)
                return category;


            return null;
        }

        private async Task<Tag> GetTagHelper(Guid id)
        {
            var tag = await _unitOfWork.Tags.GetTag(id);
            if (tag != null)
                return tag;
            
            return null;
        }

        private async Task<ArticleTag> GetArticleTagHelper(Guid articleId, Guid tagId)
        {
            var articleTag = await _unitOfWork.ArticleTags.GetArticleTag(articleId, tagId);
            if (articleTag != null)
                return articleTag;

            return null;
        }
        
        private async Task<ArticleLike> GetArticleLikeHelper(Guid articleId, string userId)
        {
            var articleLike = await _unitOfWork.ArticleLikes.GetArticleLike(articleId, userId);
            if (articleLike != null)
                return articleLike;

            return null;
        }

        private ICollection<ArticleTag> GetArticleTagsHelper(Guid articleId)
        {
            var articleTags = _unitOfWork.ArticleTags.GetArticleTags(articleId);
            if (articleTags != null)
                return articleTags;

            return null;
        }

        private async Task<string> GenarateUniqueSlug(string slug)
        {
            var article = await _unitOfWork.Articles.GetArticleBySlug(slug);
            if (article == null)
                return slug;

            int i = 1;
            string slugWithNumeric = $"{slug}-{i}";

            while (true)
            {
                article = await _unitOfWork.Articles.GetArticleBySlug(slugWithNumeric);
                if (article == null)
                    return slugWithNumeric;

                i++;
            }
        }
    }
}
