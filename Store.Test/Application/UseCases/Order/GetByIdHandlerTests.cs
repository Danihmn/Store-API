using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Order.GetById;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Order;

[TestClass]
public class GetByIdHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldReturnOrder_WhenFound()
    {
        var orderId = Guid.NewGuid();
        var order = Store.Domain.Entities.Order.Create("pending", 150.00m, Guid.NewGuid(), Guid.NewGuid()).Value;

        _orderRepository.Setup(repository =>
                repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var handler = new Handler(_orderRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var orderId = Guid.NewGuid();

        _orderRepository.Setup(repository =>
                repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Order?)null);

        var handler = new Handler(_orderRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(orderId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
