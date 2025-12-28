using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.SongDTOs
{
    public class UpdateSongDTO
    {
        public int SongId { get; set; } 
        public string SongTitle { get; set; }
        public int MinLevelRequired { get; set; }
        public int ArtistId { get; set; }
        public int CategoryId { get; set; }


        public IFormFile? SongFile { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
