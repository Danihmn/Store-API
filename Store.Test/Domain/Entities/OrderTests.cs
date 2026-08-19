using Store.Domain.Entities;

namespace Store.Test.Domain.Entities;

[TestClass]
public class OrderTests
{
    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidData()
    {
        var customerId = Guid.NewGuid();
        var addressId = Guid.NewGuid();

        var result = Order.Create("Pending", 100, customerId, addressId);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(customerId, result.Value.CustomerId);
        Assert.AreEqual(addressId, result.Value.AddressId);
        Assert.AreEqual(100, result.Value.Total.Value);
        Assert.AreNotEqual(Guid.Empty, result.Value.Id);
        Assert.IsNotNull(result.Value.CreatedAt);
        Assert.IsNotNull(result.Value.UpdatedAt);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenCustomerIdIsEmpty()
    {
        var result = Order.Create("Pending", 100, Guid.Empty, Guid.NewGuid());

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("CustomerId cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenAddressIdIsEmpty()
    {
        var result = Order.Create("Pending", 100, Guid.NewGuid(), Guid.Empty);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("AddressId cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenStatusIsInvalid()
    {
        var result = Order.Create("Unknown", 100, Guid.NewGuid(), Guid.NewGuid());

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid status", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenTotalIsZeroOrNegative()
    {
        var result = Order.Create("Pending", 0, Guid.NewGuid(), Guid.NewGuid());

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Amount must be greater than 0", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnMultipleErrors_WhenMultipleFieldsInvalid()
    {
        var result = Order.Create("Unknown", 0, Guid.Empty, Guid.Empty);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(4, result.Errors.Count);
    }

    [TestMethod]
    public void UpdateOrder_ShouldReturnOk_WhenUpdatingOnlyStatus()
    {
        var order = Order.Create("Pending", 100, Guid.NewGuid(), Guid.NewGuid()).Value;

        var result = order.UpdateOrder(status: "Paid");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Store.Domain.Enums.EStatus.Paid, order.Status.Value);
        Assert.AreEqual(100, order.Total.Value);
    }

    [TestMethod]
    public void UpdateOrder_ShouldReturnError_WhenStatusIsInvalid()
    {
        var order = Order.Create("Pending", 100, Guid.NewGuid(), Guid.NewGuid()).Value;

        var result = order.UpdateOrder(status: "Unknown");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid status", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateOrder_ShouldReturnError_WhenTotalIsZeroOrNegative()
    {
        var order = Order.Create("Pending", 100, Guid.NewGuid(), Guid.NewGuid()).Value;

        var result = order.UpdateOrder(total: -10);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Amount must be greater than 0", result.Errors[0].Message);
    }
}
