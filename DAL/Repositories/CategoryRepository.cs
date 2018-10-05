using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _appContext.Categories
                .ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _appContext.Categories
                .Include(c => c.Articles).ThenInclude(a => a.CreatedBy)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Tuple<bool, string>> UpdateCategory(Category category)
        {
//            _appContext.Entry(category).State = EntityState.Modified;
            _appContext.Categories.Update(category);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> CreateCategory(Category category)
        {
           await _appContext.Categories.AddAsync(category);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }

        public async Task<Tuple<bool, string>> DeleteCategory(Category category)
        {
            _appContext.Categories.Remove(category);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, string.Empty);
        }


        private ApplicationDbContext _appContext => (ApplicationDbContext) _context;
    }
}