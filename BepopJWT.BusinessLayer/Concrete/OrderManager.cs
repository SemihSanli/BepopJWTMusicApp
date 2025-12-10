using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        public Task TAddAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> TGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
