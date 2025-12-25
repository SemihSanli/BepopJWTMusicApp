using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DTOLayer.PaymentDTOs;
using BepopJWT.EntityLayer.Entities;
using BepopJWT.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class PaymentManager : IPaymentService
    {
        private readonly IPaymentDal _paymentDal;
        private readonly IUserService _userService;
        private readonly IPackageService _packageService;
        private readonly IOrderService _orderService;
        private readonly IIyzicoService _iyzicoService;
        public PaymentManager(IPaymentDal paymentDal, IUserService userService, IOrderService orderService, IIyzicoService iyzicoService, IPackageService packageService)
        {
            _paymentDal = paymentDal;
            _userService = userService;
            _orderService = orderService;
            _iyzicoService = iyzicoService;
            _packageService = packageService;
        }

        public async Task<string> InitializePayment(PaymentRequestDTO paymentRequestDto)
        {
            var user = await _userService.TGetByIdAsync(paymentRequestDto.UserId);
            var newPackage = await _packageService.TGetByIdAsync(paymentRequestDto.PackageId);

            if(user==null || newPackage == null) throw new Exception("Kullanıcı veya paket bulunamadı");

            if (user.PackageId != null && user.PackageId !=0)
            {
                var currentPackage = await _packageService.TGetByIdAsync(user.PackageId.Value);
                if(currentPackage !=null && currentPackage.PackageLevel >= newPackage.PackageLevel)
                {
                    throw new Exception("Mevcut paketin seviyesi, seçilen paketin seviyesinden yüksek veya eşit.)");
                }
            }
            var order = new Order
            {
                ConversationId = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                PackageId = newPackage.PackageId,
                PaidAmount = newPackage.Price,
                Status = OrderStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };
            await _orderService.TAddAsync(order);
            return await _iyzicoService.StartPaymentProcess(user, newPackage, order.ConversationId);

        }

        public async Task<string> ProcessCallBack(IyzicoCallbackDTO iyzicoCallbackDto)
        {
            var form = await _iyzicoService.GetPaymentResult(iyzicoCallbackDto.Token);
            if (form.PaymentStatus != "SUCCESS") return "FAILED" + form.ErrorMessage;
            var order = await _orderService.TGetByConversationId(form.ConversationId);
            if (order == null) return "ORDER_NOT_FOUND";

            if (order.Status == OrderStatus.Paid) return "ORDER_ALREADY_PAID";

            try
            {
                // A. Siparişi Güncelle (Enum ile)
                order.Status = OrderStatus.Paid;
                await _orderService.TUpdateAsync(order);

                // B. Ödeme Kaydı
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    IyzicoPaymentId = form.PaymentId,
                    ConversationId = form.ConversationId,
                    PaidPrice = Convert.ToDecimal(form.PaidPrice),
                    Status = PaymentStatus.Success, // Payment tablosundaki status int ise 1 kalabilir, enum ise onu da düzeltirsin.
                    PaymentDate = DateTime.UtcNow
                };
                await _paymentDal.AddAsync(payment);

                // C. Kullanıcı Paketini Güncelle
                var user = await _userService.TGetByIdAsync(order.UserId);
                user.PackageId = order.PackageId;
                await _userService.TUpdateAsync(user);

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                await _iyzicoService.RefundPayment(form.PaymentId, "127.0.0.1");
                return "SYSTEM_ERROR_REFUNDED: " + ex.Message;
            }
        }

        public async Task TAddAsync(Payment entity)
        {
            await _paymentDal.AddAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
          await _paymentDal.DeleteAsync(id);
        }

        public async Task<List<Payment>> TGetAllAsync()
        {
           return await _paymentDal.GetAllAsync();
        }

        public async Task<Payment> TGetByIdAsync(int id)
        {
           return await _paymentDal.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Payment entity)
        {
          await _paymentDal.UpdateAsync(entity);
        }
    }
}
