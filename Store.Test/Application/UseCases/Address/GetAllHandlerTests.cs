using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Address.GetAll;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Address;

[TestClass]
public class GetAllHandlerTests
{
    private readonly Mock<IAddressRepository> _addressRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldReturnAddresses_WhenAddressesExist()
    {
        var address = Store.Domain.Entities.Address.Create("Rua das Flores", "Sao Paulo", "SP", "12345678").Value;

        _addressRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([address]);

        var handler = new Handler(_addressRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, result.Value.Count());
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNoAddressesFound()
    {
        _addressRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Store.Domain.Entities.Address>?)null);

        var handler = new Handler(_addressRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
