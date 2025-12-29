using BepopJWT.Consume.DTOs.PaymentDTOs;
using BepopJWT.Consume.PaymentDTOs; // Senin DTO'ların burada
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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


        

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string email, int packageId)
        {
            // 1. Gelen Veriyi Kontrol Et 
            if (string.IsNullOrEmpty(email))
            {
                return Content($"❌ HATA: Email parametresi BOŞ geldi! PackageController'da ViewBag.UserEmail atanmamış olabilir. Gelen ID: {packageId}");
            }

            var client = _httpClientFactory.CreateClient();
            string apiUrl = "https://localhost:7209"; 

            try
            {
                // 2. API'ye Kullanıcıyı Sor
                var userResponse = await client.GetAsync($"{apiUrl}/api/Users/getbyemail?email={email}");

                if (!userResponse.IsSuccessStatusCode)
                {
                    return Content($"❌ HATA: API 'Users/getbyemail' çalışmadı! Durum Kodu: {userResponse.StatusCode}. Email: {email}");
                }

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userDetail = JsonConvert.DeserializeObject<UserForPaymentDTO>(userJson);

                if (userDetail == null || userDetail.UserId == 0)
                {
                    return Content($"❌ HATA: API'den kullanıcı döndü ama UserId 0 veya null! Gelen JSON: {userJson}");
                }

                // 3. Ödemeyi Başlat
                var paymentRequest = new PaymentRequestDTO
                {
                    UserId = userDetail.UserId,
                    PackageId = packageId
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");
                var paymentResponse = await client.PostAsync($"{apiUrl}/api/Payments/initialize", jsonContent);

                var responseStr = await paymentResponse.Content.ReadAsStringAsync();

                if (!paymentResponse.IsSuccessStatusCode)
                {
                    return Content($"❌ HATA: Iyzico Başlatılamadı (API Hatası)! Detay: {responseStr}");
                }

                var result = JsonConvert.DeserializeObject<ResponsePaymentDTO>(responseStr);

                if (string.IsNullOrEmpty(result.Url))
                {
                    return Content($"❌ HATA: Iyzico URL üretmedi! Gelen JSON: {responseStr}");
                }

                // HATA YOKSA YÖNLENDİR
                return Redirect(result.Url);
            }
            catch (Exception ex)
            {
                // API KAPALIYSA VEYA BAŞKA BİR PATLAMA VARSA BURASI ÇALIŞIR
                return Content($"🔥 KRİTİK HATA: Bir şeyler fena patladı. Hata: {ex.Message} \n\n StackTrace: {ex.StackTrace}");
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CallBack(IyzicoCallbackDTO iyzicoCallbackDto)
        {
            // 1. Durum: Token kontrolü
            if (string.IsNullOrEmpty(iyzicoCallbackDto.Token))
            {
                return Content("❌ HATA: Iyzico'dan Token gelmedi!");
            }

            var client = _httpClientFactory.CreateClient();
            string apiUrl = "https://localhost:7209";

            var jsonContent = new StringContent(JsonConvert.SerializeObject(iyzicoCallbackDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{apiUrl}/api/Payments/callback", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                // Başarı Mesajı
                TempData["PaymentStatus"] = "Success";
                TempData["Message"] = "Ödemeniz başarıyla alındı! Paketiniz hesabınıza tanımlanmıştır.";
                return RedirectToAction("Result", "Payment");
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                // Hata Mesajı
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