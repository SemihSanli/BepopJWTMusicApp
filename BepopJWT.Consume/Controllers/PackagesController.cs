using BepopJWT.Consume.PackageDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PackagesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            
            ViewBag.UserEmail = email;

            
            var client = _httpClientFactory.CreateClient();
            var packages = await client.GetFromJsonAsync<List<ResultPackageDTO>>("https://localhost:7209/api/Package");

            return View(packages);
        }
    }
}