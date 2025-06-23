using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Response;
using HemoVida.Application.Donor.Response;

namespace HemoVida.Application.Donation.Service.Interface;

public interface IDonationService
{
    Task<DonationRegisterResponse> DonationRegister(DonationRegisterRequest request);
    Task<DonationRequestedResponse> DonationRequested(string email);
}
