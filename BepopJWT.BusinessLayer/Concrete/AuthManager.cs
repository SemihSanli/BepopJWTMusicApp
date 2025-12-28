using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.AuthDTOs;
using BepopJWT.DTOLayer.TokenDTOs;
using BepopJWT.EntityLayer.Entities;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepopJWT.DTOLayer.ArtistDTOs;
using BepopJWT.DTOLayer.FileUploadDTOs;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IFileUploadService _fileUploadService;
        public AuthManager(IUserService userService, ITokenService tokenService, IFileUploadService fileUploadService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _fileUploadService = fileUploadService;
        }

        public async Task CreateUser(RegisterDTO registerDTO)
        {
           var existingUser = await _userService.TGetByEmailAsync(registerDTO.Email);
            if (existingUser !=null)
            {
                throw new Exception("Bu Email Adresi Zaten Kullanımda");
            }

            string imageUrl = "";

            if (registerDTO.ProfileImage != null)
            {
                imageUrl = await _fileUploadService.UploadImageAsync(
                    new UploadImageDTO { imageFile = registerDTO.ProfileImage },
                    "bepop_profile_images"
                );
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.PasswordHash);

            var newUser = new User
            {
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                Username = registerDTO.Username,
                PasswordHash = passwordHash,
                Role = registerDTO.Role,
                ProfileImage = imageUrl,
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
