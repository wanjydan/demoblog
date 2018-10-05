using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoBlog.Helpers.Interfaces
{
    public interface IImageHandler
    {
        Task<Tuple<bool, string>> UploadImage(IFormFile image, string slug);
    }
}
