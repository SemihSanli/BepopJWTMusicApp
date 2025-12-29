using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _MainPageComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
