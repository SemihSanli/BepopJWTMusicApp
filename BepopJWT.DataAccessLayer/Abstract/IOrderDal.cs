using BepopJWT.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DataAccessLayer.Abstract
{
    public interface IOrderDal:IGenericDal<Order>
    {
        Task<Order> GetByConversationId(string conversationId);
    }
}
