using Auker_Minitab_Interview_Assignment.Entities;

namespace Auker_Minitab_Interview_Assignment.Repositories;

public interface ICrmRepository
{
    Task UpsertCustomer(Customer customer);
    Task<Customer?> GetCustomerByEmail(string email);
    Task<List<Customer>> GetAllCustomers();
}