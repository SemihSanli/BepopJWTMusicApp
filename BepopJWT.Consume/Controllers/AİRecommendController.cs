using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using BepopJWT.Consume.SongDTOs;
using Microsoft.AspNetCore.Authorization;

namespace BepopJWT.Consume.Controllers
{
    [Authorize]
    public class AİRecommendController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AİRecommendController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public IActionResult SongRecommendation()
        {
            // Sayfaya boş model gönderiyoruz ki "Input Ekranı" açılsın
            return View();
        }

        // 2. POST: Kullanıcı butona basınca burası çalışır (Sonuç ekranı gelir)
        [HttpPost]
        public async Task<IActionResult> SongRecommendation(string mood)
        {
            // Token kontrolü
            var token = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token)) token = HttpContext.User.FindFirst("AccessToken")?.Value;

            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Login");

            // API İşlemleri
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"https://localhost:7209/api/Songs/recommend?mood={mood}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultSongWithArtists>>(jsonData);

                // Seçilen modu ekranda göstermek için saklıyoruz
                ViewBag.SelectedMood = mood;

                // Veriyi sayfaya geri gönderiyoruz
                return View(values);
            }

            // Hata varsa yine boş sayfaya dönsün ama hata mesajı ile
            ViewBag.Error = "Şarkı bulunamadı.";
            return View();
        }
    }
}
