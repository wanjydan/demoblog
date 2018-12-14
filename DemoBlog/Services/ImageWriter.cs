using System;
using System.IO;
using System.Threading.Tasks;
using DemoBlog.Helpers;
using DemoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DemoBlog.Services
{
    public class ImageWriter : IImageWriter
    {
        public async Task<Tuple<bool, string>> UploadImage(IFormFile image, string name)
        {
            if (CheckIfImageFile(image)) return await WriteImage(image, name);

            return Tuple.Create(false, "Invalid image file");
        }

        private bool CheckIfImageFile(IFormFile image)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return MediaWriter.GetMediaFormat(fileBytes) != MediaWriter.MediaFormat.unknown;
        }

        public async Task<Tuple<bool, string>> WriteImage(IFormFile image, string name)
        {
            string fileName;
            try
            {
                var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];
                fileName = name + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\featured", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                return Tuple.Create(false, e.Message);
            }

            return Tuple.Create(true, fileName);
        }
    }
}