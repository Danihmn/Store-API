using Moq;
using Store.Application.UseCases.Customer.Update;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Customer;

[TestClass]
public class UpdateHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepository = new();

    [TestMethod]
    public async Task Handle_ShouldUpdateCustomer_WhenFoundAndValidData()
    {
        var customerId = Guid.NewGuid();
        var customer = Store.Domain.Entities.Customer
            .Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "+5519993054611").Value;

        _customerRepository.Setup(repository =>
                repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _customerRepository.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Customer c, CancellationToken _) => c);

        var handler = new Handler(_customerRepository.Object);
        var result = await handler.Handle(
            new Command(customerId, "Daniel Bezerra", "daniel.bezerra.mult@outlook.com", "+5519993054611"),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _customerRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Customer>(),
                It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNotFound()
    {
        var customerId = Guid.NewGuid();

        _customerRepository.Setup(repository =>
                repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Customer?)null);

        var handler = new Handler(_customerRepository.Object);
        var result = await handler.Handle(
            new Command(customerId, "Daniel Bezerra", "daniel.bezerra.mult@outlook.com", "+5519993054611"),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _customerRepository.Verify(
            repository => repository.UpdateAsync(It.IsAny<Store.Domain.Entities.Customer>(),
                It.IsAny<CancellationToken>()), Times.Never);
    }
}
