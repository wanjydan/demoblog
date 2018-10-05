using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetTopActiveUsers(int count);
        IEnumerable<ApplicationUser> GetAllUsersData();
    }
}
