using Moq;
using Store.Application.UseCases.Product.Update;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Product;

[TestClass]
public class UpdateHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();

    [TestMethod]
    public async Task Handle_ShouldUpdateProduct_WhenFoundAndValidData()
    {
        var productId = Guid.NewGuid();
        var product = Store.Domain.Entities.Product.Create("Notebook", 3500.00m, 10).Value;

        _productRepository.Setup(repository =>
                repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _productRepository.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Product p, CancellationToken _) => p);

        var handler = new Handler(_productRepository.Object);
        var result = await handler.Handle(new Command(productId, "Notebook Pro", 4200.00m, 5), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _productRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Product>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var productId = Guid.NewGuid();

        _productRepository.Setup(repository =>
                repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Product?)null);

        var handler = new Handler(_productRepository.Object);
        var result = await handler.Handle(new Command(productId, "Notebook Pro", 4200.00m, 5), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _productRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Product>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
