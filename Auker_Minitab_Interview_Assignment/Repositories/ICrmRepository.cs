using Auker_Minitab_Interview_Assignment.Entities;

namespace Auker_Minitab_Interview_Assignment.Repositories;

public interface ICrmRepository
{
    Task UpsertCustomer(Customer customer);
}