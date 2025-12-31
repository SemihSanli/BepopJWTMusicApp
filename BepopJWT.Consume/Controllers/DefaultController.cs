using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace BepopJWT.Consume.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        [AllowAnonymous]
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
        public IActionResult Playlist()
        {
            
            return View();
        }
        public IActionResult PlaylistDetail(int id)
        {
            ViewBag.PlaylistId = id;
            return View();
        }
       
        
    }
}
