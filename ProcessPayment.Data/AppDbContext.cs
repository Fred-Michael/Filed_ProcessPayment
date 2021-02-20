using Microsoft.EntityFrameworkCore;
using ProcessPayment.Models;

namespace ProcessPayment.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<PaymentModel> PaymentModels { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //configuring the 'Amount' Column in the PaymentModel to accept decimal precision up to 2 dp
            builder.Entity<PaymentModel>()
                   .Property("Amount")
                   .HasColumnType("decimal")
                   .HasPrecision(8, 2);
        }
    }
}