using BepopJWT.DTOLayer.SongDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.ArtistDTOs
{
    public class GetArtistDetailsDTO
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<ResultSongWithArtists> resultSongs { get; set; }
    }
}
