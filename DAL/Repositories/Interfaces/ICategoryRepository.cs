using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(Guid id);
        Task<Tuple<bool, string>> UpdateCategory(Category category);
        Task<Tuple<bool, string>> CreateCategory(Category category);
        Task<Tuple<bool, string>> DeleteCategory(Category category);
    }
}
