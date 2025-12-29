using BepopJWT.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
