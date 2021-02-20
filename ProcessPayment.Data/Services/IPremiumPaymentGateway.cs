using ProcessPayment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public interface IPremiumPaymentGateway
    {
        Task<bool> AddPremiumPayment(PaymentModel model);
        Task<bool> DeletePremiumPayment(PaymentModel model);
        Task<bool> UpdatePremiumPayment(PaymentModel model);
        Task<PaymentModel> GetPremiumPaymentById(string paymentId);
        Task<IEnumerable<PaymentModel>> GetAllPremiumPayments();
    }
}