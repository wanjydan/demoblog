using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlog.ViewModels
{
    public class ArticleCreateViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public Guid CategoryId { get; set; }

        public Guid[] TagIds { get; set; }
    }
}
