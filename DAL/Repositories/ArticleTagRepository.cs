using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    internal class ArticleTagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(DbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;


        public async Task<ArticleTag> GetArticleTag(Guid articleId, Guid tagId)
        {
            return await _appContext.ArticleTags
                .Where(at => at.ArticleId == articleId && at.TagId == tagId)
                .FirstOrDefaultAsync();
        }

        public ICollection<ArticleTag> GetArticleTags(Guid articleId)
        {
            return _appContext.ArticleTags
                .Where(at => at.ArticleId == articleId)
                .ToList();
        }


        public async Task<Tuple<bool, string>> DeleteArticleTag(ArticleTag articleTag)
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

        public async Task<Tuple<bool, string>> DeleteArticleTags(IEnumerable<ArticleTag> articleTags)
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
    }
}