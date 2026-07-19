using Moq;
using Store.Application.UseCases.Customer.GetAll;
using Store.Domain.Repositories;

namespace Store.Test.Application.UseCases.Customer;

[TestClass]
public class GetAllHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepository = new();

    [TestMethod]
    public async Task Handle_ShouldReturnCustomers_WhenCustomersExist()
    {
        var customer = Store.Domain.Entities.Customer
            .Create("Daniel Eduardo", "daniel.bezerra.mult@outlook.com", "+5519993054611").Value;

        _customerRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([customer]);

        var handler = new Handler(_customerRepository.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(1, result.Value.Count());
    }

    [TestMethod]
    public async Task Handle_ShouldFail_WhenNoCustomersFound()
    {
        _customerRepository.Setup(repository =>
                repository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Store.Domain.Entities.Customer>?)null);

        var handler = new Handler(_customerRepository.Object);
        var result = await handler.Handle(new Command(), CancellationToken.None);

        Assert.IsTrue(result.IsFailed);
    }
}
