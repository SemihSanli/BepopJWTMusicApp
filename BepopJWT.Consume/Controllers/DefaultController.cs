using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Discovery()
        {
            return View();
        }
       public IActionResult Artist()
        {
            return View();
        }
        public IActionResult ArtistDetail()
        {
            return View();
        }
        public IActionResult Genres()
        {
            return View();
        }
    }
}
