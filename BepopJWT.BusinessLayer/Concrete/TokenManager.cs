using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Constants.CustomClaimTypes;
using BepopJWT.DTOLayer.TokenDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class TokenManager : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(UserTokenDTO userTokenDTO)
        {
            //Secret keyimi byte dizisine  çevirdim
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userTokenDTO.UserId.ToString()),
                new Claim(ClaimTypes.Name, userTokenDTO.Username ?? ""),
                new Claim(ClaimTypes.Email, userTokenDTO.Email ?? ""),
                new Claim(ClaimTypes.Role, userTokenDTO.Role ?? "Member")
            };
            if (userTokenDTO.PackageId.HasValue)
            {
                //PackageId'yi "" içinde yazıp hata almamak için BusinessLayer.Constants.CustomClaimTypes içine CustomClaimType ekledim. Ve burada oradan çağırdım.
                claim.Add(new Claim(CustomClaimType.PackageId, userTokenDTO.PackageId.Value.ToString()));
            }
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claim,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
