using System;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IArticleLikeRepository : IRepository<ArticleLike>
    {
        Task<ArticleLike> GetArticleLike(Guid articleId, string userId);
        Task<Tuple<bool, string>> DeleteArticleLike(ArticleLike articleLike);
    }
}