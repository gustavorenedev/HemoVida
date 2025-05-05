using HemoVida.API.Controllers.Donor;
using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;
using HemoVida.Application.Donor.Service.Interface;
using HemoVida.Core.Enum;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel;

namespace HemoVida.Test.Controllers.Donor;

public class DonorControllerTests
{
    private readonly Mock<IDonorService> _donorServiceMock;
    private readonly DonorController _controller;

    public DonorControllerTests()
    {
        _donorServiceMock = new Mock<IDonorService>();
        _controller = new DonorController(_donorServiceMock.Object);
    }

    [Fact]
    [DisplayName("Deve retornar BadRequest se ModelState for inválido")]
    public async Task Register_ModelStateInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var request = new CreateDonorRequest(); // incompleto
        _controller.ModelState.AddModelError("Email", "Email é obrigatório");

        // Act
        var result = await _controller.Register(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Dados inválidos.", badRequest.Value);
    }

    [Fact]
    [DisplayName("Deve retornar Ok com mensagem de sucesso ao registrar doador")]
    public async Task Register_DadosValidos_DeveRetornarMensagemSucesso()
    {
        // Arrange
        var request = new CreateDonorRequest
        {
            Email = "teste@exemplo.com",
            ZipCode = "12345678",
            BirthDate = DateTime.Today.AddYears(-25),
            Weight = 70,
            BloodType = "A",
            RhFactor = "+",
            Gender = Gender.Man
        };

        var response = new CreateDonorResponse { Message = "Doador cadastrado com sucesso. Aguarde a enfermeira chamar" };

        _donorServiceMock.Setup(x => x.RegisterDonor(It.IsAny<CreateDonorRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response.Message, okResult.Value);
    }

    [Fact]
    [DisplayName("Deve retornar lista de doadores disponíveis")]
    public async Task GetAvailableDonors_DeveRetornarLista()
    {
        // Arrange
        var donors = new List<GetAvailableDonorsResponse>
    {
        new GetAvailableDonorsResponse { Name = "João", BloodType = "A+", RhFactor = "+" }
    };

        _donorServiceMock.Setup(x => x.GetAvailableDonors()).ReturnsAsync(donors);

        // Act
        var result = await _controller.GetAvailableDonors();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsType<List<GetAvailableDonorsResponse>>(okResult.Value);
        Assert.Single(retorno);
    }

    [Fact]
    [DisplayName("Deve retornar OK com null se não houver doadores disponíveis")]
    public async Task GetAvailableDonors_SemDoadores_Disponiveis()
    {
        // Arrange
        _donorServiceMock.Setup(x => x.GetAvailableDonors()).ReturnsAsync((List<GetAvailableDonorsResponse>)null);

        // Act
        var result = await _controller.GetAvailableDonors();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
    }

}
