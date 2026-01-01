using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.PlaylistDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlayListService _playlistService;

        public PlaylistsController(IPlayListService playlistService)
        {
            _playlistService = playlistService;
        }

     
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPlaylists(int userId)
        {
           
            var values = await _playlistService.GetPlaylistsByUserIdAsync(userId);
            return Ok(values);
        }

     
        [HttpPost("create")]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDTO createPlaylistDto)
        {
            await _playlistService.CreatePlayListAsync(createPlaylistDto);
            return StatusCode(201, "Playlist başarıyla oluşturuldu.");
        }

     
        [HttpPost("add-song")]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] AddSongToPlaylistDTO addSongToPlaylistDto)
        {
            await _playlistService.AddSongToPlaylistAsync(addSongToPlaylistDto);
            return Ok("Şarkı listeye eklendi.");
        }

     
        [HttpGet]
        public async Task<IActionResult> GetAllPlaylists()
        {
            var values = await _playlistService.TGetAllAsync();
            return Ok(values);
        }
        [HttpGet("GetPlaylistById/{id}")]
        public async Task<IActionResult> GetPlaylistById(int id)
        {
            var value = await _playlistService.GetPlaylistWithSongsByIdAsync(id);

            if (value == null)
            {
                return NotFound("Böyle bir çalma listesi bulunamadı.");
            }

            return Ok(value);
        }


    }
}
