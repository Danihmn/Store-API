using Moq;
using Store.Application.UseCases.StoreEntity.Create;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.StoreEntity;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<IStoreRepository> _storeRepository = new();

    [TestMethod]
    public async Task Handle_ShouldCreateStore_WhenValidData()
    {
        _storeRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Store>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Store store, CancellationToken _) => store);

        var handler = new Handler(_storeRepository.Object);
        var result = await handler.Handle(
            new Command("Loja Central Ltda", "Loja Central", "11444777000161", true, Guid.NewGuid()),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _storeRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Store>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCreateStore_WhenInvalidData()
    {
        _storeRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Store>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Store store, CancellationToken _) => store);

        var handler = new Handler(_storeRepository.Object);
        var result = await handler.Handle(
            new Command("Loja Central Ltda", "Loja Central", "invalid", true, Guid.NewGuid()),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _storeRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Store>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
