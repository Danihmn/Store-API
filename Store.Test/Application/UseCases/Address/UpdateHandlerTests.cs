using Moq;
using Store.Application.UseCases.Address.Update;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Address;

[TestClass]
public class UpdateHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();

    [TestMethod]
    public async Task Handle_ShouldUpdateAddress_WhenFoundAndValidData()
    {
        var addressId = Guid.NewGuid();
        var address = Store.Domain.Entities.Address.Create("Rua das Flores", "Sao Paulo", "SP", "12345678").Value;

        _addressRepository.Setup(repository =>
                repository.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(address);
        _addressRepository.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Address>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Address a, CancellationToken _) => a);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(
            new Command(addressId, "Rua das Palmeiras", "Rio de Janeiro", "RJ", "87654321"), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _addressRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Address>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var addressId = Guid.NewGuid();

        _addressRepository.Setup(repository =>
                repository.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Address?)null);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(
            new Command(addressId, "Rua das Palmeiras", "Rio de Janeiro", "RJ", "87654321"), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _addressRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Address>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
