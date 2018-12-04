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

                var appUser1 = new ApplicationUser
                {
                    UserName = "admin",
                    FullName = "Inbuilt Administrator",
                    Email = "admin@demoblog.com",
                    PhoneNumber = "+1 (123) 000-0000",

                    EmailConfirmed = true,
                    IsEnabled = true
                };

                var appUser2 = new ApplicationUser
                {
                    UserName = "user",
                    FullName = "Inbuilt Standard User",
                    Email = "user@demoblog.com",
                    PhoneNumber = "+1 (123) 000-0001",
                    EmailConfirmed = true,
                    IsEnabled = true
                };

                await CreateUserAsync(appUser1, "P@ssw0rd!", new[] {adminRoleName});
                await CreateUserAsync(appUser2, "P@ssw0rd!", new[] {userRoleName});

                _logger.LogInformation("Inbuilt account generation completed");

                _logger.LogInformation("Seeding initial data");

                var users = await _accountManager.GetAllUsersAsync();
                
                var random = new Random();
                var randUser = users[random.Next(users.Count)];

                var cat1 = new Category
                {
                    Name = "Category1",
                    Slug = "category1",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var cat2 = new Category
                {
                    Name = "Category2",
                    Slug = "category2",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var cat3 = new Category
                {
                    Name = "Category3",
                    Slug = "category3",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };


                var tag1 = new Tag
                {
                    Name = "Tag1",
                    Slug = "tag1",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var tag2 = new Tag
                {
                    Name = "Tag2",
                    Slug = "tag2",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var tag3 = new Tag
                {
                    Name = "Tag3",
                    Slug = "tag3",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var tag4 = new Tag
                {
                    Name = "Tag4",
                    Slug = "tag4",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var tag5 = new Tag
                {
                    Name = "Tag5",
                    Slug = "tag5",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };

                var art1 = new Article
                {
                    Title = "What is Lorem Ipsum?",
                    Slug = "what-is-lorem-ipsum",
                    Body =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Category = cat2,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var art2 = new Article
                {
                    Title = "Where does it come from?",
                    Slug = "where-does-it-come-from",
                    Body =
                        "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H.Rackham.",
                    Category = cat1,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var art3 = new Article
                {
                    Title = "Why do we use it?",
                    Slug = "why-do-we-use-it",
                    Body =
                        "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = cat3,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var art4 = new Article
                {
                    Title = "Where can I get some?",
                    Slug = "where-can-i-get-some",
                    Body =
                        "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = cat2,
                    Image = "/images/featured/not-found.jpg",
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };


                var artTag1 = new ArticleTag
                {
                    Article = art1,
                    Tag = tag3,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag2 = new ArticleTag
                {
                    Article = art2,
                    Tag = tag5,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag3 = new ArticleTag
                {
                    Article = art3,
                    Tag = tag2,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag4 = new ArticleTag
                {
                    Article = art4,
                    Tag = tag1,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag5 = new ArticleTag
                {
                    Article = art3,
                    Tag = tag4,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag6 = new ArticleTag
                {
                    Article = art2,
                    Tag = tag3,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag7 = new ArticleTag
                {
                    Article = art4,
                    Tag = tag2,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag8 = new ArticleTag
                {
                    Article = art1,
                    Tag = tag5,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag9 = new ArticleTag
                {
                    Article = art3,
                    Tag = tag1,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var artTag10 = new ArticleTag
                {
                    Article = art2,
                    Tag = tag4,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };


                var comm1 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 1",
                    Article = art4,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm2 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 2",
                    Article = art2,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm3 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 3",
                    Article = art3,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm4 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 4",
                    Article = art4,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm5 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 5",
                    Article = art1,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm6 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 6",
                    Article = art2,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm7 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 7",
                    Article = art4,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm8 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 8",
                    Article = art2,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm9 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 9",
                    Article = art1,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };
                var comm10 = new Comment
                {
                    Body =
                        "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 10",
                    Article = art3,
                    CreatedBy = randUser,
                    UpdatedBy = randUser
                };

                _context.Categories.Add(cat1);
                _context.Categories.Add(cat2);
                _context.Categories.Add(cat3);

                _context.Tags.Add(tag1);
                _context.Tags.Add(tag2);
                _context.Tags.Add(tag3);
                _context.Tags.Add(tag4);
                _context.Tags.Add(tag5);

                _context.Articles.Add(art1);
                _context.Articles.Add(art2);
                _context.Articles.Add(art3);
                _context.Articles.Add(art4);

                _context.ArticleTags.Add(artTag1);
                _context.ArticleTags.Add(artTag2);
                _context.ArticleTags.Add(artTag3);
                _context.ArticleTags.Add(artTag4);
                _context.ArticleTags.Add(artTag5);
                _context.ArticleTags.Add(artTag6);
                _context.ArticleTags.Add(artTag7);
                _context.ArticleTags.Add(artTag8);
                _context.ArticleTags.Add(artTag9);
                _context.ArticleTags.Add(artTag10);

                _context.Comments.Add(comm1);
                _context.Comments.Add(comm2);
                _context.Comments.Add(comm3);
                _context.Comments.Add(comm4);
                _context.Comments.Add(comm5);
                _context.Comments.Add(comm6);
                _context.Comments.Add(comm7);
                _context.Comments.Add(comm8);
                _context.Comments.Add(comm9);
                _context.Comments.Add(comm10);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Seeding initial data completed");
            }
        }


        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
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


            return applicationUser;
        }
    }
}