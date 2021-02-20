using ProcessPayment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public interface IExpensivePaymentGateway
    {
        Task<bool> AddExpensivePayment(PaymentModel model);
        Task<bool> DeleteExpensivePayment(PaymentModel model);
        Task<bool> UpdateExpensivePayment(PaymentModel model);
        Task<PaymentModel> GetExpensivePaymentById(string paymentId);
        Task<IEnumerable<PaymentModel>> GetAllExpensivePayments();
    }
}
