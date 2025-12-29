using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.ViewComponents.UIParts
{
    public class _GenresComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
