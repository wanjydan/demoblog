using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoBlog.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoBlog.Helpers
{
    public class ImageHandler : IImageHandler
    {
        private readonly IImageWriter _imageWriter;
        public ImageHandler(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }

        public async Task<Tuple<bool, string>> UploadImage(IFormFile image, string slug)
        {
            return await _imageWriter.UploadImage(image, slug);
        }
    }
}
