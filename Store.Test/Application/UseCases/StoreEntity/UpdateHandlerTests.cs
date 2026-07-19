using Moq;
using Store.Application.UseCases.StoreEntity.Update;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.StoreEntity;

[TestClass]
public class UpdateHandlerTests
{
    private readonly Mock<IStoreRepository> _storeRepository = new();

    [TestMethod]
    public async Task Handle_ShouldUpdateStore_WhenFoundAndValidData()
    {
        var storeId = Guid.NewGuid();
        var addressId = Guid.NewGuid();
        var store = Store.Domain.Entities.Store.Create("Loja Central Ltda", "11444777000161", addressId,
            "Loja Central").Value;

        _storeRepository.Setup(repository =>
                repository.GetByIdAsync(storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(store);
        _storeRepository.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Store>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Store s, CancellationToken _) => s);

        var handler = new Handler(_storeRepository.Object);
        var result = await handler.Handle(
            new Command(storeId, "Loja Central S.A.", "Loja Central", "11444777000161", true, addressId),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _storeRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Store>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var storeId = Guid.NewGuid();
        var addressId = Guid.NewGuid();

        _storeRepository.Setup(repository =>
                repository.GetByIdAsync(storeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Store?)null);

        var handler = new Handler(_storeRepository.Object);
        var result = await handler.Handle(
            new Command(storeId, "Loja Central S.A.", "Loja Central", "11444777000161", true, addressId),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _storeRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Store>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
