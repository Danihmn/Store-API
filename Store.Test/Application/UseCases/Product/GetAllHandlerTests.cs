using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Product.GetAll;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Product;

[TestClass]
public class GetAllHandlerTests
{
    private readonly Mock<IProductRepository> _productRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldReturnProducts_WhenProductsExist()
    {
        var product = Store.Domain.Entities.Product.Create("Notebook", 3500.00m, 10).Value;

        _productRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([product]);

        var handler = new Handler(_productRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, result.Value.Count());
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNoProductsFound()
    {
        _productRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Store.Domain.Entities.Product>?)null);

        var handler = new Handler(_productRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}