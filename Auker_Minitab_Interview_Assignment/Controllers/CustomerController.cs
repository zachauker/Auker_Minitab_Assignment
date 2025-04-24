using Auker_Minitab_Interview_Assignment.Entities;
using Auker_Minitab_Interview_Assignment.Repositories;
using Auker_Minitab_Interview_Assignment.Repositories.Impl;
using Auker_Minitab_Interview_Assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auker_Minitab_Interview_Assignment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IAddressValidationService validator, ICrmRepository crm) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
    {
        var isAddressValid = customer.Address != null && await validator.IsAddressValidAsync(customer.Address);

        // If address returns as invalid throw error.
        if (!isAddressValid)
            customer.Address = null;

        // Call CRM upsert method.
        await crm.UpsertCustomer(customer);

        return Ok();
    }
}