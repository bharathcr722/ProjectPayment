using ProjectPayment.Model;

namespace ProjectPayment.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BillingDBContext _context;
        public CustomerService(BillingDBContext billingDBContext) {
            _context = billingDBContext;
        }
        public List<Project> GetProjectByCustomer(int CustomerID)
        {
            var  customer = _context.Customers.FirstOrDefault(f => f.CustomerId == CustomerID);
            if (customer == null) { 
               return customer.Projects;
            }
            return new List<Project>();
        }
    }
}
