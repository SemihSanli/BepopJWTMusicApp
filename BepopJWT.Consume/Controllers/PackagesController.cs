using BepopJWT.Consume.Helpers;
using BepopJWT.Consume.PackageDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiClientHelper _apiClientHelper;
        public PackagesController(IHttpClientFactory httpClientFactory, ApiClientHelper apiClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _apiClientHelper = apiClientHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            
            ViewBag.UserEmail = email;

            var client = _apiClientHelper.GetClient();
            var response = _httpClientFactory.CreateClient();
            var packages = await client.GetFromJsonAsync<List<ResultPackageDTO>>("https://localhost:7209/api/Package");

            return View(packages);
        }
    }
}