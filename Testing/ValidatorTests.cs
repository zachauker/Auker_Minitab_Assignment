using System.Net;
using Auker_Minitab_Interview_Assignment.Entities;
using Auker_Minitab_Interview_Assignment.Services.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace Testing;

public class ValidatorTests
{
    [Fact]
    public async Task AddressValidator_WithHighConfidence_ReturnsTrue()
    {
        const string jsonResponse = """
                                    {
                                            "results": [
                                                { "rank": { "confidence": 0.95 } }
                                            ]
                                        }
                                    """;

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var client = new HttpClient(handlerMock.Object);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "Geoapify:ApiKey", "fake" } }!)
            .Build();

        var logger = new Mock<ILogger<AddressValidationService>>();

        var service = new AddressValidationService(client, config, logger.Object);

        var result = await service.IsAddressValidAsync(new Address
        {
            Line1 = "1453 Willowbrook Dr",
            City = "Boalsburg",
            State = "PA",
            PostalCode = "16827",
            Country = "US"
        });

        Assert.True(result);
    }
}