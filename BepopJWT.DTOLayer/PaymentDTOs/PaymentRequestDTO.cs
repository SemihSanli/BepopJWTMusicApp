using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.PaymentDTOs
{
    public class PaymentRequestDTO
    {
        public int UserId { get; set; } 
        public int PackageId { get; set; } 
    }
}
