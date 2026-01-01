using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Constants;
using BepopJWT.DTOLayer.SongDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BepopJWT.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SongsController(ISongService songService, IWebHostEnvironment webHostEnvironment)
        {
            _songService = songService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSongs()
        {
            var songs = await _songService.TGetAllAsync();
            return Ok(songs);
        }
        [HttpGet("getsongwitharist")]
        public async Task<IActionResult> GetSongWithArist()
        {
            var songs = await _songService.GetSongsWithArtistsAsync();
            return StatusCode(201, songs);
        }
        [HttpGet("getsongswithcategory")]
        public async Task<IActionResult> GetSongsWithCategory()
        {
            var songs = await _songService.GetSongsWithCategoryAsync();
            return StatusCode(201, songs);
        }
        [HttpPost("fileupload")]
        public async Task<IActionResult> UploadSong([FromForm] CreateSongDTO createSongDto)
        {
           await _songService.AddSongWithFileAsync(createSongDto);
            return Ok(201 + "Dosya Yükleme İşleminiz Başarıyla Tamamlandı");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSong([FromForm] UpdateSongDTO updateSongDto)
        {
            
            await _songService.UpdateWithFileAsync(updateSongDto);
            return Ok("Şarkı başarıyla güncellendi.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            await _songService.DeleteWithFileAsync(id);
            return Ok("Şarkı ve dosyaları başarıyla silindi.");
        }
        [Authorize]
        [HttpGet("check-access/{songId}")]
        public async Task<IActionResult> CheckAccess(int songId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var hasAccess = await _songService.CheckSongAccessAsync(songId, userId);

            if (!hasAccess)
            {
                return Ok(new
                {
                    isSuccess = false,
                    message = "Bu içeriği dinlemek için paket seviyeniz yetersizdir."
                });
            }

            return Ok(new { isSuccess = true });

           
        }
        [HttpGet("recommend")]
        public async Task<IActionResult> GetRecommend([FromQuery] string mood)
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            
            var packageIdClaim = User.FindFirst(CustomClaimType.PackageId);

         
            int packageId = packageIdClaim != null ? int.Parse(packageIdClaim.Value) : 0;

       
            var result = await _songService.GetSongSuggestionByMoodAsync(userId, packageId, mood);

            return Ok(result);
        }
    }
}
