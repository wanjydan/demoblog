using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    internal class ArticleLikeRepository : Repository<ArticleLike>, IArticleLikeRepository
    {
        public ArticleLikeRepository(DbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;


        public async Task<ArticleLike> GetArticleLike(Guid articleId, string userId)
        {
            return await _appContext.ArticleLikes
                .Where(al => al.ArticleId == articleId && al.CreatedById == userId)
                .FirstOrDefaultAsync();
        }


        public async Task<Tuple<bool, string>> DeleteArticleLike(ArticleLike articleLike)
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