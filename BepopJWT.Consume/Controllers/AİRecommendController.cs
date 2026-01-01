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
          
            return View();
        }

  
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

              
                ViewBag.SelectedMood = mood;

               
                return View(values);
            }

       
            ViewBag.Error = "Şarkı bulunamadı.";
            return View();
        }
    }
}
