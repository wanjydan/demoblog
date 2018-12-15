using System;
using System.Collections.Generic;

namespace DemoBlog.ViewModels.ArticleViewModels
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public int Likes { get; set; }
        public bool Like { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public AuthorViewModel Author { get; set; }

        public ArticleCategoryViewModel Category { get; set; }

        public ICollection<ArticleTagViewModel> Tags { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}