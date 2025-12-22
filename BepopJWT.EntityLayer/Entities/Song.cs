using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.EntityLayer.Entities
{
    public class Song
    {
        public int SongId { get; set; }
        public string SongTitle { get; set; }
        public string FileUrl { get; set; }
        public string ImageUrl { get; set; }
        public int MinLevelRequired { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }
}
