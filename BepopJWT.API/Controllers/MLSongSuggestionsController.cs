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

            // 2. Eğer yapay zeka bir şey önermediyse (yeni kullanıcı vb.) boş liste dön
            if (songIds == null || !songIds.Any())
            {
                // İstersen burada "En Çok Dinlenenler" gibi bir fallback yapabilirsin
                // Şimdilik boş dönüyoruz
                return Ok(new List<ResultSongWithArtists>());
            }

            // 3. Şarkı Servisinden DTO iste (Controller artık mapping yapmıyor!)
            var recommendedSongsDTO = await _songService.GetSongsByIdsAsync(songIds);

            // 4. Sonucu dön
            return Ok(recommendedSongsDTO);
        }
    }
}
