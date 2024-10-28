using ProjectPayment.Model;

namespace ProjectPayment.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly BillingDBContext _context;
        public InvoiceService(BillingDBContext context) {
            _context = context;
        }
        public List<Invoice> GetAllInvoices()
        {
            return _context.Invoices.ToList();
        }
    }
}
