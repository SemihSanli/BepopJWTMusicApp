using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _ArtistComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
