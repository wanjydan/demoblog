using System;

namespace DAL.Models.Interfaces
{
    internal interface IAuditableEntity
    {
        ApplicationUser CreatedBy { get; set; }
        ApplicationUser UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}