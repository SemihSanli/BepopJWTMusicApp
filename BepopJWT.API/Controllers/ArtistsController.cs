using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.ArtistDTOs;
using BepopJWT.EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await _artistService.TGetAllAsync();
            return StatusCode(201, artists);
        }

        [HttpGet("GetArtistDetail/{id}")]
        public async Task<IActionResult> GetArtistDetail(int id)
        {
            var artistDetail = await _artistService.GetArtistWithSongsByIdAsync(id);

            if (artistDetail == null)
            {
                return NotFound("Sanatçı bulunamadı.");
            }

            return Ok(artistDetail);
        }
        [HttpPost]
        public async Task<IActionResult> CreateArtist(CreateArtistDTO createArtistDTO)
        {
            try
            {
               
              await _artistService.CreateArtistWithImageFileAsync(createArtistDTO);
                return StatusCode(201, new { Message = "Sanatçı Başarıyla Eklendi" });
            }
            catch (Exception ex)
            {

               return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateArtist(UpdateArtistDTO updateArtistDTO)
        {
            try
            {
                await _artistService.UpdateArtistWithImageFileAsync(updateArtistDTO);

                return StatusCode(201,new { Message = "Sanatçı başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveArtist(int id)
        {
            try
            {
                await _artistService.DeleteArtistWithImageFileAsync(id);

                return StatusCode(201,new { Message = "Sanatçı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            var values = await _artistService.TGetByIdAsync(id);

            if (values == null)
            {
                return NotFound(new { Message = "Böyle bir sanatçı bulunamadı." });
            }

            return StatusCode(201,values);
        }
        [HttpGet("GetArtistCount")]
        public async Task<IActionResult> GetArtistCount()
        {
            var count = await _artistService.GetArtistCountAsync();
            return StatusCode(201, new { ArtistCount = count });
        }
    }
}
