using Moq;
using Store.Application.UseCases.Order.Delete;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Order;

[TestClass]
public class DeleteHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();

    [TestMethod]
    public async Task Handle_ShouldDeleteWhenFoundOrder()
    {
        var orderId = Guid.NewGuid();
        var order = Store.Domain.Entities.Order.Create("pending", 150.00m, Guid.NewGuid(), Guid.NewGuid()).Value;

        _orderRepository.Setup(repository =>
                repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _orderRepository.Setup(repository =>
            repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new Handler(_orderRepository.Object);
        var result = await handler.Handle(new Command(orderId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _orderRepository.Verify(repository => repository.DeleteAsync(orderId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotDeleteWhenNotFoundOrder()
    {
        var orderId = Guid.NewGuid();

        _orderRepository.Setup(repository =>
                repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Order?)null);

        var handler = new Handler(_orderRepository.Object);
        var result = await handler.Handle(new Command(orderId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _orderRepository.Verify(repository => repository.DeleteAsync(orderId, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
