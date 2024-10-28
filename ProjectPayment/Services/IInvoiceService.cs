using ProjectPayment.Model;

namespace ProjectPayment.Services
{
    public interface IInvoiceService
    {
        List<Invoice> GetAllInvoices();
    }
}
