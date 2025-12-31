using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.EntityLayer.Entities
{
    public class Favorite
    {
        public int FavoriteId { get; set; }

        public int UserId { get; set; } 
        public User user { get; set; }

        public int SongId { get; set; } 
        public Song Song { get; set; }
    }
}
