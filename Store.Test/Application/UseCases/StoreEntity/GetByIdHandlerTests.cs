using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.StoreEntity.GetById;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.StoreEntity;

[TestClass]
public class GetByIdHandlerTests
{
    private readonly Mock<IStoreRepository> _storeRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldReturnStore_WhenFound()
    {
        var storeId = Guid.NewGuid();
        var store = Store.Domain.Entities.Store.Create("Loja Central Ltda", "11444777000161", Guid.NewGuid(),
            "Loja Central").Value;

        _storeRepository.Setup(repository =>
                repository.GetByIdAsync(storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(store);

        var handler = new Handler(_storeRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(storeId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var storeId = Guid.NewGuid();

        _storeRepository.Setup(repository =>
                repository.GetByIdAsync(storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Store?)null);

        var handler = new Handler(_storeRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(storeId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
