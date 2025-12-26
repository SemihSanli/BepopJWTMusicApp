using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.PaymentDTOs;
using BepopJWT.EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BepopJWT.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // 1. Ödeme Sayfasına Gitme (Start Payment)
        // Frontend'den buraya {UserId: 5, PackageId: 2} gibi veri gelecek.
        [HttpPost("initialize")]
        public async Task<IActionResult> InitializePayment([FromBody] PaymentRequestDTO paymentRequestDto)
        {
            try
            {
                // Manager katmanındaki metodumuzu çağırıyoruz.
                // O da bize Iyzico'nun ödeme sayfası linkini (URL) dönüyor.
                var paymentUrl = await _paymentService.InitializePayment(paymentRequestDto);

                // Frontend'e diyoruz ki: "Al bu linki, kullanıcıyı buraya yönlendir."
                return Ok(new { Url = paymentUrl });
            }
            catch (Exception ex)
            {
                // Bir hata olursa (Level yetmezse vs.) hatayı dönüyoruz.
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 2. Iyzico'dan Dönüş (Callback)
        // Kullanıcı parayı ödeyince Iyzico burayı tetikleyecek (Biz MVC üzerinden yönlendireceğiz ama API ucumuz hazır olsun)
        [HttpPost("callback")]
        public async Task<IActionResult> ProcessCallback([FromBody] IyzicoCallbackDTO callbackDto)
        {
            try
            {
                var result = await _paymentService.ProcessCallBack(callbackDto);

                if (result == "SUCCESS")
                {
                    return Ok(new { Message = "Ödeme Başarılı, Paket Tanımlandı!" });
                }
                else
                {
                    return BadRequest(new { Message = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}