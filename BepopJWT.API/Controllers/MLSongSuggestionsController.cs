using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Concrete;
using BepopJWT.DTOLayer.SongDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MLSongSuggestionsController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly IMLRecommendationService _mlRecommendationService; // <-- EKLENDİ

        // Constructor'ı güncelle
        public MLSongSuggestionsController(ISongService songService, IMLRecommendationService recommendationManager)
        {
            _songService = songService;
            _mlRecommendationService = recommendationManager;
        }

       

        [HttpGet("GetRecommendations/{userId}")]
        public async Task<IActionResult> GetRecommendations(int userId)
        {
            var songIds = await _mlRecommendationService.GetRecommendedSongIds(userId);

          
            if (songIds == null || !songIds.Any())
            {
              
                return Ok(new List<ResultSongWithArtists>());
            }

          
            var recommendedSongsDTO = await _songService.GetSongsByIdsAsync(songIds);

          
            return Ok(recommendedSongsDTO);
        }
    }
}
