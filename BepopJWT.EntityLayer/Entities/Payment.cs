using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.EntityLayer.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;


        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string? IyzicoPaymentId { get; set; }

        public string ConversationId { get; set; }


        public decimal PaidPrice { get; set; }
        public string Currency { get; set; } = "TRY";


        public int PaymentStatus { get; set; }


        public string? ErrorMessage { get; set; }
    }
}
