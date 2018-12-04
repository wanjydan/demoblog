using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Tag : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }
    }
}