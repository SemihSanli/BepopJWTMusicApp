using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.PlaylistDTOs
{
    public class ResultPlaylistWithSongsDTO
    {
       public int PlaylistId { get; set; }
       public string PlaylistName { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }    
        public List<ResultsSongsForPlaylistDTO> Songs { get; set; }
    }
}
