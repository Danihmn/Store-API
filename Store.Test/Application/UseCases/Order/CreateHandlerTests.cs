using Moq;
using Store.Application.UseCases.Order.Create;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Order;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();

    [TestMethod]
    public async Task Handle_ShouldCreateOrder_WhenValidData()
    {
        _orderRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Order order, CancellationToken _) => order);

        var handler = new Handler(_orderRepository.Object);
        var result = await handler.Handle(new Command(150.00m, Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _orderRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Order>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCreateOrder_WhenInvalidData()
    {
        _orderRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Order order, CancellationToken _) => order);

        var handler = new Handler(_orderRepository.Object);
        var result = await handler.Handle(new Command(0m, Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _orderRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Order>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
