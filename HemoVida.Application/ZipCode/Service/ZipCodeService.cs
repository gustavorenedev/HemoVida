using HemoVida.Application.ZipCode.Service.Interface;
using HemoVida.Application.ZipCode.Service.Response;
using System.Net.Http.Json;

namespace HemoVida.Application.ZipCode.Service;

public class ZipCodeService : IZipCodeService
{
    private readonly HttpClient _httpClient;

    public ZipCodeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AddressResponse> GetAddress(string zipcode)
    {
        var url = $"https://brasilapi.com.br/api/cep/v1/{zipcode}";

        return await _httpClient.GetFromJsonAsync<AddressResponse>(url);
    }
}
