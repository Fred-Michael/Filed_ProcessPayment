using ProcessPayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessPayment.Data
{
    public static class DataSeeder
    {
        //preseeding the PaymentStatus table with needed status data
        public static async Task Preseed(AppDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            var statusData = new List<PaymentState>
            {
                new PaymentState
                {
                    Status = "pending"
                },
                new PaymentState
                {
                    Status = "success"
                },
                new PaymentState
                {
                    Status = "failed"
                }
            };

            if (!ctx.PaymentStates.Any())
            {
                foreach (var state in statusData)
                {
                    var id = Guid.NewGuid().ToString();

                    state.PaymentStateId = id;
                    ctx.PaymentStates.Add(state);
                }
                await ctx.SaveChangesAsync();
            }
        }
    }
}