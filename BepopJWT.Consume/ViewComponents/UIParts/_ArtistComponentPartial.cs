using BepopJWT.Consume.ArtistDTOs; // <--- DTO Layer Namespace'i ÖNEMLİ!
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _ArtistComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ArtistComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Token'ı Claim'den al (Giriş yapmış kullanıcı)
            var token = HttpContext.User.FindFirst("AccessToken")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 2. API'ye İstek At (Endpoint ismini kontrol et: api/Artists)
            var response = await client.GetAsync("https://localhost:7209/api/Artists");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();

                // 3. JSON'ı DTO Listesine Çevir
                var artists = JsonConvert.DeserializeObject<List<ResultArtistDTO>>(jsonData);
                return View(artists);
            }

            // Hata varsa veya veri yoksa boş liste dön
            return View(new List<ResultArtistDTO>());
        }
    }
}