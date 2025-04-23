using System.Text.Json;
using Auker_Minitab_Interview_Assignment.Entities;
using Microsoft.AspNetCore.Http.Json;

namespace Auker_Minitab_Interview_Assignment.Repositories.Impl;

public class FakeCrmRepository : ICrmRepository
{
    private const string FilePath = "customers.json";
    private static readonly object FileLock = new object();

    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    public async Task UpsertCustomer(Customer customer)
    {
        if (customer == null || string.IsNullOrWhiteSpace(customer.EmailAddress))
            throw new ArgumentException("Customer or email cannot be null.");

        lock (FileLock)
        {
            List<Customer> customers;
            if (File.Exists(FilePath))
            {
                var existingJson = File.ReadAllText(FilePath);
                customers = JsonSerializer.Deserialize<List<Customer>>(existingJson)
                            ?? [];
            }
            else
            {
                customers = new List<Customer>();
            }

            var existing = customers.FirstOrDefault(c =>
                string.Equals(c.EmailAddress, customer.EmailAddress, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                existing.Name = customer.Name;
                existing.Address = customer.Address;
            }
            else
            {
                customers.Add(customer);
            }

            var newJson = JsonSerializer.Serialize(customers, JsonOptions);
            File.WriteAllText(FilePath, newJson);
        }

        await Task.CompletedTask;
    }


    public Task<Customer?> GetCustomerByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<List<Customer>> GetAllCustomers()
    {
        throw new NotImplementedException();
    }
}