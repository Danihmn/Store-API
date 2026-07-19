using Moq;
using Store.Application.UseCases.Order.GetAll;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Order;

[TestClass]
public class GetAllHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();

    [TestMethod]
    public async Task Handle_ShouldReturnOrders_WhenOrdersExist()
    {
        var order = Store.Domain.Entities.Order.Create("pending", 150.00m, Guid.NewGuid(), Guid.NewGuid()).Value;

        _orderRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([order]);

        var handler = new Handler(_orderRepository.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, result.Value.Count());
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNoOrdersFound()
    {
        _orderRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Store.Domain.Entities.Order>?)null);

        var handler = new Handler(_orderRepository.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
