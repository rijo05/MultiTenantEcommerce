using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.ValueObjects;
using System.Net.Http.Json;
using System.Text.Json;

namespace MultiTenantEcommerce.Infrastructure.AddressValidator;
public class AddressValidator : IAddressValidator
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AddressValidator(HttpClient client, IConfiguration configuration)
    {
        _httpClient = client;
        _apiKey = configuration["OpenCage:ApiKey"] ?? throw new Exception("API key doesnt exist");
    }

    public async Task<AddressValidationResult> IsAddressValid(Address address)
    {
        var query = $"{address.Street}, {address.PostalCode}, {address.City}, {address.Country}";
        var url = $"https://api.opencagedata.com/geocode/v1/json?q={Uri.EscapeDataString(query)}&key={_apiKey}&limit=1";

        var response = await _httpClient.GetFromJsonAsync<JsonDocument>(url);
        if (response is null)
            return new AddressValidationResult(false, 0, null, "", new());

        var results = response.RootElement.GetProperty("results");

        if (results.GetArrayLength() == 0)
            return new AddressValidationResult(false, 0, null, "", new());

        var best = results[0];
        var confidence = best.GetProperty("confidence").GetDouble();
        var formatted = best.GetProperty("formatted").GetString() ?? "";
        var geometry = best.GetProperty("geometry");

        var lat = geometry.GetProperty("lat").GetDouble();
        var lng = geometry.GetProperty("lng").GetDouble();

        var components = new Dictionary<string, string>();
        if (best.TryGetProperty("components", out var compJson))
        {
            foreach (var prop in compJson.EnumerateObject())
                components[prop.Name] = prop.Value.ToString();
        }

        return new AddressValidationResult(
            confidence >= 7,
            confidence,
            new GeoLocation(lat, lng),
            formatted,
            components
        );
    }
}
