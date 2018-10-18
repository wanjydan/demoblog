using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ArticleTag: AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
