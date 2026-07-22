using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.User.Authenticate;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Security.Services;

namespace Store.Test.Application.UseCases.User;

[TestClass]
public class AuthenticateHandlerTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldAuthenticate_WhenCredentialsAreValid()
    {
        var hashedPassword = PasswordService.HashPassword("P@ssw0rd123");
        var user = Store.Domain.Entities.User.Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com",
            hashedPassword, true, "admin").Value;

        _userRepository.Setup(repository =>
                repository.GetByEmailAsync("daniel.bezerra.mult@outlook.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _tokenService.Setup(service => service.GenerateToken(user)).Returns("fake-jwt-token");

        var handler = new Handler(_userRepository.Object, _tokenService.Object, _logger.Object);
        var result = await handler.Handle(new Command("daniel.bezerra.mult@outlook.com", "P@ssw0rd123"),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("fake-jwt-token", result.Value.Token);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenCredentialsAreInvalid()
    {
        var hashedPassword = PasswordService.HashPassword("P@ssw0rd123");
        var user = Store.Domain.Entities.User.Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com",
            hashedPassword, true, "admin").Value;

        _userRepository.Setup(repository =>
                repository.GetByEmailAsync("daniel.bezerra.mult@outlook.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new Handler(_userRepository.Object, _tokenService.Object, _logger.Object);
        var result = await handler.Handle(new Command("daniel.bezerra.mult@outlook.com", "WrongPassword"),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _tokenService.Verify(service => service.GenerateToken(It.IsAny<Store.Domain.Entities.User>()), Times.Never);
    }
}