using BepopJWT.Consume.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // 1️⃣ Token kontrolü: Cookie veya Claim
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                {
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;
                }

                // 2️⃣ Token yoksa login sayfasına yönlendir
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("SignIn", "Auth"); // Login sayfası
                }

                // 3️⃣ Token varsa header'a ekle
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // 4️⃣ API isteği
                var responseMessage = await client.GetAsync("https://localhost:7209/api/Users/getAllUsers");

                // 5️⃣ Başarılıysa veriyi çek
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultUserDTOs>>(jsonData);
                    return View(values);
                }

                // 6️⃣ API 401 dönerse login sayfasına yönlendir
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("SignIn", "Auth");
                }

                // 7️⃣ Diğer durumlarda boş liste dön
                ViewBag.Error = $"API isteği başarısız oldu. Durum kodu: {responseMessage.StatusCode}";
                return View(new List<ResultUserDTOs>());
            }
            catch (Exception ex)
            {
                // Hata varsa loglayabilir veya kullanıcıya mesaj gösterebilirsin
                ViewBag.Error = "Kullanıcıları çekerken bir hata oluştu: " + ex.Message;
                return View(new List<ResultUserDTOs>());
            }
        }
    }
}
