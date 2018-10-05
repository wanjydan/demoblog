using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Category: AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
