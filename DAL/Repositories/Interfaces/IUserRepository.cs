using System.Collections.Generic;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetTopActiveUsers(int count);
        IEnumerable<ApplicationUser> GetAllUsersData();
    }
}