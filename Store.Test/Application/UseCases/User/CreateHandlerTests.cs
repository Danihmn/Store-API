using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.User.Create;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;

namespace Store.Test.Application.UseCases.User;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldCreateUser_WhenEmailNotInUse()
    {
        _userRepository.Setup(repository =>
                repository.GetByEmailAsync("daniel.bezerra.mult@outlook.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.User?)null);
        _userRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.User user, CancellationToken _) => user);
        _tokenService.Setup(service => service.GenerateToken(It.IsAny<Store.Domain.Entities.User>()))
            .Returns("fake-jwt-token");

        var handler = new Handler(_userRepository.Object, _tokenService.Object, _logger.Object);
        var result = await handler.Handle(
            new Command("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "P@ssw0rd123", true, "admin"),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _userRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.User>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCreateUser_WhenEmailAlreadyExists()
    {
        var existingUser = Store.Domain.Entities.User.Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com",
            "hashed-password", true, "admin").Value;

        _userRepository.Setup(repository =>
                repository.GetByEmailAsync("daniel.bezerra.mult@outlook.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var handler = new Handler(_userRepository.Object, _tokenService.Object, _logger.Object);
        var result = await handler.Handle(
            new Command("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "P@ssw0rd123", true, "admin"),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _userRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.User>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}