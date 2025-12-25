using BepopJWT.DTOLayer.PaymentDTOs;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IPaymentService:IGenericService<Payment>
    {
        Task<string> InitializePayment(PaymentRequestDTO paymentRequestDto);
        Task<string> ProcessCallBack(IyzicoCallbackDTO iyzicoCallbackDto);
    }
}
