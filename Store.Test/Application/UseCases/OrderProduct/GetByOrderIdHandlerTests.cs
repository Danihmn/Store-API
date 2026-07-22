using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.OrderProduct.GetByOrderId;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.OrderProduct;

[TestClass]
public class GetByOrderIdHandlerTests
{
    private readonly Mock<IOrderProductRepository> _orderProductRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldReturnItems_WhenItemsExist()
    {
        var orderId = Guid.NewGuid();
        var item = Store.Domain.Entities.OrderProduct.Create(orderId, Guid.NewGuid(), 2).Value;

        _orderProductRepository.Setup(repository =>
                repository.GetByOrderIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([item]);

        var handler = new Handler(_orderProductRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, result.Value.Count());
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNoItemsFound()
    {
        var orderId = Guid.NewGuid();

        _orderProductRepository.Setup(repository =>
                repository.GetByOrderIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Store.Domain.Entities.OrderProduct>?)null);

        var handler = new Handler(_orderProductRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
