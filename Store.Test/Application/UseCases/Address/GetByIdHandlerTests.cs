using Moq;
using Store.Application.UseCases.Address.GetById;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Address;

[TestClass]
public class GetByIdHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();

    [TestMethod]
    public async Task Handle_ShouldReturnAddress_WhenFound()
    {
        var addressId = Guid.NewGuid();
        var address = Store.Domain.Entities.Address.Create("Rua das Flores", "Sao Paulo", "SP", "12345678").Value;

        _addressRepository.Setup(repository =>
                repository.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(address);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(new Command(addressId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var addressId = Guid.NewGuid();

        _addressRepository.Setup(repository =>
                repository.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Address?)null);

        var handler = new Handler(_addressRepository.Object);
        var result = await handler.Handle(new Command(addressId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
