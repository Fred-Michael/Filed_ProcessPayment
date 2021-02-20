using Microsoft.EntityFrameworkCore;
using ProcessPayment.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPayment.Data.Services
{
    public class PremiumPaymentGateway : IPremiumPaymentGateway
    {
        private readonly AppDbContext _ctx;

        public PremiumPaymentGateway(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> AddPremiumPayment(PaymentModel model)
        {
            _ctx.PaymentModels.Add(model);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePremiumPayment(PaymentModel model)
        {
            _ctx.PaymentModels.Remove(model);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PaymentModel>> GetAllPremiumPayments()
        {
            return await _ctx.PaymentModels.ToListAsync();
        }

        public async Task<PaymentModel> GetPremiumPaymentById(string paymentId)
        {
            return await _ctx.PaymentModels.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<bool> UpdatePremiumPayment(PaymentModel model)
        {
            _ctx.PaymentModels.Update(model);
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}