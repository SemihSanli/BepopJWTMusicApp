using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.SongDTOs
{
    public class ResultSongWithArtists
    {
        public int SongId { get; set; }
        public string SongTitle { get; set; }
        public string FileUrl { get; set; }
        public string ImageUrl { get; set; }
        public int MinLevelRequired { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
    }
}
