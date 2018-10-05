using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ArticleLike: AuditableEntity
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public string CreatedById { get; set; }
        public string UpdatedById { get; set; }
    }
}
