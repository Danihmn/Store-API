using Moq;
using Store.Application.UseCases.Address.Create;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Address;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();

    [TestMethod]
    public async Task Handle_ShouldCreateAddress_WhenValidData()
    {
        _addressRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Address>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Address address, CancellationToken _) => address);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(
            new Command("Rua das Flores", "Sao Paulo", "SP", "12345678"), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _addressRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Address>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCreateAddress_WhenInvalidData()
    {
        _addressRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Address>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Address address, CancellationToken _) => address);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(
            new Command("Rua das Flores", "Sao Paulo", "SP", "invalid"), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _addressRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Address>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
