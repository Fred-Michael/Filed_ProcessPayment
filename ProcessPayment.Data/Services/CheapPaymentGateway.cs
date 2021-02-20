using Microsoft.EntityFrameworkCore;
using ProcessPayment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public class CheapPaymentGateway : ICheapPaymentGateway
    {
        private readonly AppDbContext _ctx;

        public CheapPaymentGateway(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> AddCheapPayment(PaymentModel model)
        {
            _ctx.PaymentModels.Add(model);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCheapPayment(PaymentModel model)
        {
            _ctx.PaymentModels.Remove(model);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PaymentModel>> GetAllCheapPayments()
        {
            return await _ctx.PaymentModels.ToListAsync();
        }

        public async Task<PaymentModel> GetCheapPaymentById(string paymentId)
        {
            return await _ctx.PaymentModels.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<bool> UpdateCheapPayment(PaymentModel model)
        {
            _ctx.PaymentModels.Update(model);
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}