using System;

namespace ProjectPayment.Model
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
