namespace ProjectPayment.Model
{
    public class MatchedInvoice
    {
        public int InvoiceId { get; set; }
        public string Status { get; set; }
        public decimal MatchedAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
