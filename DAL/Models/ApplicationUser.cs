using System;
using System.Collections.Generic;
using DAL.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public sealed class ApplicationUser : IdentityUser, IAuditableEntity
    {
        public string FullName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => LockoutEnabled && LockoutEnd >= DateTimeOffset.UtcNow;

        public ICollection<IdentityUserRole<string>> Roles { get; set; }

        public ICollection<IdentityUserClaim<string>> Claims { get; set; }

//        public ICollection<Article> Articles { get; set; }
//        public ICollection<Category> Categories { get; set; }
//        public ICollection<Tag> Tags { get; set; }
//        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleLike> ArticleLikes { get; set; }

        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}