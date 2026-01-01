using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.PlaylistDTOs
{
    public class ResultsSongsForPlaylistDTO
    {
        public int SongId { get; set; }
        public string SongTitle { get; set; }
        public string ArtistName { get; set; }
        public string ImageUrl { get; set; }
        public int MinLevelRequired { get; set; }
        public string FileUrl { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
