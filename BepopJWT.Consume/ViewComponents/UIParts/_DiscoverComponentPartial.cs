using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _DiscoverComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
