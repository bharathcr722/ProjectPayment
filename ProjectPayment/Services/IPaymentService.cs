using ProjectPayment.Model;

namespace ProjectPayment.Services
{
    public interface IPaymentService
    {
        List<MatchedInvoice> MatchPayments(List<PaymentTranscation> transcations);
    }
}
