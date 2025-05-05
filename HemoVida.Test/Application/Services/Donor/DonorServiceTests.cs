using AutoMapper;
using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;
using HemoVida.Application.Donor.Service;
using HemoVida.Application.ZipCode.Service.Interface;
using HemoVida.Application.ZipCode.Service.Response;
using HemoVida.Core.Entities;
using HemoVida.Core.Enum;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Core.Interfaces.Service;
using Moq;
using System.ComponentModel;

namespace HemoVida.Test.Application.Services.Donor;

public class DonorServiceTests
{
    private readonly Mock<IDonorRepository> _donorRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IZipCodeService> _zipCodeService;
    private readonly Mock<IRedisService> _redisService;
    private readonly Mock<IMapper> _mapper;
    private readonly DonorService _service;

    public DonorServiceTests()
    {

        _donorRepository = new Mock<IDonorRepository>();
        _userRepository = new Mock<IUserRepository>();
        _zipCodeService = new Mock<IZipCodeService>();
        _redisService = new Mock<IRedisService>();
        _mapper = new Mock<IMapper>();

        _service = new DonorService(
            _donorRepository.Object,
            _userRepository.Object,
            _zipCodeService.Object,
            _mapper.Object,
            _redisService.Object
        );
    }

    [Fact]
    [DisplayName("Deve retornar mensagem quando doador já estiver cadastrado")]
    public async Task RegisterDonor_DonorAlreadyExists_ReturnsMessage()
    {
        // Arrange
        var request = new CreateDonorRequest { Email = "teste@email.com" };
        _donorRepository.Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(new Core.Entities.Donor());

        // Act
        var result = await _service.RegisterDonor(request);

        // Assert
        Assert.Equal("Doador já possui cadastro.", result.Message);
    }

    [Fact]
    [DisplayName("Deve retornar mensagem quando endereço não for encontrado")]
    public async Task RegisterDonor_AddressNotFound_ReturnsMessage()
    {
        var request = new CreateDonorRequest { Email = "novo@email.com", ZipCode = "00000000" };

        _donorRepository.Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Core.Entities.Donor)null);

        _zipCodeService.Setup(z => z.GetAddress(request.ZipCode))
            .ReturnsAsync((AddressResponse)null);

        // Act
        var result = await _service.RegisterDonor(request);

        // Assert
        Assert.Equal("Endereço não encontrado.", result.Message);
    }

    [Fact]
    [DisplayName("Deve retornar mensagem quando usuário não for encontrado")]
    public async Task RegisterDonor_UserNotFound_ReturnsMessage()
    {
        var request = new CreateDonorRequest { Email = "novo@email.com", ZipCode = "12345678" };

        _donorRepository.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((Core.Entities.Donor)null);
        _zipCodeService.Setup(z => z.GetAddress(request.ZipCode)).ReturnsAsync(new AddressResponse());
        _userRepository.Setup(u => u.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);

        // Act
        var result = await _service.RegisterDonor(request);

        // Assert
        Assert.Equal("Usuário não encontrado.", result.Message);
    }

    [Fact]
    [DisplayName("Deve cadastrar doador com sucesso quando dados estiverem corretos")]
    public async Task RegisterDonor_ValidData_ReturnsSuccessMessage()
    {
        var request = new CreateDonorRequest
        {
            Email = "novo@email.com",
            ZipCode = "12345678",
            BirthDate = DateTime.Today,
            Weight = 70,
            BloodType = "A",
            RhFactor = "+",
            Gender = Core.Enum.Gender.Man
        };

        var user = new User { Id = 1 };
        var endereco = new AddressResponse();
        var newDonor = new Core.Entities.Donor { Id = 1, UserId = user.Id, User = user };

        _donorRepository.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((Core.Entities.Donor)null);
        _zipCodeService.Setup(z => z.GetAddress(request.ZipCode)).ReturnsAsync(endereco);
        _userRepository.Setup(u => u.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _donorRepository.Setup(r => r.CreateDonor(It.IsAny<Core.Entities.Donor>())).ReturnsAsync(newDonor);
        _mapper.Setup(m => m.Map<Address>(endereco)).Returns(new Address());

        // Act
        var result = await _service.RegisterDonor(request);

        // Assert
        Assert.Equal("Doador cadastrado com sucesso. Aguarde a enfermeira chamar", result.Message);
    }

    [Fact]
    [DisplayName("Deve retornar nulo quando não houver doadores disponíveis no Redis")]
    public async Task GetAvailableDonors_NoDonors_ReturnsNull()
    {
        _redisService.Setup(r => r.GetAvailableDonorsAsync())
            .ReturnsAsync((List<Core.Entities.Donor>)null);

        var result = await _service.GetAvailableDonors();

        Assert.Null(result);
    }

    [Fact]
    [DisplayName("Deve retornar doadores disponíveis quando houver dados no Redis")]
    public async Task GetAvailableDonors_ValidDonors_ReturnsMappedList()
    {
        var donors = new List<Core.Entities.Donor> { new Core.Entities.Donor(), new Core.Entities.Donor() };
        var mapped = new List<GetAvailableDonorsResponse>
        {
            new GetAvailableDonorsResponse(),
            new GetAvailableDonorsResponse()
        };

        _redisService.Setup(r => r.GetAvailableDonorsAsync()).ReturnsAsync(donors);
        _mapper.Setup(m => m.Map<List<GetAvailableDonorsResponse>>(donors)).Returns(mapped);

        var result = await _service.GetAvailableDonors();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    [DisplayName("Deve retornar erro se ZipCode estiver nulo ou inválido")]
    public async Task RegisterDonor_InvalidZipCode_ReturnsAddressNotFound()
    {
        var request = new CreateDonorRequest
        {
            Email = "teste@email.com",
            ZipCode = null
        };

        _donorRepository.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((Core.Entities.Donor)null);
        _zipCodeService.Setup(z => z.GetAddress(request.ZipCode)).ReturnsAsync((AddressResponse)null);

        var result = await _service.RegisterDonor(request);

        Assert.Equal("Endereço não encontrado.", result.Message);
    }

    [Fact]
    [DisplayName("Deve lançar exceção ao falhar ao atualizar doador")]
    public async Task RegisterDonor_UpdateThrowsException_Throws()
    {
        var request = new CreateDonorRequest
        {
            Email = "email@teste.com",
            ZipCode = "12345678",
            BirthDate = DateTime.Today,
            Weight = 70,
            BloodType = "A",
            RhFactor = "+",
            Gender = Core.Enum.Gender.Woman
        };

        var user = new User { Id = 1 };
        var endereco = new AddressResponse();
        var createdDonor = new Core.Entities.Donor { Id = 1, UserId = user.Id, User = user };

        _donorRepository.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((Core.Entities.Donor)null);
        _zipCodeService.Setup(z => z.GetAddress(request.ZipCode)).ReturnsAsync(endereco);
        _userRepository.Setup(u => u.GetByEmailAsync(request.Email)).ReturnsAsync(user);
        _donorRepository.Setup(r => r.CreateDonor(It.IsAny<Core.Entities.Donor>())).ReturnsAsync(createdDonor);
        _mapper.Setup(m => m.Map<Address>(It.IsAny<AddressResponse>())).Returns(new Address());

        _donorRepository.Setup(r => r.UpdateDonor(It.IsAny<Core.Entities.Donor>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Erro na atualização"));

        await Assert.ThrowsAsync<Exception>(() => _service.RegisterDonor(request));
    }
}