using HemoVida.API.Controllers.Donation;
using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Response;
using HemoVida.Application.Donation.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace HemoVida.Test.Controllers.Donation;

public class DonationControllerTests
{
    private readonly DonationController _controller;
    private readonly Mock<IDonationService> _donationServiceMock;

    public DonationControllerTests()
    {
        _donationServiceMock = new Mock<IDonationService>();
        _controller = new DonationController(_donationServiceMock.Object);
    }

    [Fact(DisplayName = "Deve retornar 200 OK quando a doação for registrada com sucesso")]
    public async Task DonationRegister_DeveRetornarOk_QuandoSucesso()
    {
        // Arrange
        var request = new DonationRegisterRequest
        {
            Email = "teste@email.com",
            MlQuantity = 500,
            DonationDate = DateTime.Now
        };

        var expectedResponse = new DonationRegisterResponse
        {
            Message = "Doação registrada com sucesso!"
        };

        _donationServiceMock
            .Setup(service => service.DonationRegister(request))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.DonationRegister(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var actualResponse = Assert.IsType<DonationRegisterResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Message, actualResponse.Message);
    }

    [Fact(DisplayName = "Deve retornar 200 OK com mensagem de erro quando doador não for encontrado")]
    public async Task DonationRegister_DeveRetornarOk_QuandoDoadorNaoEncontrado()
    {
        // Arrange
        var request = new DonationRegisterRequest
        {
            Email = "naoexiste@email.com",
            MlQuantity = 500,
            DonationDate = DateTime.Now
        };

        var expectedResponse = new DonationRegisterResponse
        {
            Message = "Doador não encontrado."
        };

        _donationServiceMock
            .Setup(service => service.DonationRegister(request))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.DonationRegister(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var actualResponse = Assert.IsType<DonationRegisterResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Message, actualResponse.Message);
    }

    [Fact(DisplayName = "Deve retornar 200 OK com mensagem de erro quando a doação não puder ser registrada")]
    public async Task DonationRegister_DeveRetornarOk_QuandoErroAoRegistrar()
    {
        // Arrange
        var request = new DonationRegisterRequest
        {
            Email = "erro@email.com",
            MlQuantity = 500,
            DonationDate = DateTime.Now
        };

        var expectedResponse = new DonationRegisterResponse
        {
            Message = "Erro ao registrar doação."
        };

        _donationServiceMock
            .Setup(service => service.DonationRegister(request))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.DonationRegister(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        var actualResponse = Assert.IsType<DonationRegisterResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Message, actualResponse.Message);
    }
}