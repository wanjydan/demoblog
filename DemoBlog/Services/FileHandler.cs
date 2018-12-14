using System;
using System.Threading.Tasks;
using DemoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DemoBlog.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly IImageWriter _imageWriter;

        public FileHandler(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }

        public async Task<Tuple<bool, string>> UploadFile(IFormFile file, string name)
        {
            return await _imageWriter.UploadImage(file, name);
        }
    }
}