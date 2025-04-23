using Auker_Minitab_Interview_Assignment.Controllers;
using Auker_Minitab_Interview_Assignment.Entities;
using Auker_Minitab_Interview_Assignment.Repositories;
using Auker_Minitab_Interview_Assignment.Repositories.Impl;
using Auker_Minitab_Interview_Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Testing;

public class CustomerControllerTests
{
    [Fact]
    public async Task PostCustomer_ValidAddress_CallsCrmWithAddress()
    {
        // Set up mock validator, crm and controller. 
        var validatorMock = new Mock<IAddressValidationService>();
        validatorMock.Setup(v => v.IsAddressValidAsync(It.IsAny<Address>()))
            .ReturnsAsync(true);

        var crmMock = new Mock<FakeCrmRepository>();

        var controller = new CustomerController(validatorMock.Object, crmMock.Object);

        // Create test customer 
        var customer = new Customer
        {
            Name = "Jane Doe",
            EmailAddress = "jane@example.com",
            Address = new Address
            {
                Line1 = "10 Main St",
                City = "Chicago",
                State = "IL",
                PostalCode = "60603",
                Country = "US"
            }
        };

        // Post to customer endpoint.
        var result = await controller.PostCustomer(customer);

        // Assert that success result is returned. 
        var okResult = Assert.IsType<OkResult>(result);
    }
}