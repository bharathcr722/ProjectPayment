namespace ProjectPayment.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
