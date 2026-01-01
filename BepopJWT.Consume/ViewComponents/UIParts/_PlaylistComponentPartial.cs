using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using BepopJWT.Consume.PlaylistDTOs;
using BepopJWT.Consume.Helpers;


namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _PlaylistComponentPartial:ViewComponent
    {
        private  readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _PlaylistComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var token = HttpContext.User.FindFirst("AccessToken")?.Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

         
            var response = await client.GetAsync($"https://localhost:7209/api/Playlists/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var playlists = JsonConvert.DeserializeObject<List<GetPlaylistByUserId>>(jsonData);
                return View(playlists);
            }
            return View(new List<GetPlaylistByUserId>());
        }
    }
}
