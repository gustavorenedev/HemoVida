using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Response;
using HemoVida.Application.Donation.Service.Interface;
using HemoVida.Core.Interfaces.Repositories;

namespace HemoVida.Application.Donation.Service;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _donationRepository;
    private readonly IDonorRepository _donorRepository;
    private readonly IStockRepository _stockRepository;

    public DonationService(IDonationRepository donationRepository, IDonorRepository donorRepository, IStockRepository stockRepository)
    {
        _donationRepository = donationRepository;
        _donorRepository = donorRepository;
        _stockRepository = stockRepository;
    }

    public async Task<DonationRegisterResponse> DonationRegister(DonationRegisterRequest request)
    {
        var donor = await _donorRepository.GetByEmailAsync(request.Email);
        if (donor == null) return new DonationRegisterResponse
        {
            Message = "Doador não encontrado."
        };

        var validationResult = ValidateDonation(donor);

        if (validationResult != null && validationResult.Message != "Você ainda não doou.")
        {
            return validationResult;
        }

        var donation = new Core.Entities.Donation
        {
            DonorId = donor.Id,
            Donor = donor,
            MlQuantity = request.MlQuantity,
            DonationDate = request.DonationDate
        };
        bool result = await _donationRepository.RegisterDonationAsync(donation);

        if (!result)
            return new DonationRegisterResponse
            {
                Message = "Erro ao registrar doação."
            };

        await _stockRepository.UpdateStockAsync(donation);

        // TO DO: Enviar um email formalizando a doação -- kafka

        return new DonationRegisterResponse
        {
            Message = "Doação registrada com sucesso!"
        };
    }

    private DonationRegisterResponse ValidateDonation(Core.Entities.Donor request)
    {
        var lastDonation = request.Donations?.OrderByDescending(x => x.DonationDate).FirstOrDefault();

        if (lastDonation == null) return new DonationRegisterResponse
        {
            Message = "Você ainda não doou."
        };

        var age = CalculateAge(request.BirthDate);
        var nextAllowedDonationDateWoman = lastDonation.DonationDate.AddDays(90);
        var nextAllowedDonationDateMan = lastDonation.DonationDate.AddDays(60);

        if (age < 18) return new DonationRegisterResponse
        {
            Message = "Menores de idade não podem doar."
        };

        if (request.Gender == Core.Enum.Gender.Woman && DateTime.Now < nextAllowedDonationDateWoman)
        {
            return new DonationRegisterResponse
            {
                Message = "Mulheres só podem doar de 90 em 90 dias."
            };
        }

        if (request.Gender == Core.Enum.Gender.Man && DateTime.Now < nextAllowedDonationDateMan)
        {
            return new DonationRegisterResponse
            {
                Message = "Homens só podem doar de 60 em 60 dias."
            };
        }

        return null;
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}
