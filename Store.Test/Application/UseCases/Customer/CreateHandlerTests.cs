using Moq;
using Store.Application.UseCases.Customer.Create;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Customer;

[TestClass]
public class CreateHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepository = new();

    [TestMethod]
    public async Task Handle_ShoudCreateCustomer_WhenValidData()
    {
        _customerRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Customer customer, CancellationToken _) => customer);

        var handler = new Handler(_customerRepository.Object);
        var result =
            await handler.Handle(new Command("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "+5519993054611"),
                CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        _customerRepository.Verify(
            repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShoudNotCreateCustomer_WhenInvalidData()
    {
        _customerRepository.Setup(repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Store.Domain.Entities.Customer customer, CancellationToken _) => customer);

        var handler = new Handler(_customerRepository.Object);
        var result =
            await handler.Handle(new Command("Daniel Eduardo", "daniel.bezerra.mult", "5519993054611"),
                CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
        _customerRepository.Verify(
            repository =>
                repository.CreateAsync(It.IsAny<Store.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}