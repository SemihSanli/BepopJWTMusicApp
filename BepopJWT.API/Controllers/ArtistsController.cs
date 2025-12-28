using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.ArtistDTOs;
using BepopJWT.EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await _artistService.TGetAllAsync();
            return StatusCode(201, artists);
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
    }
}
