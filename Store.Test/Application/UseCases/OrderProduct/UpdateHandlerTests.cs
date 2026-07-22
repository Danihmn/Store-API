using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.OrderProduct.Update;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.OrderProduct;

[TestClass]
public class UpdateHandlerTests
{
    private readonly Mock<IOrderProductRepository> _orderProductRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldUpdateOrderProduct_WhenFoundAndValidData()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var item = Store.Domain.Entities.OrderProduct.Create(orderId, productId, 2).Value;

        _orderProductRepository.Setup(repository =>
                repository.GetByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        _orderProductRepository.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.OrderProduct i, CancellationToken _) => i);

        var handler = new Handler(_orderProductRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId, productId, 5), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _orderProductRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        _orderProductRepository.Setup(repository =>
                repository.GetByCompositeKeyAsync(orderId, productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.OrderProduct?)null);

        var handler = new Handler(_orderProductRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId, productId, 5), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _orderProductRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
