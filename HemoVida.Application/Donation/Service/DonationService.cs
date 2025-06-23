using AutoMapper;
using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Response;
using HemoVida.Application.Donation.Service.Interface;
using HemoVida.Application.Donor.Response;
using HemoVida.Application.Publisher.Service.Interface;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Core.Interfaces.Service;

namespace HemoVida.Application.Donation.Service;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _donationRepository;
    private readonly IDonorRepository _donorRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IPublisherService _publisherService;
    private readonly IRedisService _redisService;
    private readonly IMapper _mapper;

    public DonationService(IDonationRepository donationRepository, IDonorRepository donorRepository, IStockRepository stockRepository, IPublisherService publisherService, IMapper mapper, IRedisService redisService)
    {
        _donationRepository = donationRepository;
        _donorRepository = donorRepository;
        _stockRepository = stockRepository;
        _publisherService = publisherService;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<DonationRegisterResponse> DonationRegister(DonationRegisterRequest request)
    {
        var donor = await _donorRepository.GetByEmailAsync(request.Email);
        if (donor == null) return new DonationRegisterResponse
        {
            Message = "Doador não encontrado."
        };

        var validationResult = ValidateDonation(donor);

        if (validationResult != null && validationResult != "Você ainda não doou.")
        {
            return new DonationRegisterResponse
            {
                Message = validationResult
            };
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

        await _redisService.RemoveDonorAsync(donation.DonorId);

        await _stockRepository.UpdateStockAsync(donation);

        var donationRequest = _mapper.Map<DonationPublisherRequest>(donation);

        await _publisherService.PublishAsync(donationRequest);

        return new DonationRegisterResponse
        {
            Message = "Sua doação foi registrada com sucesso no sistema."
        };
    }

    public async Task<DonationRequestedResponse> DonationRequested(string email)
    {
        var donor = await _donorRepository.GetByEmailAsync(email);
        if (donor == null) return new DonationRequestedResponse
        {
            Message = "Doador não encontrado."
        };

        var validationResult = ValidateDonation(donor);

        if (validationResult != null && validationResult != "Você ainda não doou.")
        {
            return new DonationRequestedResponse
            {
                Message = validationResult
            };
        }

        await _redisService.AddAvailableDonorAsync(donor);

        return new DonationRequestedResponse { Message = "Aguarde a enfermeira chamar" };
    }

    private string ValidateDonation(Core.Entities.Donor request)
    {
        var age = CalculateAge(request.BirthDate);

        if (age < 18)
        {
            return "Menores de idade não podem doar.";
        }

        var lastDonation = request.Donations?.OrderByDescending(x => x.DonationDate).FirstOrDefault();

        if (lastDonation == null) return "Você ainda não doou.";

        var nextAllowedDonationDateWoman = lastDonation.DonationDate.AddDays(90);
        var nextAllowedDonationDateMan = lastDonation.DonationDate.AddDays(60);

        if (request.Gender == Core.Enum.Gender.Woman && DateTime.Now < nextAllowedDonationDateWoman)
        {
            return "Mulheres só podem doar de 90 em 90 dias.";
        }

        if (request.Gender == Core.Enum.Gender.Man && DateTime.Now < nextAllowedDonationDateMan)
        {
            return "Homens só podem doar de 60 em 60 dias.";
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
