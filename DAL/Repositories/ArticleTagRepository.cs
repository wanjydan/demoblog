using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    class ArticleTagRepository: Repository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(DbContext context) : base(context)
        { }



        public async Task<ArticleTag> GetArticleTag(int articleId, int tagId)
        {
            return await _appContext.ArticleTags
                .Where(at => at.ArticleId == articleId && at.TagId == tagId)
                .FirstOrDefaultAsync();
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


        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
