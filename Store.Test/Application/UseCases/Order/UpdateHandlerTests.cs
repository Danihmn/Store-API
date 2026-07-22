using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Order.Update;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Order;

[TestClass]
public class UpdateHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldUpdateOrder_WhenFoundAndValidData()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var addressId = Guid.NewGuid();
        var order = Store.Domain.Entities.Order.Create("pending", 150.00m, customerId, addressId).Value;

        _orderRepository.Setup(repository =>
                repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _orderRepository.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Order o, CancellationToken _) => o);

        var handler = new Handler(_orderRepository.Object, _logger.Object);
        var result = await handler.Handle(
            new Command(orderId, "paid", 200.00m, customerId, addressId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _orderRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Order>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var addressId = Guid.NewGuid();

        _orderRepository.Setup(repository =>
                repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Order?)null);

        var handler = new Handler(_orderRepository.Object, _logger.Object);
        var result = await handler.Handle(
            new Command(orderId, "paid", 200.00m, customerId, addressId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _orderRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Order>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
