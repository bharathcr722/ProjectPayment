using Microsoft.EntityFrameworkCore;

namespace ProjectPayment.Model
{
    public class BillingDBContext : DbContext
    {
        public BillingDBContext(DbContextOptions<BillingDBContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PaymentTranscation> PaymentTranscations { get; set; }
    }
}
