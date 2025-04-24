using AutoMapper;
using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;
using HemoVida.Application.Donor.Service.Interface;
using HemoVida.Application.ZipCode.Service.Interface;
using HemoVida.Core.Entities;
using HemoVida.Core.Interfaces;
using HemoVida.Infrastructure.Repositories.Interfaces;

namespace HemoVida.Application.Donor.Service;

public class DonorService : IDonorService
{

    private readonly IDonorRepository _donorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IZipCodeService _zipCodeService;
    private readonly IMapper _mapper;

    public DonorService(IDonorRepository donorRepository, IUserRepository userRepository, IZipCodeService zipCodeService, IMapper mapper)
    {
        _donorRepository = donorRepository;
        _userRepository = userRepository;
        _zipCodeService = zipCodeService;
        _mapper = mapper;
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
        };

        newDonor = await _donorRepository.CreateDonor(newDonor);

        var endereco = _mapper.Map<Address>(address);
        endereco.DonorId = newDonor.Id;
        newDonor.Address = endereco;

        await _donorRepository.UpdateDonor(newDonor, newDonor.Id);

        return new CreateDonorResponse { Message = "Doador cadastrado com sucesso. Aguarde a enfermeira chamar" };
    }
}
