using BepopJWT.EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.EntityLayer.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string ConversationId { get; set; } 
        public OrderStatus Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public ICollection<Payment> Payments { get; set; }

    }
}
