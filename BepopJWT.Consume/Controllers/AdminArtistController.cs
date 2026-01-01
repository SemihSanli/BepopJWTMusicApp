using BepopJWT.Consume.ArtistDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminArtistController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminArtistController(IHttpClientFactory httpClientFactory)
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
                {
                    token = HttpContext.User.FindFirst("AccessToken")?.Value;
                }

                if (string.IsNullOrEmpty(token))
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("https://localhost:7092/api/Artists");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultArtistDTO>>(jsonData);
                    return View(values);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("SignIn", "Auth");

                ViewBag.Error = $"API isteği başarısız oldu. Durum kodu: {response.StatusCode}";
                return View(new List<ResultArtistDTO>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçıları çekerken hata oluştu: " + ex.Message;
                return View(new List<ResultArtistDTO>());
            }
        }

        // 2️⃣ CREATE (GET)
        [HttpGet]
        public IActionResult CreateArtist() => View();

        // 2️⃣ CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> CreateArtist(CreateArtistDTO dto)
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

                var formData = new MultipartFormDataContent
                {
                    { new StringContent(dto.Name), "Name" },
                    { new StringContent(dto.Bio), "Bio" }
                };

                if (dto.ImageUrl != null)
                {
                    var streamContent = new StreamContent(dto.ImageUrl.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ImageUrl.ContentType);
                    formData.Add(streamContent, "ImageUrl", dto.ImageUrl.FileName);
                }

                var response = await client.PostAsync("https://localhost:7092/api/Artists", formData);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçı eklerken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }

        // 3️⃣ DELETE
        public async Task<IActionResult> DeleteArtist(int id)
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
                var response = await client.DeleteAsync($"https://localhost:7092/api/Artists/{id}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçı silinirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (GET)
        [HttpGet]
        public async Task<IActionResult> UpdateArtist(int id)
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
                var response = await client.GetAsync($"https://localhost:7092/api/Artists/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<UpdateArtistDTO>(jsonData);
                    return View(values);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçı güncellenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (POST)
        [HttpPost]
        public async Task<IActionResult> UpdateArtist(UpdateArtistDTO dto)
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

                var formData = new MultipartFormDataContent
                {
                    { new StringContent(dto.ArtistId.ToString()), "ArtistId" },
                    { new StringContent(dto.Name), "Name" },
                    { new StringContent(dto.Bio), "Bio" }
                };

                if (dto.ImageUrl != null)
                {
                    var streamContent = new StreamContent(dto.ImageUrl.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ImageUrl.ContentType);
                    formData.Add(streamContent, "ImageUrl", dto.ImageUrl.FileName);
                }

                var response = await client.PutAsync("https://localhost:7092/api/Artists", formData);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Sanatçı güncellenirken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }
    }
}
