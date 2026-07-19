using Moq;
using Store.Application.UseCases.Customer.Delete;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Customer;

[TestClass]
public class DeleteHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepository = new();

    [TestMethod]
    public async Task Handle_ShouldDeleteWhenFoundCustomer()
    {
        var customerId = Guid.NewGuid();
        var customer = Store.Domain.Entities.Customer
            .Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "+5519993054611").Value;

        _customerRepository.Setup(repository =>
                repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _customerRepository.Setup(repository =>
            repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new Handler(_customerRepository.Object);
        var result = await handler.Handle(new Command(customerId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _customerRepository.Verify(repository => repository.DeleteAsync(customerId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldDeleteWhenNotFoundCustomer()
    {
        var customerId = Guid.NewGuid();

        _customerRepository.Setup(repository =>
                repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Customer?)null);

        var handler = new Handler(_customerRepository.Object);
        var result = await handler.Handle(new Command(customerId), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _customerRepository.Verify(repository => repository.DeleteAsync(customerId, It.IsAny<CancellationToken>()),
            Times.Never);
    }
}