using BepopJWT.Consume.ArtistDTOs; // <--- DTO Layer Namespace'i ÖNEMLİ!
using BepopJWT.Consume.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _ArtistComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _ArtistComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();

         
            var token = HttpContext.User.FindFirst("AccessToken")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

           
            var response = await client.GetAsync("https://localhost:7209/api/Artists");
            
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();

                
                var artists = JsonConvert.DeserializeObject<List<ResultArtistDTO>>(jsonData);
                ViewData["ArtistCount"] = artists.Count;
                return View(artists);
            }

            return View(new List<ResultArtistDTO>());
        }
    }
}