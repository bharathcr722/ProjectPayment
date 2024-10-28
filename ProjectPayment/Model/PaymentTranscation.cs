namespace ProjectPayment.Model
{
    public class PaymentTranscation
    {
        public int TransactionID { get; set; }
        public int CustomerID { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
