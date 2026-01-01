using BepopJWT.Consume.ArtistDTOs;
using BepopJWT.Consume.DTOs.OrderDTOs;
using BepopJWT.Consume.PackageDTOs;
using BepopJWT.Consume.SongDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BepopJWT.Consume.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminDefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Token Kontrolü
            var token = HttpContext.Request.Cookies["AccessToken"] ?? HttpContext.User.FindFirst("AccessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("SignIn", "Auth");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string baseUrl = "https://localhost:7209/api";

            try
            {
                // 2. İstekleri Tanımlama ve Paralel Başlatma
                var artistTask = client.GetAsync($"{baseUrl}/Artists");
                var songTask = client.GetAsync($"{baseUrl}/Songs/getsongwitharist");
                var packageTask = client.GetAsync($"{baseUrl}/Packages");
                var orderTask = client.GetAsync($"{baseUrl}/Orders");

                // Tüm API yanıtlarının gelmesini bekle
                await Task.WhenAll(artistTask, songTask, packageTask, orderTask);

                // 3. Yanıtları İşleme (HandleResponse içinde await kullanarak veri kaybını önle)
                // ViewBag atamalarını HandleResponse sonucuna göre yapıyoruz.
                ViewBag.ArtistList = await HandleResponse<List<ResultArtistDTO>>(await artistTask);
                ViewBag.SongWithArtist = await HandleResponse<List<ResultSongWithArtists>>(await songTask);
                ViewBag.PackageCount = await HandleResponse<List<ResultPackageDTO>>(await packageTask);
                ViewBag.OrderCount = await HandleResponse<List<ResultOrderDTO>>(await orderTask);
            }
            catch (Exception ex)
            {
                // Genel bir hata oluşursa listeleri boş başlat ki View çökmesin
                ViewBag.ArtistList = new List<ResultArtistDTO>();
                ViewBag.SongWithArtist = new List<ResultSongWithArtists>();
                ViewBag.PackageCount = new List<ResultPackageDTO>();
                ViewBag.OrderCount = new List<ResultOrderDTO>();

                // Hata mesajını debug konsoluna yazdır
                System.Diagnostics.Debug.WriteLine($"Dashboard Hatası: {ex.Message}");
            }

            return View();
        }

        /// <summary>
        /// JSON verisini çözen ve harf duyarlılığını (Case Sensitivity) ortadan kaldıran yardımcı metod.
        /// </summary>
        private async Task<T> HandleResponse<T>(HttpResponseMessage response) where T : new()
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                try
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // ÖNEMLİ: JSON isimlendirme farklarını (Büyük/Küçük harf) tolere etmek için ayarlar
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    return JsonConvert.DeserializeObject<T>(jsonData, settings) ?? new T();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"JSON Deserialization Hatası: {ex.Message}");
                    return new T();
                }
            }
            return new T();
        }
    }
}