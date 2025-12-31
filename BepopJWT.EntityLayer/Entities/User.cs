using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.EntityLayer.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Member";
        public int? PackageId { get; set; }
        public Package Package { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}
