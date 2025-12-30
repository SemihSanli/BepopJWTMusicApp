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
        public IActionResult ArtistDetail(int id)
        {
            ViewBag.ArtistId = id;
            return View();
        }
        public IActionResult Genres(int categoryId=0)
        {
            ViewBag.CategoryId = categoryId;
            return View();
        }
    }
}
