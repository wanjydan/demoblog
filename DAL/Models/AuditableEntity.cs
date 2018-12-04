using System;
using DAL.Models.Interfaces;

namespace DAL.Models
{
    public class AuditableEntity : IAuditableEntity
    {
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}