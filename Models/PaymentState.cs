using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProcessPayment.Models
{
    public class PaymentState
    {
        [Key]
        public string PaymentStateId { get; set; }

        [Required]
        [MaxLength(7)]
        public string Status { get; set; } //pending, success, failed

        public IEnumerable<PaymentModel> PaymentModels { get; set; }
    }
}