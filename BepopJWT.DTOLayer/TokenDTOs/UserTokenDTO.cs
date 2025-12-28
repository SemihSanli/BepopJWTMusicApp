using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.TokenDTOs
{
    public class UserTokenDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfileImage { get; set; }
        public int? PackageId { get; set; }
    }
}
