using BepopJWT.DTOLayer.TokenDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface ITokenService
    {
        string CreateToken(UserTokenDTO userTokenDTO);
    }
}
