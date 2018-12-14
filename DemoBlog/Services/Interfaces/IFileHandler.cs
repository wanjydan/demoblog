using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DemoBlog.Services.Interfaces
{
    public interface IFileHandler
    {
        Task<Tuple<bool, string>> UploadFile(IFormFile file, string name);
    }
}