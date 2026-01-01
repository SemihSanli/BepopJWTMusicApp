using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Options.IyzicoOptions;
using BepopJWT.DTOLayer.PaymentDTOs;
using BepopJWT.EntityLayer.Entities;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class IyzicoManager : IIyzicoService
    {
        private readonly IyzicoSettings _iyzicoSettings;
        private readonly PaymentSettings _paymentSettings;

        public IyzicoManager(IOptions<PaymentSettings> paymentSettingsOptions, IOptions<IyzicoSettings> iyzicoSettingsOptions)
        {
            
            _paymentSettings = paymentSettingsOptions.Value;
            _iyzicoSettings = iyzicoSettingsOptions.Value;
        }

        private Iyzipay.Options GetOptions()
        {
            var options = new Iyzipay.Options();
            options.ApiKey = _iyzicoSettings.ApiKey;
            options.SecretKey = _iyzicoSettings.SecretKey;
            options.BaseUrl = _iyzicoSettings.BaseUrl;
            return options;

        }

        public Task<CheckoutForm> GetPaymentResult(string token)
        {
           var request = new RetrieveCheckoutFormRequest {
               Token = token
           };
            return CheckoutForm.Retrieve(request, GetOptions());
        }

        public async Task<bool> RefundPayment(string paymentId, string ip)
        {
            var request = new CreateCancelRequest();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Locale = Locale.TR.ToString();
            request.PaymentId = paymentId; 
            request.Ip = ip ?? "127.0.0.1"; 
            request.Reason = "other";
            request.Description = "Sistem hatası nedeniyle otomatik iptal (Rollback)";
          
            {
                var cancel = await Cancel.Create(request, GetOptions());

              
                if (cancel.Status == "success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
        }

        public async Task<string> StartPaymentProcess(User user, Package package, string conversationId)
        {

            // --- 1. AD VE SOYAD AYRIŞTIRMA MANTIĞI ---
            string buyerName = user.Username; //Bu kısım benim yedeğim aslında. Eğer name ve surname boşsa name yerine username surname yerine de "Uye" yazıyor.
            string buyerSurname = "Uye";      


            //Bu kısım normalde gerekli değil fakat ben Db'de fullName tuttuğum için ve iyzico da surname kısmını zorunlu kıldığı için böyle bir bölme işlemine girdim.
            if (!string.IsNullOrWhiteSpace(user.FullName))
            {
                string tempName = user.FullName.Trim(); //burdaki trim, kullanıcı yanlışlıkla boşluk bırakarak yazarsa o boşlukları düzeltiyor
                int lastSpaceIndex = tempName.LastIndexOf(' '); // Burada en son boşluğu arıyor, "Ad Soyad" gibi

                if (lastSpaceIndex > 0)
                {
                    //0'cı karakterden başlayıp son boşluğa kadar alıyor.
                    buyerName = tempName.Substring(0, lastSpaceIndex);

                    // Son boşluktan sonraki kısmı alıyor burası da.
                    buyerSurname = tempName.Substring(lastSpaceIndex + 1);
                }
                else
                {
                    // Hiç boşluk yoksa (Tek kelime ise), hata vermesin diye ikisine de aynı şeyi yazıyoruz.
                    buyerName = tempName;
                    buyerSurname = tempName;
                }
            }
            // -----------------------------------------


            var request = new CreateCheckoutFormInitializeRequest();
            request.CallbackUrl = _paymentSettings.CallbackUrl;

            request.ConversationId = conversationId;
            request.Price = package.Price.ToString("F2",CultureInfo.InvariantCulture);
            request.PaidPrice=package.Price.ToString("F2", CultureInfo.InvariantCulture);
            request.Currency = Currency.TRY.ToString();
            request.BasketId = conversationId;
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.EnabledInstallments = new List<int> { 1 };

            //Alıcı bilgilerim

            var buyer = new Buyer
            {
                Id = user.UserId.ToString(),
                Name = buyerName,
                Surname = buyerSurname,
                GsmNumber = "+905555555555", //buraya canlı olarak user entitysine ekleyebilirz fakat ben yapmadım sandbox olduğu için
                Email = user.Email,
                IdentityNumber = "11111111111", //Sandbox ortamında bu geçerlidir fakat canlıda TC Kimlik no girilmeli
                RegistrationAddress = "İzmir", //Bu da db'den çekilebilir
                Ip = "127.0.0.1",
                City = "İzmir",
                Country="Turkey"
            };
            request.Buyer = buyer;

            //Fatura adresi
            var address = new Address
            {
                ContactName = user.FullName,
                City = "İzmir",
                Country = "Turkey",
                Description = "İzmir Adres",
                ZipCode = "35000"
            };
            request.BillingAddress = address;
            request.ShippingAddress = address;

            //Sepet içeriğimiz

            var basketItems = new List<BasketItem>
            {
                new BasketItem
                {
                    Id = package.PackageId.ToString(),
                    Name = package.PackageTitle,
                    Category1 = "Package",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = package.Price.ToString("F2",CultureInfo.InvariantCulture)
                }
            };
            request.BasketItems = basketItems;

            var form = await CheckoutFormInitialize.Create(request, GetOptions());

            if(form.Status == "success")
            {
                return form.PaymentPageUrl;
            }
            throw new Exception($"Iyzico başlama hatası : {form.ErrorMessage} - Code : {form.ErrorCode}");
        }
    }
}
