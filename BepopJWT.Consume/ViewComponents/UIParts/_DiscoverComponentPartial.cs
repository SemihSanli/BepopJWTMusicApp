
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

        public _DiscoverComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var token = HttpContext.User.FindFirst("AccessToken")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync("https://localhost:7209/api/Songs");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var songs = JsonConvert.DeserializeObject<List<ResultSongWithArtists>>(jsonData);
                return View(songs);
            }

            return View(new List<ResultSongWithArtists>());
        }
    }
}
