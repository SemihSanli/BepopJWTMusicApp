
using BepopJWT.Consume.Helpers;
using BepopJWT.Consume.SongDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _DiscoverComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _DiscoverComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();
            var token = HttpContext.User.FindFirst("AccessToken")?.Value;
            var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 1. Önerilen şarkıları ML API'sinden çekiyoruz
            var mlResponse = await client.GetAsync($"https://localhost:7209/api/MLSongSuggestions/GetRecommendations/{userIdStr}");

            List<ResultSongWithArtists> recommendedSongs = new List<ResultSongWithArtists>();

            if (mlResponse.IsSuccessStatusCode)
            {
                var mlData = await mlResponse.Content.ReadAsStringAsync();
                recommendedSongs = JsonConvert.DeserializeObject<List<ResultSongWithArtists>>(mlData);
            }

            // 2. Sayfanın geri kalanı için genel şarkıları çekiyoruz
            var allSongsResponse = await client.GetAsync("https://localhost:7209/api/Songs");
            List<ResultSongWithArtists> allSongs = new List<ResultSongWithArtists>();

            if (allSongsResponse.IsSuccessStatusCode)
            {
                var allData = await allSongsResponse.Content.ReadAsStringAsync();
                allSongs = JsonConvert.DeserializeObject<List<ResultSongWithArtists>>(allData);
            }

            // İki listeyi de View'a göndermek için bir ViewModel kullanabilirsin 
            // veya ViewBag üzerinden önerileri gönderip ana modeli "Tüm Şarkılar" yapabilirsin.
            ViewBag.RecommendedSongs = recommendedSongs;

            return View(allSongs);
        }
    }
}
