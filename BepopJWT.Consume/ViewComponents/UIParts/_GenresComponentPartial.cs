using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using BepopJWT.Consume.SongDTOs;
using BepopJWT.Consume.Helpers;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _GenresComponentPartial:ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public _GenresComponentPartial(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId = 0)
        {
            var client = _apiClientHelper.GetClient();
            var client2 = _httpClientFactory.CreateClient();

            var token = HttpContext.User.FindFirst("AccessToken")?.Value;
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API zaten tüm şarkıları kategorileri ile birlikte döndürüyor
            string url = "https://localhost:7209/api/Songs/getsongswithcategory";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var allSongs = JsonConvert.DeserializeObject<List<ResultSongWithArtists>>(jsonData);

                // Eğer kategori seçilmişse client-side filtre uygula
                if (categoryId != 0)
                    allSongs = allSongs.Where(s => s.CategoryId == categoryId).ToList();

                return View(allSongs);
            }

            return View(new List<ResultSongWithArtists>());
        }

    }
}
