using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface ITagRepository: IRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetTags();
        Task<Tag> GetTag(Guid id);
        Task<Tuple<bool, string>> UpdateTag(Tag tag);
        Task<Tuple<bool, string>> CreateTag(Tag tag);
        Task<Tuple<bool, string>> DeleteTag(Tag tag);
    }
}
