using ProcessPayment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public interface ICheapPaymentGateway
    {
        Task<bool> AddCheapPayment(PaymentModel model);
        Task<bool> DeleteCheapPayment(PaymentModel model);
        Task<bool> UpdateCheapPayment(PaymentModel model);
        Task<PaymentModel> GetCheapPaymentById(string paymentId);
        Task<IEnumerable<PaymentModel>> GetAllCheapPayments();
    }
}