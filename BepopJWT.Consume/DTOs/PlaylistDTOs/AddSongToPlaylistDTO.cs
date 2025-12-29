using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.PlaylistDTOs
{
    public class AddSongToPlaylistDTO
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }
}
