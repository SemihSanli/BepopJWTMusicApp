using BepopJWT.Consume.Models;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.Consume.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Page(int id)
        {
           
            var errorModel = new ErrorPageModel();

            switch (id)
            {
                case 404:
                    errorModel.Code = "404";
                    errorModel.Title = "Sayfa Bulunamadı";
                    errorModel.Message = "Aradığın ritmi bulamadık. Bu sayfa silinmiş veya hiç var olmamış olabilir.";
                    break;
                case 403:
                    errorModel.Code = "403";
                    errorModel.Title = "Erişim Reddedildi";
                    errorModel.Message = "Dur bakalım! Bu alana girmek için VIP (Admin) biletine ihtiyacın var.";
                    break;
                case 401:
                    errorModel.Code = "401";
                    errorModel.Title = "Yetkisiz Giriş";
                    errorModel.Message = "Kim olduğunu bilmiyoruz. Lütfen önce giriş yap.";
                    break;
                case 500:
                    errorModel.Code = "500";
                    errorModel.Title = "Sunucu Hatası";
                    errorModel.Message = "Bizim tarafta teller koptu. Teknik ekip üzerinde çalışıyor.";
                    break;
                default:
                    errorModel.Code = id.ToString();
                    errorModel.Title = "Bir Hata Oluştu";
                    errorModel.Message = "Beklenmedik bir şeyler oldu.";
                    break;
            }

            return View(errorModel);
        }
    }
}
