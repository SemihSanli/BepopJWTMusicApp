using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class PaymentManager : IPaymentService
    {
        public Task TAddAsync(Payment entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Payment>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Payment> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}
