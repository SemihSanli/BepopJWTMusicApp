namespace BepopJWT.Consume.DTOs.PaymentDTOs
{
    public class UserForPaymentDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int? PackageId { get; set; }
    }
}
