using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.OrderProduct.Delete;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.OrderProduct;

[TestClass]
public class DeleteHandlerTests
{
    private readonly Mock<IOrderProductRepository> _orderProductRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldDeleteWhenFoundOrderProduct()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var item = Store.Domain.Entities.OrderProduct.Create(orderId, productId, 2).Value;

        _orderProductRepository.Setup(repository =>
                repository.GetByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        _orderProductRepository.Setup(repository =>
            repository.DeleteByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new Handler(_orderProductRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId, productId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _orderProductRepository.Verify(
            repository => repository.DeleteByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotDeleteWhenNotFoundOrderProduct()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        _orderProductRepository.Setup(repository =>
                repository.GetByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.OrderProduct?)null);

        var handler = new Handler(_orderProductRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId, productId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _orderProductRepository.Verify(
            repository => repository.DeleteByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
