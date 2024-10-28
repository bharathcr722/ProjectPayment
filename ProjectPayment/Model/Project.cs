namespace ProjectPayment.Model
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string  Name{ get; set; }
        public BillingMode BillingMode { get; set; }
        public List<Invoice> Invoices { get; set; }= new List<Invoice>();
    }
}
