using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ArticleTag: AuditableEntity
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
