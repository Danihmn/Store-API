using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Product.Create;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Product;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldCreateProduct_WhenValidData()
    {
        _productRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Product product, CancellationToken _) => product);

        var handler = new Handler(_productRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command("Notebook", 3500.00m, 10), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _productRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Product>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCreateProduct_WhenInvalidData()
    {
        _productRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Product product, CancellationToken _) => product);

        var handler = new Handler(_productRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command("Notebook", 0m, 10), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _productRepository.Verify(
            repository => repository.CreateAsync(It.IsAny<Store.Domain.Entities.Product>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
