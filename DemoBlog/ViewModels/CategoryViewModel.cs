using System;
using System.Collections.Generic;
using System.Linq;


namespace DemoBlog.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<ArticleListViewModel> Articles { get; set; }
    }
}
