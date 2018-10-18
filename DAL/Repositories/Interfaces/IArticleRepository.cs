using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IArticleRepository: IRepository<Article>
    {
        Task<IEnumerable<Article>> GetArticles();
        Task<Article> GetArticle(Guid id);
        Task<Article> GetArticleBySlug(string slug);
        Task<Tuple<bool, string>> UpdateArticle(Article article);
        Task<Tuple<bool, string>> UpdateArticle(Article article, ICollection<Tag> tags);
        Task<Tuple<bool, string>> CreateArticle(Article article, ICollection<Tag> tags);
        Task<Tuple<bool, string>> DeleteArticle(Article article);
        Task<Tuple<bool, string>> AddTag(Article article, Tag tag);
        Task<Tuple<bool, string>> RemoveTag(ArticleTag articleTag);
        Task<Tuple<bool, string>> AddTags(Article article, ICollection<Tag> tags);
        Task<Tuple<bool, string>> RemoveTags(ICollection<ArticleTag> articleTags);
        Task<Tuple<bool, string>> LikeArticle(Article article);
        Task<Tuple<bool, string>> UnlikeArticle(ArticleLike articleLike);
    }
}
