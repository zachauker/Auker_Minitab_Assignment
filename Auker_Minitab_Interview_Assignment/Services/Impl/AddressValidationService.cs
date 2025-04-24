using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Xml.Linq;
using Auker_Minitab_Interview_Assignment.Entities;

namespace Auker_Minitab_Interview_Assignment.Services.Impl;

public class AddressValidationService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<AddressValidationService> logger) : IAddressValidationService
{
    private readonly string _apiKey = configuration["Geoapify:ApiKey"] ?? throw new InvalidOperationException();

    public async Task<bool> IsAddressValidAsync(Address address)
    {
        // Build full address string
        var fullAddress = $"{address.Line1}, {address.City}, {address.State} {address.PostalCode}, {address.Country}";
        var requestUrl =
            $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(fullAddress)}&format=json&apiKey={_apiKey}";

        logger.LogInformation("Geoapify Request URL: {Url}", requestUrl);

        HttpResponseMessage response;

        try
        {
            response = await httpClient.GetAsync(requestUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "HTTP request to Geoapify failed");
            return false;
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        GeoapifyResponse? geoData;

        try
        {
            geoData = JsonSerializer.Deserialize<GeoapifyResponse>(responseBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deserialize Geoapify response");
            return false;
        }

        if (geoData?.Results == null || geoData.Results.Count == 0)
        {
            logger.LogWarning("No results found in Geoapify response.");
            return false;
        }

        // Grab confidence value from Geoapify response. 
        var confidence = geoData.Results.First().Rank?.Confidence ?? 0;
        logger.LogInformation("Geoapify confidence score: {Confidence}", confidence);

        // If confidence score is higher than 0.8 consider address valid. 
        return confidence > 0.8;
    }

    private class GeoapifyResponse
    {
        [JsonPropertyName("results")] public List<Result>? Results { get; set; }

        public class Result
        {
            [JsonPropertyName("rank")] public Rank? Rank { get; set; }

        }

        public class Rank
        {
            [JsonPropertyName("confidence")] public double Confidence { get; set; }

            [JsonPropertyName("confidence_street_level")]
            public double ConfidenceStreetLevel { get; set; }
        }
    }
}