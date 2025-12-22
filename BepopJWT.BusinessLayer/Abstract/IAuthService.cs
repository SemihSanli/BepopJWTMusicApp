using BepopJWT.DTOLayer.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IAuthService
    {
        Task CreateUser(RegisterDTO registerDTO);
        Task<string>LogIn(LoginDTO loginDTO);
    }
}
