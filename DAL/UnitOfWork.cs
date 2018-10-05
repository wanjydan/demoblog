using System;
using System.Collections.Generic;
using System.Text;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IUserRepository _users;
        private IArticleRepository _articles;
        private ICategoryRepository _categories;
        private ITagRepository _tags;
        private ICommentRepository _comments;
        private IArticleTagRepository _articleTags;
        private IArticleLikeRepository _articleLikes;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }



        public IUserRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(_context);

                return _users;
            }
        }



        public IArticleRepository Articles
        {
            get
            {
                if (_articles == null)
                    _articles = new ArticleRepository(_context);

                return _articles;
            }
        }



        public ICategoryRepository Categories
        {
            get
            {
                if (_categories == null)
                    _categories = new CategoryRepository(_context);

                return _categories;
            }
        }

        public ITagRepository Tags
        {
            get
            {
                if (_tags == null)
                    _tags = new TagRepository(_context);

                return _tags;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                if (_comments == null)
                    _comments = new CommentRepository(_context);

                return _comments;
            }
        }

        public IArticleTagRepository ArticleTags
        {
            get
            {
                if (_articleTags == null)
                    _articleTags = new ArticleTagRepository(_context);

                return _articleTags;
            }
        }

        public IArticleLikeRepository ArticleLikes
        {
            get
            {
                if (_articleLikes == null)
                    _articleLikes = new ArticleLikeRepository(_context);

                return _articleLikes;
            }
        }



        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
