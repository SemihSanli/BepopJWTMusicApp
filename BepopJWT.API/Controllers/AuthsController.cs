using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.AuthDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO registerDto)
        {
            try
            {
                await _authService.CreateUser(registerDto);
                return Ok("Kayıt Başarılı");
            }
            catch (Exception ex)
            {

              return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginDTO loginDto)
        {
            try
            {
                var token = await _authService.LogIn(loginDto);
                
                return Ok(new
                {
                    Token = token,
                    Message = "Başarıyla Giriş Yapıldı"
                });

                           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
