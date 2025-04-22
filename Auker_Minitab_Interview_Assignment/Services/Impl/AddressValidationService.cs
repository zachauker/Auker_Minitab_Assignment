using System.Text.Json;
using System.Web;
using System.Xml.Linq;
using Auker_Minitab_Interview_Assignment.Entities;

namespace Auker_Minitab_Interview_Assignment.Services.Impl;

public class AddressValidationService(HttpClient httpClient, IConfiguration configuration) : IAddressValidationService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _apiKey = configuration["Geoapify:ApiKey"] ?? throw new InvalidOperationException();

     public async Task<bool> IsAddressValidAsync(Address address)
    {
        // Build full address string
        var fullAddress = $"{address.Line1}, {address.City}, {address.State} {address.PostalCode}, {address.Country}";

        var requestUrl = $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(fullAddress)}&format=json&apiKey={_apiKey}";

        var response = await _httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
            return false;

        var content = await response.Content.ReadAsStringAsync();

        var geoData = JsonSerializer.Deserialize<GeoapifyResponse>(content);

        // Address is valid if Geoapify returns at least one result with a high confidence score
        return geoData?.Results?.Any(r => r.Rank?.Confidence > 0.8) == true;
    }
    
    private class GeoapifyResponse
    {
        public List<Result> Results { get; set; }

        public class Result
        {
            public Rank Rank { get; set; }
        }

        public class Rank
        {
            public double Confidence { get; set; }
        }
    }
}