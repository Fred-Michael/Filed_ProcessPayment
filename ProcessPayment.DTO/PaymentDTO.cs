using System;
using System.ComponentModel.DataAnnotations;

namespace ProcessPayment.DTO
{
    public class PaymentDTO
    {
        [Required]
        [CreditCard]
        public string CreditCardNumber { get; set; }

        [Required]
        public string CardHolder { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = "Security Code should be 3 digits")]
        public string SecurityCode { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}