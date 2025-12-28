using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.PlaylistDTOs
{
    public class CreatePlaylistDTO
    {
        public string PlaylistName { get; set; }
        public int UserId { get; set; } 
    }
}
