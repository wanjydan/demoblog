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
    class TagRepository: Repository<Tag>, ITagRepository
    {
        public TagRepository(DbContext context) : base(context)
        { }


        public async Task<IEnumerable<Tag>> GetTags()
        {
            return await _appContext.Tags
                .ToListAsync();
        }

        public async Task<Tag> GetTag(int id)
        {
            return await _appContext.Tags
                .Include(t => t.ArticleTags).ThenInclude(at => at.Article).ThenInclude(a => a.CreatedBy)
                .Include(t => t.ArticleTags).ThenInclude(at => at.Article).ThenInclude(a => a.Category)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Tuple<bool, string>> UpdateTag(Tag tag)
        {
            _appContext.Tags.Update(tag);

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

        public async Task<Tuple<bool, string>> CreateTag(Tag tag)
        {
            await _appContext.Tags.AddAsync(tag);

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

        public async Task<Tuple<bool, string>> DeleteTag(Tag tag)
        {
            _appContext.Tags.Remove(tag);

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

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
