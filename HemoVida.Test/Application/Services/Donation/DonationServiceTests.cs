using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Service;
using HemoVida.Core.Enum;
using HemoVida.Core.Interfaces.Repositories;
using Moq;

namespace HemoVida.Test.Application.Services.Donation;


public class DonationServiceTests
{
    private readonly Mock<IDonationRepository> _donationRepositoryMock;
    private readonly Mock<IDonorRepository> _donorRepositoryMock;
    private readonly Mock<IStockRepository> _stockRepositoryMock;
    private readonly DonationService _service;

    public DonationServiceTests()
    {
        _donationRepositoryMock = new Mock<IDonationRepository>();
        _donorRepositoryMock = new Mock<IDonorRepository>();
        _stockRepositoryMock = new Mock<IStockRepository>();

        _service = new DonationService(
            _donationRepositoryMock.Object,
            _donorRepositoryMock.Object,
            _stockRepositoryMock.Object
        );
    }

    [Fact]
    public async Task DonationRegister_DoadorNaoEncontrado_DeveRetornarMensagemErro()
    {
        // Arrange
        var request = new DonationRegisterRequest { Email = "naoexiste@teste.com" };
        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((Core.Entities.Donor)null);

        // Act
        var response = await _service.DonationRegister(request);

        // Assert
        Assert.Equal("Doador não encontrado.", response.Message);
    }

    [Fact]
    public async Task DonationRegister_PrimeiraDoacao_DeveRegistrarComSucesso()
    {
        // Arrange
        var request = new DonationRegisterRequest
        {
            Email = "teste@doador.com",
            MlQuantity = 450,
            DonationDate = DateTime.Today
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-30),
            Gender = Gender.Man,
            Donations = new List<Core.Entities.Donation>()
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);
        _donationRepositoryMock.Setup(r => r.RegisterDonationAsync(It.IsAny<Core.Entities.Donation>())).ReturnsAsync(true);
        _stockRepositoryMock.Setup(r => r.UpdateStockAsync(It.IsAny<Core.Entities.Donation>())).Returns(Task.FromResult(true));

        // Act
        var response = await _service.DonationRegister(request);

        // Assert
        Assert.Equal("Doação registrada com sucesso!", response.Message);
    }

    [Fact]
    public async Task DonationRegister_DoadorMenorIdade_DeveRecusar()
    {
        // Arrange
        var request = new DonationRegisterRequest
        {
            Email = "jovem@teste.com",
            DonationDate = DateTime.Today,
            MlQuantity = 450
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-16),
            Gender = Gender.Man,
            Donations = new List<Core.Entities.Donation>
            {
                new Core.Entities.Donation { DonationDate = DateTime.Today.AddMonths(-3) }
            }
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);

        // Act
        var response = await _service.DonationRegister(request);

        // Assert
        Assert.Equal("Menores de idade não podem doar.", response.Message);
    }

    [Fact]
    public async Task DonationRegister_HomemAntesDe60Dias_DeveRecusar()
    {
        var request = new DonationRegisterRequest
        {
            Email = "homem@teste.com",
            DonationDate = DateTime.Today,
            MlQuantity = 450
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-25),
            Gender = Gender.Man,
            Donations = new List<Core.Entities.Donation>
            {
                new Core.Entities.Donation { DonationDate = DateTime.Today.AddDays(-30) }
            }
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);

        var response = await _service.DonationRegister(request);

        Assert.Equal("Homens só podem doar de 60 em 60 dias.", response.Message);
    }

    [Fact]
    public async Task DonationRegister_ErroAoSalvar_Doacao_DeveRetornarMensagemErro()
    {
        var request = new DonationRegisterRequest
        {
            Email = "erro@teste.com",
            DonationDate = DateTime.Today,
            MlQuantity = 450
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-30),
            Gender = Gender.Man,
            Donations = new List<Core.Entities.Donation>()
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);
        _donationRepositoryMock.Setup(r => r.RegisterDonationAsync(It.IsAny<Core.Entities.Donation>())).ReturnsAsync(false);

        var response = await _service.DonationRegister(request);

        Assert.Equal("Erro ao registrar doação.", response.Message);
    }

    [Fact]
    public async Task DonationRegister_EmailJaCadastrado_DevePermitirDoacao()
    {
        var request = new DonationRegisterRequest
        {
            Email = "repetido@teste.com",
            MlQuantity = 450,
            DonationDate = DateTime.Today
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-30),
            Gender = Gender.Man,
            Donations = new List<Core.Entities.Donation>()
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);
        _donationRepositoryMock.Setup(r => r.RegisterDonationAsync(It.IsAny<Core.Entities.Donation>())).ReturnsAsync(true);
        _stockRepositoryMock.Setup(r => r.UpdateStockAsync(It.IsAny<Core.Entities.Donation>())).Returns(Task.FromResult(true));

        var response = await _service.DonationRegister(request);

        Assert.Equal("Doação registrada com sucesso!", response.Message);
    }

    [Fact]
    public async Task DonationRegister_MenorIdadeComCadastroNaoPodeDoar()
    {
        var request = new DonationRegisterRequest
        {
            Email = "menor@teste.com",
            MlQuantity = 450,
            DonationDate = DateTime.Today
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-17),
            Gender = Gender.Man,
            Donations = new List<Core.Entities.Donation>()
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);

        var response = await _service.DonationRegister(request);

        Assert.Equal("Menores de idade não podem doar.", response.Message);
    }

    [Theory]
    [InlineData(400)]
    [InlineData(480)]
    [InlineData(420)]
    [InlineData(470)]
    [InlineData(450)]
    public async Task DonationRegister_MlForaDoIntervalo_DeveSeComportarCorretamente(int ml)
    {
        var request = new DonationRegisterRequest
        {
            Email = "volume@teste.com",
            MlQuantity = ml,
            DonationDate = DateTime.Today
        };

        var validator = new DonationRegisterRequestValidator();
        var result = validator.Validate(request);

        if (ml < 420 || ml > 470)
        {
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "A quantidade de mililitros deve ser entre 420ml e 470ml.");
        }
        else
        {
            Assert.True(result.IsValid);
        }
    }

    [Fact]
    public async Task DonationRegister_MulherAntesDe90Dias_DeveRecusar()
    {
        var request = new DonationRegisterRequest
        {
            Email = "mulher@teste.com",
            DonationDate = DateTime.Today,
            MlQuantity = 450
        };

        var donor = new Core.Entities.Donor
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-30),
            Gender = Gender.Woman,
            Donations = new List<Core.Entities.Donation>
            {
                new Core.Entities.Donation { DonationDate = DateTime.Today.AddDays(-60) }
            }
        };

        _donorRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(donor);

        var response = await _service.DonationRegister(request);

        Assert.Equal("Mulheres só podem doar de 90 em 90 dias.", response.Message);
    }
}
