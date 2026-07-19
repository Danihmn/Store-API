using Moq;
using Store.Application.UseCases.Address.Delete;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Address;

[TestClass]
public class DeleteHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();

    [TestMethod]
    public async Task Handle_ShouldDeleteWhenFoundAddress()
    {
        var addressId = Guid.NewGuid();
        var address = Store.Domain.Entities.Address.Create("Rua das Flores", "Sao Paulo", "SP", "12345678").Value;

        _addressRepository.Setup(repository =>
                repository.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(address);
        _addressRepository.Setup(repository =>
            repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(new Command(addressId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _addressRepository.Verify(repository => repository.DeleteAsync(addressId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotDeleteWhenNotFoundAddress()
    {
        var addressId = Guid.NewGuid();

        _addressRepository.Setup(repository =>
                repository.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Address?)null);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(new Command(addressId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _addressRepository.Verify(repository => repository.DeleteAsync(addressId, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
