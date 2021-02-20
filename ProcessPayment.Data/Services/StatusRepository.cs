using Microsoft.EntityFrameworkCore;
using ProcessPayment.Models;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _ctx;

        public StatusRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<PaymentState> GetStatusByName(string statusName)
        {
            return await _ctx.PaymentStates.FirstOrDefaultAsync(x => x.Status == statusName);
        }
    }
}