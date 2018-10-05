using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlog.ViewModels
{
    public class ArticleEditViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }

        public int CategoryId { get; set; }

        public int[] TagIds { get; set; }
    }
}
