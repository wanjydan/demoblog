using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }


    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IAccountManager _accountManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager,
            ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync()
                && !await _context.Categories.AnyAsync()
                && !await _context.Tags.AnyAsync()
                && !await _context.Articles.AnyAsync()
                && !await _context.ArticleTags.AnyAsync()
                && !await _context.Comments.AnyAsync()
            )
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator",
                    ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

                var user1 = await CreateUserAsync(new ApplicationUser
                {
                    UserName = "Admin",
                    FullName = "Inbuilt Administrator",
                    Email = "admin@demoblog.com",
                    PhoneNumber = "+1 (123) 000-0000",
                    EmailConfirmed = true,
                    IsEnabled = true
                }, "P@ssw0rd!", new[] {adminRoleName});
                var user2 = await CreateUserAsync(new ApplicationUser
                {
                    UserName = "User",
                    FullName = "Inbuilt Standard User",
                    Email = "user@demoblog.com",
                    PhoneNumber = "+1 (123) 000-0001",
                    EmailConfirmed = true,
                    IsEnabled = true
                }, "P@ssw0rd!", new[] {userRoleName});

                _logger.LogInformation("Inbuilt account generation completed");

                _logger.LogInformation("Seeding initial data");

                var cat1 = new Category
                {
                    Name = "Category1",
                    Slug = "category1",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var cat2 = new Category
                {
                    Name = "Category2",
                    Slug = "category2",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var cat3 = new Category
                {
                    Name = "Category3",
                    Slug = "category3",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };


                var tag1 = new Tag
                {
                    Name = "Tag1",
                    Slug = "tag1",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var tag2 = new Tag
                {
                    Name = "Tag2",
                    Slug = "tag2",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var tag3 = new Tag
                {
                    Name = "Tag3",
                    Slug = "tag3",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var tag4 = new Tag
                {
                    Name = "Tag4",
                    Slug = "tag4",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var tag5 = new Tag
                {
                    Name = "Tag5",
                    Slug = "tag5",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };

                var art1 = new Article
                {
                    Title = "What is Lorem Ipsum?",
                    Slug = "what-is-lorem-ipsum",
                    Body =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Category = cat2,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var art2 = new Article
                {
                    Title = "Where does it come from?",
                    Slug = "where-does-it-come-from",
                    Body =
                        "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H.Rackham.",
                    Category = cat1,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var art3 = new Article
                {
                    Title = "Why do we use it?",
                    Slug = "why-do-we-use-it",
                    Body =
                        "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = cat3,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var art4 = new Article
                {
                    Title = "Where can I get some?",
                    Slug = "where-can-i-get-some",
                    Body =
                        "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = cat2,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = user2,
                    UpdatedBy = user2
                };


                var artTag1 = new ArticleTag
                {
                    Article = art1,
                    Tag = tag3,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var artTag2 = new ArticleTag
                {
                    Article = art2,
                    Tag = tag5,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var artTag3 = new ArticleTag
                {
                    Article = art3,
                    Tag = tag2,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var artTag4 = new ArticleTag
                {
                    Article = art4,
                    Tag = tag1,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var artTag5 = new ArticleTag
                {
                    Article = art3,
                    Tag = tag4,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var artTag6 = new ArticleTag
                {
                    Article = art2,
                    Tag = tag3,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var artTag7 = new ArticleTag
                {
                    Article = art4,
                    Tag = tag2,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var artTag8 = new ArticleTag
                {
                    Article = art1,
                    Tag = tag5,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var artTag9 = new ArticleTag
                {
                    Article = art3,
                    Tag = tag1,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var artTag10 = new ArticleTag
                {
                    Article = art2,
                    Tag = tag4,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };


                var comm1 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 1",
                    Article = art4,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var comm2 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 2",
                    Article = art2,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var comm3 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 3",
                    Article = art3,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var comm4 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 4",
                    Article = art4,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var comm5 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 5",
                    Article = art1,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var comm6 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 6",
                    Article = art2,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var comm7 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 7",
                    Article = art4,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var comm8 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 8",
                    Article = art2,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };
                var comm9 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 9",
                    Article = art1,
                    CreatedBy = user1,
                    UpdatedBy = user1
                };
                var comm10 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 10",
                    Article = art3,
                    CreatedBy = user2,
                    UpdatedBy = user2
                };


                await _context.Categories.AddRangeAsync(cat1, cat2, cat3);

                await _context.Tags.AddRangeAsync(tag1, tag2, tag3, tag4, tag5);

                await _context.Articles.AddRangeAsync(art1, art2, art3, art4);

                await _context.ArticleTags.AddRangeAsync(artTag1, artTag2, artTag3, artTag4, artTag5, artTag6, artTag7,
                    artTag8, artTag9, artTag10);

                await _context.Comments.AddRangeAsync(comm1, comm2, comm3, comm4, comm5, comm6, comm7, comm8, comm9,
                    comm10);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Seeding initial data completed");
            }
        }


        private async Task EnsureRoleAsync(string roleName, string description, IEnumerable<string> claims)
        {
            if (await _accountManager.GetRoleByNameAsync(roleName) == null)
            {
                var applicationRole = new ApplicationRole(roleName, description);

                var result = await _accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Item1)
                    throw new Exception(
                        $"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(ApplicationUser applicationUser, string password,
            IEnumerable<string> roles)
        {
            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Item1)
                throw new Exception(
                    $"Seeding \"{applicationUser.UserName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");


            var user = await _accountManager.GetUserByIdAsync(applicationUser.Id);
            return user;
        }
    }
}