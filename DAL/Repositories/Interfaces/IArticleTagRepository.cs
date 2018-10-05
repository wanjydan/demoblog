using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IArticleTagRepository: IRepository<ArticleTag>
    {
        Task<ArticleTag> GetArticleTag(int articleId, int tagId);
        Task<Tuple<bool, string>> DeleteArticleTag(ArticleTag articleTag);
    }
}
