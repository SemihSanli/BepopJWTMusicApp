using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.SongDTOs
{
    public class CreateSongDTO
    {
        public string SongTitle { get; set; }
        public int CategoryId { get; set; }
        public int ArtistId { get; set; }
        public int MinLevelRequired { get; set; } // Hangi paket dinleyebilir?

        // Dosya yüklemek için IFormFile kullanıyoruz
        public IFormFile SongFile { get; set; }  // MP3 Dosyası
        public IFormFile ImageFile { get; set; } // Kapak Resmi
    }
}
