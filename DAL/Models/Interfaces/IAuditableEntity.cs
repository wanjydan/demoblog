using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models.Interfaces
{
    interface IAuditableEntity
    {
        ApplicationUser CreatedBy { get; set; }
        ApplicationUser UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
