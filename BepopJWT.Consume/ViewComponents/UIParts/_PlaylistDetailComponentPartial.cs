using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using BepopJWT.Consume.PlaylistDTOs;
using BepopJWT.EntityLayer.Entities;
using BepopJWT.Consume.Helpers;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _PlaylistDetailComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _PlaylistDetailComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int playlistId)
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();
         
            var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var token = HttpContext.User.FindFirst("AccessToken")?.Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      
            var response = await client.GetAsync($"https://localhost:7209/api/Playlists/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
             
                var allPlaylists = JsonConvert.DeserializeObject<List<ResultPlaylistWithSongsDTO>>(jsonData);

              
                var selectedPlaylist = allPlaylists.FirstOrDefault(x => x.PlaylistId == playlistId);

                if (selectedPlaylist != null)
                {
                    return View(selectedPlaylist);
                }
            }
            return Content("Çalma listesi detayı bulunamadı.");
        }
    }
}
