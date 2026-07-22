using Microsoft.Extensions.Logging;
using Moq;
using Store.Application.UseCases.Customer.GetById;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Customer;

[TestClass]
public class GetByIdHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepository = new();
    private readonly Mock<ILogger<Handler>> _logger = new();

    [TestMethod]
    public async Task Handle_ShouldReturnCustomer_WhenFound()
    {
        var customerId = Guid.NewGuid();
        var customer = Store.Domain.Entities.Customer
            .Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "+5519993054611").Value;

        _customerRepository.Setup(repository =>
                repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var handler = new Handler(_customerRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(customerId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var customerId = Guid.NewGuid();

        _customerRepository.Setup(repository =>
                repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Customer?)null);

        var handler = new Handler(_customerRepository.Object, _logger.Object);
        var result = await handler.Handle(new Command(customerId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
