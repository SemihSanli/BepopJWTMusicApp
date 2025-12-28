using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.PlaylistDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlayListService _playlistService;

        public PlaylistsController(IPlayListService playlistService)
        {
            _playlistService = playlistService;
        }

        // 1. KULLANICININ LİSTELERİNİ GETİR (Resimli, Şarkılı, İsimli)
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPlaylists(int userId)
        {
            // Manager'daki o özel DTO dönüşümünü çağırıyoruz
            var values = await _playlistService.GetPlaylistsByUserIdAsync(userId);
            return Ok(values);
        }

        // 2. YENİ LİSTE OLUŞTUR
        [HttpPost("create")]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDTO createPlaylistDto)
        {
            await _playlistService.CreatePlayListAsync(createPlaylistDto);
            return StatusCode(201, "Playlist başarıyla oluşturuldu.");
        }

        // 3. LİSTEYE ŞARKI EKLE
        [HttpPost("add-song")]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] AddSongToPlaylistDTO addSongToPlaylistDto)
        {
            await _playlistService.AddSongToPlaylistAsync(addSongToPlaylistDto);
            return Ok("Şarkı listeye eklendi.");
        }

        // 4. (Opsiyonel) TÜM LİSTELERİ GÖR
        [HttpGet]
        public async Task<IActionResult> GetAllPlaylists()
        {
            var values = await _playlistService.TGetAllAsync();
            return Ok(values);
        }
    }
}
