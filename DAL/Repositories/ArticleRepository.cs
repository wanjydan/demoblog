using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    internal class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(DbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;


        public IQueryable<Article> GetArticles()
        {
            return _appContext.Articles
                .Include(a => a.ArticleLikes)
                .Include(a => a.CreatedBy)
                .Include(a => a.Category);
        }

        public async Task<Article> GetArticle(Guid id)
        {
            return await _appContext.Articles
                .Include(a => a.ArticleTags).ThenInclude(at => at.Tag)
                .Include(a => a.ArticleLikes)
                .Include(a => a.CreatedBy)
                .Include(a => a.Category)
                .Include(a => a.Comments)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Article> GetArticleBySlug(string slug)
        {
            return await _appContext.Articles
                .Include(a => a.ArticleTags).ThenInclude(at => at.Tag)
                .Include(a => a.ArticleLikes)
                .Include(a => a.CreatedBy)
                .Include(a => a.Category)
                .Include(a => a.Comments)
                .Where(a => a.Slug == slug)
                .FirstOrDefaultAsync();
        }

        public async Task<Tuple<bool, string>> UpdateArticle(Article article)
        {
            _appContext.Articles.Update(article);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> UpdateArticle(Article article, IEnumerable<Tag> tags)
        {
            _appContext.Articles.Update(article);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            /*ICollection<ArticleTag> articleTags = new List<ArticleTag>();
            foreach (var tag in tags)
            {
                articleTags.Add(new ArticleTag()
                {
                    Id = Guid.NewGuid(),
                    Article = article,
                    Tag = tag
                });
            }

            await RemoveTags(articleTags);*/

            await AddTags(article, tags);

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> CreateArticle(Article article, IEnumerable<Tag> tags)
        {
            await _appContext.Articles.AddAsync(article);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            await AddTags(article, tags);

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> DeleteArticle(Article article)
        {
            _appContext.Articles.Remove(article);


            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }


        public async Task<Tuple<bool, string>> AddTag(Article article, Tag tag)
        {
            await _appContext.ArticleTags.AddAsync(new ArticleTag
            {
                Article = article,
                Tag = tag
            });

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> RemoveTag(ArticleTag articleTag)
        {
            _appContext.ArticleTags.Remove(articleTag);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> AddTags(Article article, IEnumerable<Tag> tags)
        {
            ICollection<ArticleTag> articleTags = new List<ArticleTag>();
            foreach (var tag in tags)
                articleTags.Add(new ArticleTag
                {
                    Article = article,
                    Tag = tag
                });
            await _appContext.ArticleTags.AddRangeAsync(articleTags);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
//                _appContext.Articles.Remove(article);
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> RemoveTags(IEnumerable<ArticleTag> articleTags)
        {
            _appContext.ArticleTags.RemoveRange(articleTags);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> LikeArticle(Article article)
        {
            await _appContext.ArticleLikes.AddAsync(new ArticleLike
            {
                Article = article
            });

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> UnlikeArticle(ArticleLike articleLike)
        {
            _appContext.ArticleLikes.Remove(articleLike);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }
    }
}