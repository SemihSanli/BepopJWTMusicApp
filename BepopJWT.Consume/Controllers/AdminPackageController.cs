using BepopJWT.Consume.PackageDTOs; // DTO namespace'ini kontrol et
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace BepopJWT.Consume.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPackageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminPackageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 1️⃣ LIST (READ)
        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("https://localhost:7902/api/Packages");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultPackageDTO>>(jsonData);
                    return View(values);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("SignIn", "Auth");

                ViewBag.Error = $"API isteği başarısız oldu. Durum kodu: {response.StatusCode}";
                return View(new List<ResultPackageDTO>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Paketleri çekerken hata oluştu: " + ex.Message;
                return View(new List<ResultPackageDTO>());
            }
        }

        // 2️⃣ CREATE (GET)
        [HttpGet]
        public IActionResult CreatePackage() => View();

        // 2️⃣ CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> CreatePackage(CreatePackageDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7902/api/Packages", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Paket eklerken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }

        // 3️⃣ DELETE
        public async Task<IActionResult> DeletePackage(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.DeleteAsync($"https://localhost:7902/api/Packages/{id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Paket silinirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (GET)
        [HttpGet]
        public async Task<IActionResult> UpdatePackage(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"https://localhost:7902/api/Packages/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<UpdatePackageDTO>(jsonData);
                    return View(values);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Paket güncellenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (POST)
        [HttpPost]
        public async Task<IActionResult> UpdatePackage(UpdatePackageDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var jsonData = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PutAsync("https://localhost:7902/api/Packages", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Paket güncellenirken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }
    }
}
