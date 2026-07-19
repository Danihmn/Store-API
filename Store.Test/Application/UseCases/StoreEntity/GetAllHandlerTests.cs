using Moq;
using Store.Application.UseCases.StoreEntity.GetAll;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.StoreEntity;

[TestClass]
public class GetAllHandlerTests
{
    private readonly Mock<IStoreRepository> _storeRepository = new();

    [TestMethod]
    public async Task Handle_ShouldReturnStores_WhenStoresExist()
    {
        var store = Store.Domain.Entities.Store.Create("Loja Central Ltda", "11444777000161", Guid.NewGuid(),
            "Loja Central").Value;

        _storeRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([store]);

        var handler = new Handler(_storeRepository.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, result.Value.Count());
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNoStoresFound()
    {
        _storeRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Store.Domain.Entities.Store>?)null);

        var handler = new Handler(_storeRepository.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
