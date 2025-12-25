using BepopJWT.DTOLayer.PaymentDTOs;
using BepopJWT.EntityLayer.Entities;
using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Abstract
{
    public interface IIyzicoService
    {
        Task<string> StartPaymentProcess(User user, Package package, string conversationId);
        Task<CheckoutForm> GetPaymentResult(string token);

        Task<bool> RefundPayment(string paymentId, string ip);
    }
}
