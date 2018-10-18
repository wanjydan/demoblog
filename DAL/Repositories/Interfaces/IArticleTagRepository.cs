using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IArticleTagRepository: IRepository<ArticleTag>
    {
        Task<ArticleTag> GetArticleTag(Guid articleId, Guid tagId);
        ICollection<ArticleTag> GetArticleTags(Guid articleId);
        Task<Tuple<bool, string>> DeleteArticleTag(ArticleTag articleTag);
        Task<Tuple<bool, string>> DeleteArticleTags(ICollection<ArticleTag> articleTags);
    }
}
