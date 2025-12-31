using BepopJWT.EntityLayer.Enums;
using Newtonsoft.Json;

namespace BepopJWT.Consume.DTOs.OrderDTOs
{
    public class ResultOrderDTO
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
      
        public decimal? PaidAmount { get; set; }
       
    
        public decimal TotalPrice { get; set; }
    }
}
