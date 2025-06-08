using AutoMapper;
using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;
using HemoVida.Application.Donor.Service.Interface;
using HemoVida.Application.ZipCode.Service.Interface;
using HemoVida.Core.Entities;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Core.Interfaces.Service;

namespace HemoVida.Application.Donor.Service;

public class DonorService : IDonorService
{

    private readonly IDonorRepository _donorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IZipCodeService _zipCodeService;
    private readonly IRedisService _redisService;
    private readonly IMapper _mapper;

    public DonorService(IDonorRepository donorRepository, IUserRepository userRepository, IZipCodeService zipCodeService, IMapper mapper, IRedisService redisService)
    {
        _donorRepository = donorRepository;
        _userRepository = userRepository;
        _zipCodeService = zipCodeService;
        _mapper = mapper;
        _redisService = redisService;
    }

    public async Task<List<GetAvailableDonorsResponse>> GetAvailableDonors()
    {
        var result = await _redisService.GetAvailableDonorsAsync();

        if (result == null || result.Count == 0)
            return null;

        return _mapper.Map<List<GetAvailableDonorsResponse>>(result);
    }

    public async Task<GetDonationHistoryResponse> GetDonationHistory(string email)
    {
        var result = await _donorRepository.GetByEmailAsync(email);

        if (result == null)
            return null;

        return _mapper.Map<GetDonationHistoryResponse>(result);
    }

    public async Task<List<GetDonationHistoryResponse>> GetDonationHistory()
    {
        var result = await _donorRepository.GetAllAsync();

        if (result == null)
            return null;

        return _mapper.Map<List<GetDonationHistoryResponse>>(result);
    }

    public async Task<CreateDonorResponse> RegisterDonor(CreateDonorRequest request)
    {
        var address = await _zipCodeService.GetAddress(request.ZipCode);
        var user = await _userRepository.GetByEmailAsync(request.Email);
        var donor = await _donorRepository.GetByEmailAsync(request.Email);

        if (donor != null)
            return new CreateDonorResponse { Message = "Doador já possui cadastro." };

        if (address == null)
            return new CreateDonorResponse { Message = "Endereço não encontrado." };

        if (user == null)
            return new CreateDonorResponse { Message = "Usuário não encontrado." };

        var newDonor = new Core.Entities.Donor
        {
            UserId = user.Id,
            User = user,
            BirthDate = request.BirthDate,
            Weight = request.Weight,
            BloodType = request.BloodType,
            RhFactor = request.RhFactor,
            Gender = request.Gender
        };

        newDonor = await _donorRepository.CreateDonor(newDonor);

        var endereco = _mapper.Map<Address>(address);
        endereco.DonorId = newDonor.Id;
        newDonor.Address = endereco;

        await _donorRepository.UpdateDonor(newDonor, newDonor.Id);

        await _redisService.AddAvailableDonorAsync(newDonor);

        return new CreateDonorResponse { Message = "Doador cadastrado com sucesso. Aguarde a enfermeira chamar" };
    }
}
