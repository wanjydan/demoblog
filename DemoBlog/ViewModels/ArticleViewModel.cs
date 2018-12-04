using System;
using System.Collections.Generic;
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

        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public UserViewModel Author { get; set; }

        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public CategoryViewModel Category { get; set; }

        public ICollection<TagViewModel> Tags { get; set; }

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