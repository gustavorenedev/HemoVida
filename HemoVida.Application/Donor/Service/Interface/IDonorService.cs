using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;

namespace HemoVida.Application.Donor.Service.Interface;

public interface IDonorService
{
    Task<CreateDonorResponse> RegisterDonor(CreateDonorRequest request);
}
