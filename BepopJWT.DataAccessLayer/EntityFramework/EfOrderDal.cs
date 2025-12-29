using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.Context;
using BepopJWT.DataAccessLayer.Repositories;
using BepopJWT.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DataAccessLayer.EntityFramework
{
    public class EfOrderDal:GenericRepository<Order>,IOrderDal
    {
        private readonly AppDbContext _appDbContext;
        public EfOrderDal(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Order> GetByConversationId(string conversationId)
        {
            var value = await _appDbContext.Orders
          .FirstOrDefaultAsync(o => o.ConversationId.Trim().ToLower() == conversationId.Trim().ToLower());
            return value;
        }
    }
}
