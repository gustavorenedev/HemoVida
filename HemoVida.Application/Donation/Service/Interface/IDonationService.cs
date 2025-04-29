using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Response;

namespace HemoVida.Application.Donation.Service.Interface;

public interface IDonationService
{
    Task<DonationRegisterResponse> DonationRegister(DonationRegisterRequest request);
}
