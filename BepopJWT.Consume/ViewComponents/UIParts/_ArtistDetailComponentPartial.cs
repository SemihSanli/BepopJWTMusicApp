
using BepopJWT.Consume.ArtistDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _ArtistDetailComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ArtistDetailComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Parametre olarak ID alıyor
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
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
