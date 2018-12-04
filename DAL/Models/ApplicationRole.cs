using System;
using System.Collections.Generic;
using DAL.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public sealed class ApplicationRole : IdentityRole, IAuditableEntity
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public ApplicationRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        public string Description { get; set; }

        public ICollection<IdentityUserRole<string>> Users { get; set; }

        public ICollection<IdentityRoleClaim<string>> Claims { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}