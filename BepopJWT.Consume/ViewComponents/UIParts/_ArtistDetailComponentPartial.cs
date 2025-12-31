
using BepopJWT.Consume.ArtistDTOs;
using BepopJWT.Consume.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _ArtistDetailComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _ArtistDetailComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        // Parametre olarak ID alıyor
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();
            var token = HttpContext.User.FindFirst("AccessToken")?.Value;
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // API'den Sanatçı + Şarkıları çek
            var response = await client.GetAsync($"https://localhost:7209/api/Artists/GetArtistDetail/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetArtistDetailsDTO>(jsonData);
                return View(values);
            }

            return View(new GetArtistDetailsDTO());
        }
    }
}
