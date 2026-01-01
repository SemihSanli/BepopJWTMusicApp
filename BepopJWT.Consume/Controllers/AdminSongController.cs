using BepopJWT.Consume.SongDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminSongController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSongController(IHttpClientFactory httpClientFactory)
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
                    return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("https://localhost:7902/api/Songs");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<List<ResultSongWithArtists>>(jsonData);
                    return View(values);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("SignIn", "Auth");

                ViewBag.Error = $"API isteği başarısız oldu. Durum kodu: {response.StatusCode}";
                return View(new List<ResultSongWithArtists>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Şarkıları çekerken hata oluştu: " + ex.Message;
                return View(new List<ResultSongWithArtists>());
            }
        }

        // 2️⃣ CREATE (GET)
        [HttpGet]
        public async Task<IActionResult> CreateSong()
        {
            // Burada ViewBag ile Artist ve Category listesini doldurabilirsin
            return View();
        }

        // 2️⃣ CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> CreateSong(CreateSongDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token)) return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var formData = new MultipartFormDataContent
                {
                    { new StringContent(dto.SongTitle), "SongTitle" },
                    { new StringContent(dto.CategoryId.ToString()), "CategoryId" },
                    { new StringContent(dto.ArtistId.ToString()), "ArtistId" },
                    { new StringContent(dto.MinLevelRequired.ToString()), "MinLevelRequired" }
                };

                if (dto.ImageFile != null)
                {
                    var imageStream = new StreamContent(dto.ImageFile.OpenReadStream());
                    imageStream.Headers.ContentType = new MediaTypeHeaderValue(dto.ImageFile.ContentType);
                    formData.Add(imageStream, "ImageFile", dto.ImageFile.FileName);
                }

                if (dto.SongFile != null)
                {
                    var songStream = new StreamContent(dto.SongFile.OpenReadStream());
                    songStream.Headers.ContentType = new MediaTypeHeaderValue(dto.SongFile.ContentType);
                    formData.Add(songStream, "SongFile", dto.SongFile.FileName);
                }

                var response = await client.PostAsync("https://localhost:7902/api/Songs", formData);
                if (response.IsSuccessStatusCode) return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Şarkı eklerken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }

        // 3️⃣ DELETE
        public async Task<IActionResult> DeleteSong(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token)) return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.DeleteAsync($"https://localhost:7902/api/Songs/{id}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Şarkı silinirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (GET)
        [HttpGet]
        public async Task<IActionResult> UpdateSong(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token)) return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"https://localhost:7902/api/Songs/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<UpdateSongDTO>(jsonData);
                    return View(values);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Şarkı güncellenirken hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // 4️⃣ UPDATE (POST)
        [HttpPost]
        public async Task<IActionResult> UpdateSong(UpdateSongDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token)) return RedirectToAction("SignIn", "Auth");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var formData = new MultipartFormDataContent
                {
                    { new StringContent(dto.SongId.ToString()), "SongId" },
                    { new StringContent(dto.SongTitle), "SongTitle" },
                    { new StringContent(dto.CategoryId.ToString()), "CategoryId" },
                    { new StringContent(dto.ArtistId.ToString()), "ArtistId" },
                    { new StringContent(dto.MinLevelRequired.ToString()), "MinLevelRequired" }
                };

                if (dto.ImageFile != null)
                {
                    var imageStream = new StreamContent(dto.ImageFile.OpenReadStream());
                    imageStream.Headers.ContentType = new MediaTypeHeaderValue(dto.ImageFile.ContentType);
                    formData.Add(imageStream, "ImageFile", dto.ImageFile.FileName);
                }

                if (dto.SongFile != null)
                {
                    var songStream = new StreamContent(dto.SongFile.OpenReadStream());
                    songStream.Headers.ContentType = new MediaTypeHeaderValue(dto.SongFile.ContentType);
                    formData.Add(songStream, "SongFile", dto.SongFile.FileName);
                }

                var response = await client.PutAsync("https://localhost:7902/api/Songs", formData);
                if (response.IsSuccessStatusCode) return RedirectToAction("Index");

                ViewBag.Error = $"API isteği başarısız: {response.StatusCode}";
                return View(dto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Şarkı güncellenirken hata oluştu: " + ex.Message;
                return View(dto);
            }
        }
    }
}
