using BepopJWT.DTOLayer.AuthDTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using BepopJWT.DTOLayer.TokenDTOs;
using BepopJWT.Consume.DTOs.TokenDTOs;


namespace BepopJWT.Consume.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterDTO registerDto)
        {
            var client = _httpClientFactory.CreateClient("BepopApi");

         
            var multipartContent = new MultipartFormDataContent();

           //FormFile olduğu için MultipartContent var.
            multipartContent.Add(new StringContent(registerDto.Username ?? ""), "Username");
            multipartContent.Add(new StringContent(registerDto.FullName ?? ""), "FullName");
            multipartContent.Add(new StringContent(registerDto.Email ?? ""), "Email");
            multipartContent.Add(new StringContent(registerDto.PasswordHash ?? ""), "PasswordHash");
            multipartContent.Add(new StringContent("Member"), "Role"); // Varsayılan rol

          
            if (registerDto.ProfileImage != null)
            {
                var imageStream = registerDto.ProfileImage.OpenReadStream();
                var streamContent = new StreamContent(imageStream);
              
                multipartContent.Add(streamContent, "ProfileImage", registerDto.ProfileImage.FileName);
            }

           
            var response = await client.PostAsync("https://localhost:7209/api/Auths/register", multipartContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SignIn", "Auth",new {email = registerDto.Email});
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ViewBag.Error = "Kayıt başarısız: " + errorMsg;
            return View();
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDTO loginDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7209/api/Auths/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                // 4. API'den gelen cevabı (Token) oku
                var jsonData = await response.Content.ReadAsStringAsync();
                var tokenModel = JsonConvert.DeserializeObject<TokenResponseDTO>(jsonData);

                if (tokenModel != null && !string.IsNullOrEmpty(tokenModel.Token))
                {
                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(tokenModel.Token);

                    // Claims listesini oluşturuyoruz
                    var claims = new List<Claim>();
                    claims.AddRange(jwtToken.Claims);

                  
                    claims.Add(new Claim("AccessToken", tokenModel.Token));

               
                    HttpContext.Session.SetString("JWToken", tokenModel.Token);

                   
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = jwtToken.ValidTo
                    };

                 
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                  
                    var packageIdClaim = claims.FirstOrDefault(x => x.Type == "PackageId")?.Value;

                
                    if (string.IsNullOrEmpty(packageIdClaim) || packageIdClaim == "0")
                    {
                        return RedirectToAction("Index", "Packages");
                    }

                   
                    return RedirectToAction("Discovery", "Default");
                }
            }

        
            ViewBag.Error = "Email veya şifre hatalı!";
            return View(loginDto);
        }
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
           
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Auth");
        }
    }
}
