using BepopJWT.Consume.CategoryDTOs; // DTO namespace'ini eklemeyi unutma
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminCategoryController(IHttpClientFactory httpClientFactory)
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

                var response = await client.GetAsync("https://localhost:7209/api/Categories");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultCategoryDTO>>(jsonData);
                    return View(values);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("SignIn", "Auth");

                ViewBag.Error = $"API isteği başarısız oldu. Durum kodu: {response.StatusCode}";
                return View(new List<ResultCategoryDTO>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Kategorileri çekerken hata oluştu: " + ex.Message;
                return View(new List<ResultCategoryDTO>());
            }
        }

        // 2️⃣ CREATE (GET)
        [HttpGet]
        public IActionResult CreateCategory() => View();

        // 2️⃣ CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO dto)
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

                var response = await client.PostAsync("https://localhost:7209/api/Categories", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Kategori eklerken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }

        // 3️⃣ DELETE
        public async Task<IActionResult> DeleteCategory(int id)
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

                await client.DeleteAsync($"https://localhost:7209/api/Categories/{id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Kategori silinirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (GET)
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
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

                var response = await client.GetAsync($"https://localhost:7209/api/Categories/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<UpdateCategoryDTO>(jsonData);
                    return View(values);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Kategori güncellenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (POST)
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO dto)
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

                var response = await client.PutAsync("https://localhost:7209/api/Categories", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Kategori güncellenirken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }
    }
}
