using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IArticleLikeRepository _articleLikes;
        private IArticleRepository _articles;
        private IArticleTagRepository _articleTags;
        private ICategoryRepository _categories;
        private ICommentRepository _comments;
        private ITagRepository _tags;

        private IUserRepository _users;


        protected UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public IUserRepository Users => _users ?? (_users = new UserRepository(_context));

        public IArticleRepository Articles => _articles ?? (_articles = new ArticleRepository(_context));

        public ICategoryRepository Categories => _categories ?? (_categories = new CategoryRepository(_context));

        public ITagRepository Tags => _tags ?? (_tags = new TagRepository(_context));

        public ICommentRepository Comments => _comments ?? (_comments = new CommentRepository(_context));

        public IArticleTagRepository ArticleTags => _articleTags ?? (_articleTags = new ArticleTagRepository(_context));

        public IArticleLikeRepository ArticleLikes =>
            _articleLikes ?? (_articleLikes = new ArticleLikeRepository(_context));


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}