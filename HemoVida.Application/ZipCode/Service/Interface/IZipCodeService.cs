using HemoVida.Application.ZipCode.Service.Response;

namespace HemoVida.Application.ZipCode.Service.Interface;

public interface IZipCodeService
{
    Task<AddressResponse> GetAddress(string zipcode);
}
