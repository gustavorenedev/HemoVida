using HemoVida.API.Controllers.Auth;
using HemoVida.Application.Auth.Request;
using HemoVida.Application.Auth.Response;
using HemoVida.Application.Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HemoVida.Tests.Controllers.Auth;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();

        _authController = new AuthController(_mockAuthService.Object);
    }

    [Fact(DisplayName = "Register - Should return created user response when valid data is provided")]
    [Trait("Category", "AuthController")]
    public async Task Register_ShouldReturnCreatedUserResponse_WhenValidDataIsProvided()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Email = "newuser@example.com",
            Password = "validPassword",
            Name = "New User"
        };

        var createUserResponse = new CreateUserResponse
        {
            Id = 1,
            Name = createUserRequest.Name
        };

        _mockAuthService.Setup(service => service.RegisterAsync(It.IsAny<CreateUserRequest>()))
                       .ReturnsAsync(createUserResponse);

        // Act
        var result = await _authController.Register(createUserRequest);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<CreateUserResponse>(createdResult.Value);
        Assert.Equal(createUserResponse.Name, returnValue.Name);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact(DisplayName = "Register - Should return BadRequest when data is invalid")]
    [Trait("Category", "AuthController")]
    public async Task Register_ShouldReturnBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Email = "", // Dados inválidos: Email vazio
            Password = "validPassword",
            Name = "New User"
        };

        _authController.ModelState.AddModelError("Email", "Email is required.");

        // Act
        var result = await _authController.Register(createUserRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Dados inválidos.", badRequestResult.Value);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact(DisplayName = "Login - Should return token when valid credentials are provided")]
    [Trait("Category", "AuthController")]
    public async Task Login_ShouldReturnToken_WhenValidCredentialsAreProvided()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "existinguser@example.com",
            Password = "validPassword"
        };

        var expectedToken = "valid-jwt-token";

        _mockAuthService.Setup(service => service.LoginAsync(It.IsAny<LoginUserRequest>()))
                       .ReturnsAsync(expectedToken);

        // Act
        var result = await _authController.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedToken, okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact(DisplayName = "Login - Should return Unauthorized when credentials are invalid")]
    [Trait("Category", "AuthController")]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "nonexistentuser@example.com",
            Password = "wrongPassword"
        };

        _mockAuthService.Setup(service => service.LoginAsync(It.IsAny<LoginUserRequest>()))
                       .ReturnsAsync((string)null!);

        // Act
        var result = await _authController.Login(loginRequest);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Credenciais inválidas.", unauthorizedResult.Value);
        Assert.Equal(401, unauthorizedResult.StatusCode);
    }
}