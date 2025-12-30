using BepopJWT.Consume.DTOs.PaymentDTOs;
using BepopJWT.Consume.PaymentDTOs; // Senin DTO'ların burada
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace BepopJWT.Consume.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }




        // Dosya: BepopJWT.Consume/Controllers/PaymentController.cs

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int packageId) // 'email' parametresini kaldırdık
        {
            // 1. Kullanıcı Giriş Yapmış mı Kontrolü
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Login"); // Giriş yapmamışsa login sayfasına at
            }

            // 2. Email'i Cookie/Token içindeki Claim'den çekiyoruz
            // (Login olurken ClaimTypes.Email veya ClaimTypes.Name olarak kaydettiysen ona göre değiştir)
            string email = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            // Veya ClaimTypes kullanıyorsan: User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                email = User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
            }
            if (string.IsNullOrEmpty(email) && User.Identity.Name.Contains("@"))
            {
                email = User.Identity.Name;
            }
            if (string.IsNullOrEmpty(email))
            {
                return Content($"❌ HATA: Giriş yapmış kullanıcının Email bilgisi Cookie/Token içinde bulunamadı! \n" +
                               $"Görünen Kullanıcı Adı: {User.Identity.Name} \n" +
                               $"Lütfen LoginController tarafında Claims eklerken 'ClaimTypes.Email' eklediğinden emin ol.");
            }
            var client = _httpClientFactory.CreateClient();
            string apiUrl = "https://localhost:7209";

            try
            {
                // 3. API'ye Kullanıcıyı Sor (Email ile ID'sini bulmak için)
                var userResponse = await client.GetAsync($"{apiUrl}/api/Users/getbyemail?email={email}");

                if (!userResponse.IsSuccessStatusCode)
                {
                    return Content($"❌ HATA: Kullanıcı bilgileri API'den çekilemedi. Durum: {userResponse.StatusCode}");
                }

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userDetail = JsonConvert.DeserializeObject<UserForPaymentDTO>(userJson);

                if (userDetail == null || userDetail.UserId == 0)
                {
                    return Content("❌ HATA: Kullanıcı bulundu ama ID geçersiz.");
                }

                // 4. Ödemeyi Başlat
                var paymentRequest = new PaymentRequestDTO
                {
                    UserId = userDetail.UserId,
                    PackageId = packageId
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

                // Buradan dönen cevapta Manager katmanındaki hata mesajı (Zaten sahipsiniz vs.) olabilir.
                var paymentResponse = await client.PostAsync($"{apiUrl}/api/Payments/initialize", jsonContent);
                var responseStr = await paymentResponse.Content.ReadAsStringAsync();

                if (!paymentResponse.IsSuccessStatusCode)
                {
                    // Manager'daki "Zaten bu pakete sahipsiniz" hatası burada yakalanır
                    // İstersen burada TempData ile hata mesajını View'e taşıyıp kullanıcıya şık bir uyarı gösterebilirsin.
                    return Content($"⚠️ İŞLEM BAŞARISIZ: {responseStr}");
                }

                var result = JsonConvert.DeserializeObject<ResponsePaymentDTO>(responseStr);

                if (string.IsNullOrEmpty(result.Url))
                {
                    return Content("❌ HATA: Iyzico ödeme sayfası oluşturulamadı.");
                }

                // HATA YOKSA IYZICO'YA YÖNLENDİR
                return Redirect(result.Url);
            }
            catch (Exception ex)
            {
                return Content($"🔥 SİSTEM HATASI: {ex.Message}");
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CallBack(IyzicoCallbackDTO iyzicoCallbackDto)
        {
            // 1. Token Kontrolü
            if (string.IsNullOrEmpty(iyzicoCallbackDto.Token))
            {
                return Content("❌ HATA: Iyzico'dan Token gelmedi!");
            }

            var client = _httpClientFactory.CreateClient();
            string apiUrl = "https://localhost:7209";

            // 2. API'ye Ödeme Onayı Gönder
            var jsonContent = new StringContent(JsonConvert.SerializeObject(iyzicoCallbackDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{apiUrl}/api/Payments/callback", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                // --- BURASI YENİ EKLENEN KISIM: KİMLİK TAZELEME ---

                // A) Şu anki kullanıcının Email'ini al (Hala eski cookie'den okuyoruz)
                var currentEmail = User.Identity.Name; // veya User.Claims'den e-postayı çek

                if (!string.IsNullOrEmpty(currentEmail))
                {
                    // B) API'den kullanıcının GÜNCEL halini çek (Artık PackageId dolu gelecek)
                    var userResponse = await client.GetAsync($"{apiUrl}/api/Users/getbyemail?email={currentEmail}");

                    if (userResponse.IsSuccessStatusCode)
                    {
                        var userJson = await userResponse.Content.ReadAsStringAsync();
                        var updatedUser = JsonConvert.DeserializeObject<UserForPaymentDTO>(userJson);

                        // C) Eğer MVC tarafında JWT'yi Session'da tutuyorsan, API'den yeni bir JWT de istemen gerekebilir.
                        // Ancak sadece Cookie Claims üzerinden yetki kontrolü yapıyorsan aşağısı yeterlidir:

                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, updatedUser.Email),
                    new Claim(ClaimTypes.NameIdentifier, updatedUser.UserId.ToString()),
                    // DİKKAT: Artık yeni PackageId'yi claim'e basıyoruz
                    new Claim("PackageId", updatedUser.PackageId.ToString()) 
                    // Varsa rolünü de tekrar ekle
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true, // Beni hatırla mantığı varsa
                            ExpiresUtc = DateTime.UtcNow.AddDays(7)
                        };

                        // D) ESKİ COOKIE'Yİ SİLİP YENİSİNİ BASIYORUZ (Re-Sign In)
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // Eğer token'ı Session'da tutuyorsan onu da güncellemen gerekebilir
                        // HttpContext.Session.SetString("JWToken", "YENİ_TOKEN_VARSA_BURAYA");
                    }
                }
                // ---------------------------------------------------

                TempData["PaymentStatus"] = "Success";
                TempData["Message"] = "Ödemeniz başarıyla alındı! Paketiniz güncellendi.";
                return RedirectToAction("Result", "Payment");
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                TempData["PaymentStatus"] = "Error";
                TempData["Message"] = "Ödeme onaylanamadı: " + errorMsg;
                return RedirectToAction("Result", "Payment");
            }
        }
        [HttpGet]
        public IActionResult Result()
        {
            // TempData'dan verileri alıp View'a gönderiyoruz
            ViewBag.Status = TempData["PaymentStatus"];
            ViewBag.Message = TempData["Message"];

            if (ViewBag.Status == null)
            {
                return RedirectToAction("Index", "Packages"); // Doğrudan linkle girmeye çalışırsa geri at
            }

            return View(); // Views/Payment/Result.cshtml

        }
    }
}