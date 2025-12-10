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

        public int PackageId { get; set; }
        public Package Package { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }
}
