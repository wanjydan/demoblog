using System;
using System.Collections.Generic;

namespace DemoBlog.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<ArticleViewModel> Articles { get; set; }
    }
}