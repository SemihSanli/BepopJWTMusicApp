using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.AuthDTOs;
using BepopJWT.DTOLayer.TokenDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public AuthManager(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task CreateUser(RegisterDTO registerDTO)
        {
           var existingUser = await _userService.TGetByEmailAsync(registerDTO.Email);
            if (existingUser !=null)
            {
                throw new Exception("Bu Email Adresi Zaten Kullanımda");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.PasswordHash);

            var newUser = new User
            {
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                Username = registerDTO.Username,
                PasswordHash = passwordHash,
                Role = registerDTO.Role,
                PackageId = null
            };
            await _userService.TAddAsync(newUser);
        }

        public async Task<string> LogIn(LoginDTO loginDTO)
        {
            var user = await _userService.TGetByEmailAsync(loginDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.PasswordHash, user.PasswordHash))
            {
                throw new Exception("Geçersiz Email Veya Şifre");
            }

            var tokenUserDto = new UserTokenDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                PackageId = user.PackageId
            };
          return _tokenService.CreateToken(tokenUserDto);
        }
    }
}
