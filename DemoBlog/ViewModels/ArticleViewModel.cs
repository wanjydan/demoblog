using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;


namespace DemoBlog.ViewModels
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ArticleUserViewModel Author { get; set; }

        public ArticleCategoryViewModel Category { get; set; }
        
        public ICollection<ArticleTagViewModel> Tags { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }

    public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
    {
        public ArticleViewModelValidator()
        {
            RuleFor(register => register.Title).NotEmpty().WithMessage("Article title cannot be empty");
            RuleFor(register => register.Body).NotEmpty().WithMessage("Body cannot be empty");
        }
    }
}
