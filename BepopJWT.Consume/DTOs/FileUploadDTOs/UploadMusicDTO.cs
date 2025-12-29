using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.FileUploadDTOs
{
    public class UploadMusicDTO
    {
        public IFormFile musicFile { get; set; }
        public string rootPath { get; set; }    
    }
}
