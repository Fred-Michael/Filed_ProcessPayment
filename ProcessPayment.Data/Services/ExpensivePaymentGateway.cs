using Microsoft.EntityFrameworkCore;
using ProcessPayment.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public class ExpensivePaymentGateway : IExpensivePaymentGateway
    {
        private readonly AppDbContext _ctx;

        public ExpensivePaymentGateway(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> AddExpensivePayment(PaymentModel model)
        {
            try
            {
                _ctx.PaymentModels.Add(model);
                int affectedRow = await _ctx.SaveChangesAsync();
                return affectedRow > 0;
            }
            catch (Exception ex)
            {
                var men = ex.Message;
                throw;
            }
        }

        public async Task<bool> DeleteExpensivePayment(PaymentModel model)
        {
            _ctx.PaymentModels.Remove(model);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PaymentModel>> GetAllExpensivePayments()
        {
            return await _ctx.PaymentModels.ToListAsync();
        }

        public async Task<PaymentModel> GetExpensivePaymentById(string paymentId)
        {
            var gone = await _ctx.PaymentModels.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            return gone;
        }

        public async Task<bool> UpdateExpensivePayment(PaymentModel model)
        {
            _ctx.PaymentModels.Update(model);
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}