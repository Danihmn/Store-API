using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Product.Delete;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Product;

[TestClass]
public class DeleteHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldDeleteWhenFoundProduct()
    {
        var productId = Guid.NewGuid();
        var product = Store.Domain.Entities.Product.Create("Notebook", 3500.00m, 10).Value;

        _productRepository.Setup(repository =>
                repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _productRepository.Setup(repository =>
            repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new Handler(_productRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(productId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _productRepository.Verify(repository => repository.DeleteAsync(productId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotDeleteWhenNotFoundProduct()
    {
        var productId = Guid.NewGuid();

        _productRepository.Setup(repository =>
                repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Product?)null);

        var handler = new Handler(_productRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(productId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _productRepository.Verify(repository => repository.DeleteAsync(productId, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}