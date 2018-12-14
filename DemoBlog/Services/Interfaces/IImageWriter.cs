using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DemoBlog.Services.Interfaces
{
    public interface IImageWriter
    {
        Task<Tuple<bool, string>> UploadImage(IFormFile image, string name);
    }
}