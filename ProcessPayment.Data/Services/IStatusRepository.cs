using ProcessPayment.Models;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public interface IStatusRepository
    {
        Task<PaymentState> GetStatusByName(string statusName);
    }
}