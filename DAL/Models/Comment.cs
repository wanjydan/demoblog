using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Comment: AuditableEntity
    {
        public int Id { get; set; }
        public string Body { get; set; }
        
        public Article Article { get; set; }
    }
}
