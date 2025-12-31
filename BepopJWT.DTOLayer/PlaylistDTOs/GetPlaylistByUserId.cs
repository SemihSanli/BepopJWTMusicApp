using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.PlaylistDTOs
{
    public class GetPlaylistByUserId
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public List<ResultsSongsForPlaylistDTO> Songs { get; set; }
    }
}
