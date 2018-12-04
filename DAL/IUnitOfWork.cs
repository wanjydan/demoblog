using DAL.Repositories.Interfaces;

namespace DAL
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IArticleRepository Articles { get; }
        ICategoryRepository Categories { get; }
        ITagRepository Tags { get; }
        ICommentRepository Comments { get; }
        IArticleTagRepository ArticleTags { get; }
        IArticleLikeRepository ArticleLikes { get; }


        int SaveChanges();
    }
}