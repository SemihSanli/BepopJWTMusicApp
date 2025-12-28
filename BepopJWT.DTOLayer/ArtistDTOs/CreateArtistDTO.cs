using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.ArtistDTOs
{
    public class CreateArtistDTO
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
