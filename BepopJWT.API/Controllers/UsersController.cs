using BepopJWT.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("getAllUsers")]
        public async Task< IActionResult> GetAllUsers()
        {
            var values = await _userService.TGetAllAsync();
            return StatusCode(201, values);
        }
        [HttpGet("getbyemail")]
        public async Task< IActionResult> GetByEmail(string email)
        {
            // Sen serviste yazmışsın ya, işte onu burada çağıracağız.
            var user = await _userService.TGetByEmailAsync(email);

            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
