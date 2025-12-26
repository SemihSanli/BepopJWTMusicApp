using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.FileUploadDTOs
{
    public class UploadImageDTO
    {
        public IFormFile imageFile { get; set; }
        public string rootPath { get; set; }
    }
}
