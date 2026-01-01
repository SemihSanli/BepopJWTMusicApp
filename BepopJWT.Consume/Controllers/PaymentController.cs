using BepopJWT.Consume.DTOs.PaymentDTOs;
using BepopJWT.Consume.PaymentDTOs; // Senin DTO'ların burada
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using BepopJWT.Consume.Helpers;

namespace BepopJWT.Consume.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;



        public PaymentController(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }




      

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int packageId) // 'email' parametresini kaldırdık
        {
           
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Login"); 
            }

           
            string email = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            

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
            var client = _apiClientHelper.GetClient();
            var response = _httpClientFactory.CreateClient();
            string apiUrl = "https://localhost:7209";

            try
            {
               
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
                   
                    return Content($"⚠️ İŞLEM BAŞARISIZ: {responseStr}");
                }

                var result = JsonConvert.DeserializeObject<ResponsePaymentDTO>(responseStr);

                if (string.IsNullOrEmpty(result.Url))
                {
                    return Content("❌ HATA: Iyzico ödeme sayfası oluşturulamadı.");
                }

            
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

               
                var currentEmail = User.Identity.Name;

                if (!string.IsNullOrEmpty(currentEmail))
                {
                  
                    var userResponse = await client.GetAsync($"{apiUrl}/api/Users/getbyemail?email={currentEmail}");

                    if (userResponse.IsSuccessStatusCode)
                    {
                        var userJson = await userResponse.Content.ReadAsStringAsync();
                        var updatedUser = JsonConvert.DeserializeObject<UserForPaymentDTO>(userJson);

                     

                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, updatedUser.Email),
                    new Claim(ClaimTypes.NameIdentifier, updatedUser.UserId.ToString()),
                 
                    new Claim("PackageId", updatedUser.PackageId.ToString()) 
                    
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true, 
                            ExpiresUtc = DateTime.UtcNow.AddDays(7)
                        };

                       
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                    
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
          
            ViewBag.Status = TempData["PaymentStatus"];
            ViewBag.Message = TempData["Message"];

            if (ViewBag.Status == null)
            {
                return RedirectToAction("Index", "Packages"); 
            }

            return View(); 

        }
    }
}