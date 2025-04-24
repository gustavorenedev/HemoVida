using HemoVida.Application.ZipCode.Service.Response;
using HemoVida.Core.Entities;

namespace HemoVida.Application.ZipCode.Profile;

public class ZipCodeProfile : AutoMapper.Profile
{
    public ZipCodeProfile()
    {
        CreateMap<AddressResponse, Address>()
            .AfterMap((src, dest) => dest.ZipCode = src.Cep);
    }
}
