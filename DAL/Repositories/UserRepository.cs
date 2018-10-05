using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    class UserRepository: Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        { }


        public IEnumerable<ApplicationUser> GetTopActiveUsers(int count)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ApplicationUser> GetAllUsersData()
        {
            return _appContext.Users
//                .Include(u => u.Articles).ThenInclude(a => a.Comments)
                .OrderBy(u => u.FullName)
                .ToList();
        }



        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
