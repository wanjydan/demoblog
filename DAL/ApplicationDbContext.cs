using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Models.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationUser CurrentUser { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
//            builder.Entity<ApplicationUser>().HasMany(u => u.Articles).WithOne(a => a.CreatedBy).HasForeignKey(a => a.CreatedById);
//            builder.Entity<ApplicationUser>().HasMany(u => u.Articles).WithOne(a => a.UpdatedBy).HasForeignKey(a => a.UpdatedById);

            builder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);
//            builder.Entity<Category>().HasOne(c => c.CreatedBy).WithMany(u => u.Categories);
//            builder.Entity<Category>().HasOne(c => c.UpdatedBy).WithMany(u => u.Categories);
            builder.Entity<Category>().ToTable($"App{nameof(Categories)}");

//            builder.Entity<Tag>().HasKey(t => t.Id);
            builder.Entity<Tag>().Property(t => t.Name).IsRequired().HasMaxLength(100);
//            builder.Entity<Tag>().HasOne(t => t.CreatedBy).WithMany(u => u.Tags);
//            builder.Entity<Tag>().HasOne(t => t.UpdatedBy).WithMany(u => u.Tags);
            builder.Entity<Tag>().ToTable($"App{nameof(Tags)}");

            builder.Entity<Comment>().Property(c => c.Body).IsRequired();
//            builder.Entity<Comment>().HasOne(c => c.Article).WithMany(a => a.Comments);
//            builder.Entity<Comment>().HasOne(c => c.CreatedBy).WithMany(u => u.Comments);
//            builder.Entity<Comment>().HasOne(c => c.UpdatedBy).WithMany(u => u.Comments);
            builder.Entity<Comment>().ToTable($"App{nameof(Comments)}");

            builder.Entity<Article>().Property(a => a.Title).IsRequired().HasMaxLength(100);
            //            builder.Entity<Article>().HasIndex(a => a.Title);
            //            builder.Entity<Article>().HasOne(a => a.Category).WithMany(c => c.Articles);
            //            builder.Entity<Article>().HasOne(a => a.CreatedBy).WithMany(u => u.Articles).HasForeignKey(a => a.CreatedById);
            //            builder.Entity<Article>().HasOne(a => a.UpdatedBy).WithMany(u => u.Articles).HasForeignKey(a => a.CreatedById);
            builder.Entity<Article>().ToTable($"App{nameof(Articles)}");

            builder.Entity<ArticleTag>().HasKey(at => new {at.Id, at.ArticleId, at.TagId});
//            builder.Entity<ArticleTag>().Property(at => at.Id).ValueGeneratedOnAdd();
            builder.Entity<ArticleTag>().HasOne(at => at.Article).WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId);
            builder.Entity<ArticleTag>().HasOne(at => at.Tag).WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);
            builder.Entity<ArticleTag>().ToTable($"App{nameof(ArticleTags)}");

            builder.Entity<ArticleLike>().HasKey(al => new {al.Id, al.ArticleId, al.CreatedById});
//            builder.Entity<ArticleLike>().Property(al => al.Id).ValueGeneratedOnAdd();
            builder.Entity<ArticleLike>().HasOne(al => al.Article).WithMany(a => a.ArticleLikes)
                .HasForeignKey(al => al.ArticleId);
            builder.Entity<ArticleLike>().HasOne(al => al.CreatedBy).WithMany(u => u.ArticleLikes)
                .HasForeignKey(al => al.CreatedById);
            builder.Entity<ArticleLike>().ToTable($"App{nameof(ArticleLikes)}");
        }


        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity) entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUser;
                }
                else
                {
                    base.Entry(entity).Reference(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUser;
            }
        }
    }
}