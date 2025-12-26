using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.SongDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
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
        [HttpPost("fileupload")]
        public async Task<IActionResult> UploadSong([FromForm] CreateSongDTO createSongDto)
        {
            string rootPath = _webHostEnvironment.WebRootPath;

            await _songService.AddSongWithFileAsync(createSongDto, rootPath);
            return Ok(201 + "Dosya Yükleme İşleminiz Başarıyla Tamamlandı");
        }
    }
}
