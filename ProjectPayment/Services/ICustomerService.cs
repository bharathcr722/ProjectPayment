using ProjectPayment.Model;

namespace ProjectPayment.Services
{
    public interface ICustomerService
    {
        List<Project> GetProjectByCustomer(int CustomerID);

    }
}
