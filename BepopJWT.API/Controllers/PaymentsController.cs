using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DTOLayer.PaymentDTOs;
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

     
        [HttpPost("initialize")]
        public async Task<IActionResult> InitializePayment([FromBody] PaymentRequestDTO paymentRequestDto)
        {
            try
            {
              
                var paymentUrl = await _paymentService.InitializePayment(paymentRequestDto);

                
                return Ok(new { Url = paymentUrl });
            }
            catch (Exception ex)
            {
                // Bir hata olursa (Level yetmezse vs.) hatayı dönüyoruz.
                return BadRequest(new { Message = ex.Message });
            }
        }

       
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