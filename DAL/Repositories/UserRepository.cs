using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    internal class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;


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
    }
}