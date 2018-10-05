using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DemoBlog.Helpers.Interfaces
{
    public interface IImageWriter
    {
        Task<Tuple<bool, string>> UploadImage(IFormFile image, string slug);
    }
}
