using Moq;
using Store.Application.UseCases.OrderProduct.Create;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.OrderProduct;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<IOrderProductRepository> _orderProductRepository = new();

    [TestMethod]
    public async Task Handle_ShouldCreateOrderProduct_WhenValidData()
    {
        _orderProductRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.OrderProduct item, CancellationToken _) => item);

        var handler = new Handler(_orderProductRepository.Object);
        var result = await handler.Handle(new Command(Guid.NewGuid(), Guid.NewGuid(), 2), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _orderProductRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCreateOrderProduct_WhenInvalidData()
    {
        _orderProductRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.OrderProduct item, CancellationToken _) => item);

        var handler = new Handler(_orderProductRepository.Object);
        var result = await handler.Handle(new Command(Guid.NewGuid(), Guid.NewGuid(), 0), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _orderProductRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.OrderProduct>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
