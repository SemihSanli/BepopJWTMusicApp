using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _ArtistDetailComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
