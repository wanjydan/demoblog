using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemoBlog.Helpers.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DemoBlog.Helpers
{
    public class ImageWriter : IImageWriter
    {
        public async Task<Tuple<bool, string>> UploadImage(IFormFile image, string slug)
        {
            if (CheckIfImageFile(image))
            {
                return await WriteFile(image, slug);
            }

            return Tuple.Create(false, "Invalid image file");
        }

        /// <summary>
        /// Method to check if file is image file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return MediaWriter.GetMediaFormat(fileBytes) != MediaWriter.MediaFormat.unknown;
        }

        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> WriteFile(IFormFile image, string slug)
        {
            string fileName;
            try
            {
                var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];
//                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 
                fileName = slug + extension;
                //for the file due to security reasons.
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
