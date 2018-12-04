using System;

namespace DAL.Models
{
    public class Comment : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Body { get; set; }

        public Article Article { get; set; }
    }
}