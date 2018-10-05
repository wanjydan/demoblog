using DAL.Core;
using DAL.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }




    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

                await CreateUserAsync("admin", "tempP@ss123", "Inbuilt Administrator", "admin@ebenmonney.com", "+1 (123) 000-0000", new string[] { adminRoleName });
                await CreateUserAsync("user", "tempP@ss123", "Inbuilt Standard User", "user@ebenmonney.com", "+1 (123) 000-0001", new string[] { userRoleName });

                _logger.LogInformation("Inbuilt account generation completed");
            }



            if (!await _context.Categories.AnyAsync() && !await _context.Tags.AnyAsync())
            {
                _logger.LogInformation("Seeding initial data");

                Random rnd = new Random();

                Category cat_1 = new Category()
                {
                    Name = "Category1",
                    Slug = "category1"
                };
                Category cat_2 = new Category()
                {
                    Name = "Category2",
                    Slug = "category2"
                };
                Category cat_3 = new Category()
                {
                    Name = "Category3",
                    Slug = "category3"
                };


                Tag tag_1 = new Tag()
                {
                    Name = "Tag1",
                    Slug = "tag1"
                };
                Tag tag_2 = new Tag()
                {
                    Name = "Tag2",
                    Slug = "tag2"
                };
                Tag tag_3 = new Tag()
                {
                    Name = "Tag3",
                    Slug = "tag3"
                };
                Tag tag_4 = new Tag()
                {
                    Name = "Tag4",
                    Slug = "tag4"
                };
                Tag tag_5 = new Tag()
                {
                    Name = "Tag5",
                    Slug = "tag5"
                };

                Article art_1 = new Article()
                {
                    Title = "What is Lorem Ipsum?",
                    Slug = "what-is-lorem-ipsum",
                    Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Category = cat_2
                };
                Article art_2 = new Article()
                {
                    Title = "Where does it come from?",
                    Slug = "where-does-it-come-from",
                    Body = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of \"de Finibus Bonorum et Malorum\" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, \"Lorem ipsum dolor sit amet..\", comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H.Rackham.",
                    Category = cat_1
                };
                Article art_3 = new Article()
                {
                    Title = "Why do we use it?",
                    Slug = "why-do-we-use-it",
                    Body = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = cat_3
                };
                Article art_4 = new Article()
                {
                    Title = "Where can I get some?",
                    Slug = "where-can-i-get-some",
                    Body = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using \'Content here, content here\', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for \'lorem ipsum\' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = cat_2
                };

                
                ArticleTag artTag_1 = new ArticleTag()
                {
                    Article = art_1,
                    Tag = tag_1
                };
                ArticleTag artTag_2 = new ArticleTag()
                {
                    Article = art_1,
                    Tag = tag_3
                };
                ArticleTag artTag_3 = new ArticleTag()
                {
                    Article = art_2,
                    Tag = tag_4
                };
                ArticleTag artTag_4 = new ArticleTag()
                {
                    Article = art_2,
                    Tag = tag_3
                };
                ArticleTag artTag_5 = new ArticleTag()
                {
                    Article = art_3,
                    Tag = tag_5
                };
                ArticleTag artTag_6 = new ArticleTag()
                {
                    Article = art_3,
                    Tag = tag_2
                };
                ArticleTag artTag_7 = new ArticleTag()
                {
                    Article = art_4,
                    Tag = tag_1
                };
                ArticleTag artTag_8 = new ArticleTag()
                {
                    Article = art_4,
                    Tag = tag_2
                };
                ArticleTag artTag_9 = new ArticleTag()
                {
                    Article = art_4,
                    Tag = tag_5
                };
                ArticleTag artTag_10 = new ArticleTag()
                {
                    Article = art_2,
                    Tag = tag_4
                };


                Comment comm_1 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 1",
                    Article = art_4
                };
                Comment comm_2 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 2",
                    Article = art_2
                };
                Comment comm_3 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 3",
                    Article = art_3
                };
                Comment comm_4 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 4",
                    Article = art_4
                };
                Comment comm_5 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 5",
                    Article = art_1
                };
                Comment comm_6 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 6",
                    Article = art_2
                };
                Comment comm_7 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 7",
                    Article = art_4
                };
                Comment comm_8 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 8",
                    Article = art_2
                };
                Comment comm_9 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 9",
                    Article = art_1
                };
                Comment comm_10 = new Comment()
                {
                    Body = "There is no one who loves pain itself, who seeks after it and wants to have it, simply because it is pain 10",
                    Article = art_3
                };

                _context.Categories.Add(cat_1);
                _context.Categories.Add(cat_2);
                _context.Categories.Add(cat_3);

                _context.Tags.Add(tag_1);
                _context.Tags.Add(tag_2);
                _context.Tags.Add(tag_3);
                _context.Tags.Add(tag_4);
                _context.Tags.Add(tag_5);

                _context.Articles.Add(art_1);
                _context.Articles.Add(art_2);
                _context.Articles.Add(art_3);
                _context.Articles.Add(art_4);

                _context.ArticleTags.Add(artTag_1);
                _context.ArticleTags.Add(artTag_2);
                _context.ArticleTags.Add(artTag_3);
                _context.ArticleTags.Add(artTag_4);
                _context.ArticleTags.Add(artTag_5);
                _context.ArticleTags.Add(artTag_6);
                _context.ArticleTags.Add(artTag_7);
                _context.ArticleTags.Add(artTag_8);
                _context.ArticleTags.Add(artTag_9);
                _context.ArticleTags.Add(artTag_10);

                _context.Comments.Add(comm_1);
                _context.Comments.Add(comm_2);
                _context.Comments.Add(comm_3);
                _context.Comments.Add(comm_4);
                _context.Comments.Add(comm_5);
                _context.Comments.Add(comm_6);
                _context.Comments.Add(comm_7);
                _context.Comments.Add(comm_8);
                _context.Comments.Add(comm_9);
                _context.Comments.Add(comm_10);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Seeding initial data completed");
            }
        }



        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Item1)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");


            return applicationUser;
        }
    }
}
