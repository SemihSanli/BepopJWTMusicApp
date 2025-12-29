using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.AuthDTOs
{
    public class LoginDTO
    {
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
