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
                // Load customers.json file and deserialize contents.
                var existingJson = File.ReadAllText(FilePath);
                customers = JsonSerializer.Deserialize<List<Customer>>(existingJson)
                            ?? [];
            }
            else
            {
                customers = [];
            }

            // Check if customer already exists in JSON file by matching email address.
            var existing = customers.FirstOrDefault(c =>
                string.Equals(c.EmailAddress, customer.EmailAddress, StringComparison.OrdinalIgnoreCase));

            // If customer doesn't already exist then create it. 
            if (existing != null)
            {
                existing.Name = customer.Name;
                existing.Address = customer.Address;
            }
            else
            {
                customers.Add(customer);
            }

            // Write customer data to JSON file.
            var newJson = JsonSerializer.Serialize(customers, JsonOptions);
            File.WriteAllText(FilePath, newJson);
        }

        await Task.CompletedTask;
    }
}