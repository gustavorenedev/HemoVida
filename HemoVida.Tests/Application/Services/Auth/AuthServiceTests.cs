using AutoMapper;
using HemoVida.Application.Auth.Request;
using HemoVida.Application.Auth.Response;
using HemoVida.Application.Auth.Service;
using HemoVida.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HemoVida.Tests.Application.Services.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockConfiguration = new Mock<IConfiguration>();

        _authService = new AuthService(_mockUserRepository.Object, _mockMapper.Object, _mockConfiguration.Object);
    }

    [Fact(DisplayName = "LoginAsync - Should return token when valid credentials are provided")]
    [Trait("Category", "AuthService")]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCredentialsAreProvided()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "user@example.com",
            Password = "validPassword"
        };

        var user = new Core.Entities.User
        {
            Id = 1,
            Email = "user@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("validPassword")
        };

        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(loginRequest.Email))
                          .ReturnsAsync(user);

        _mockConfiguration.Setup(config => config["JwtSettings:Key"]).Returns("uma-chave-super-secreta-de-pelo-menos-32-caracteres");
        _mockConfiguration.Setup(config => config["JwtSettings:ExpiresInMinutes"]).Returns("60");
        _mockConfiguration.Setup(config => config["JwtSettings:Issuer"]).Returns("testIssuer");
        _mockConfiguration.Setup(config => config["JwtSettings:Audience"]).Returns("testAudience");

        // Act
        var token = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }

    [Fact(DisplayName = "LoginAsync - Should return null when invalid credentials are provided")]
    [Trait("Category", "AuthService")]
    public async Task LoginAsync_ShouldReturnNull_WhenInvalidCredentialsAreProvided()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "user@example.com",
            Password = "invalidPassword"
        };

        var user = new Core.Entities.User
        {
            Id = 1,
            Email = "user@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("validPassword")
        };

        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(loginRequest.Email))
                          .ReturnsAsync(user);

        _mockConfiguration.Setup(config => config["JwtSettings:Key"]).Returns("uma-chave-super-secreta-de-pelo-menos-32-caracteres");
        _mockConfiguration.Setup(config => config["JwtSettings:ExpiresInMinutes"]).Returns("60");
        _mockConfiguration.Setup(config => config["JwtSettings:Issuer"]).Returns("testIssuer");
        _mockConfiguration.Setup(config => config["JwtSettings:Audience"]).Returns("testAudience");

        // Act
        var token = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.Null(token);
    }

    [Fact(DisplayName = "RegisterAsync - Should return created user response when valid data is provided")]
    [Trait("Category", "AuthService")]
    public async Task RegisterAsync_ShouldReturnCreatedUserResponse_WhenValidDataIsProvided()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Email = "newuser@example.com",
            Password = "validPassword",
            ConfirmPassword = "validPassword",
            Name = "New User"
        };

        var newUser = new Core.Entities.User
        {
            Id = 1,
            Email = createUserRequest.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password),
            Name = createUserRequest.Name
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Core.Entities.User>())).ReturnsAsync(newUser);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(mapper => mapper.Map<CreateUserResponse>(It.IsAny<Core.Entities.User>()))
                  .Returns(new CreateUserResponse
                  {
                      Id = newUser.Id,
                      Name = newUser.Name
                  });

        var authService = new AuthService(userRepositoryMock.Object, mapperMock.Object, new Mock<IConfiguration>().Object);

        // Act
        var response = await authService.RegisterAsync(createUserRequest);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(newUser.Name, response.Name);
    }

    [Fact(DisplayName = "RegisterAsync - Should throw exception when email is already in use")]
    [Trait("Category", "AuthService")]
    public async Task RegisterAsync_ShouldThrowException_WhenEmailIsAlreadyInUse()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            Email = "existinguser@example.com",
            Password = "validPassword",
            Name = "Existing User"
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(repo => repo.GetByEmailAsync(createUserRequest.Email))
                          .ReturnsAsync(new Core.Entities.User());

        var authService = new AuthService(userRepositoryMock.Object, new Mock<IMapper>().Object, new Mock<IConfiguration>().Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => authService.RegisterAsync(createUserRequest));
    }
}