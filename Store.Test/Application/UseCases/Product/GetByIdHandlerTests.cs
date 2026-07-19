using Moq;
using Store.Application.UseCases.Product.GetById;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Product;

[TestClass]
public class GetByIdHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();

    [TestMethod]
    public async Task Handle_ShouldReturnProduct_WhenFound()
    {
        var productId = Guid.NewGuid();
        var product = Store.Domain.Entities.Product.Create("Notebook", 3500.00m, 10).Value;

        _productRepository.Setup(repository =>
                repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new Handler(_productRepository.Object);
        var result = await handler.Handle(new Command(productId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var productId = Guid.NewGuid();

        _productRepository.Setup(repository =>
                repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Product?)null);

        var handler = new Handler(_productRepository.Object);
        var result = await handler.Handle(new Command(productId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
