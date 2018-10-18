using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Core;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class Article: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }

        public Category Category { get; set; }
        
        public ICollection<ArticleTag> ArticleTags { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleLike> ArticleLikes { get; set; }
    }
}
