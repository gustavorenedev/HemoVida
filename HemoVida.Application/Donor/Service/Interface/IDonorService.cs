using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;

namespace HemoVida.Application.Donor.Service.Interface;

public interface IDonorService
{
    Task<CreateDonorResponse> RegisterDonor(CreateDonorRequest request);
    Task<List<GetAvailableDonorsResponse>> GetAvailableDonors();
    Task<GetDonationHistoryResponse> GetDonationHistory(string email);
    Task<List<GetDonationHistoryResponse>> GetDonationHistory();
    Task<bool> GetDonorByEmail(string email);
}
