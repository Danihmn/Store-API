using Store.Domain.Entities;

namespace Store.Test.Domain.Entities;

[TestClass]
public class OrderProductTests
{
    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidData()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var result = OrderProduct.Create(orderId, productId, 5);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(orderId, result.Value.OrderId);
        Assert.AreEqual(productId, result.Value.ProductId);
        Assert.AreEqual(5, result.Value.Quantity);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenOrderIdIsEmpty()
    {
        var result = OrderProduct.Create(Guid.Empty, Guid.NewGuid(), 5);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("OrderId cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenProductIdIsEmpty()
    {
        var result = OrderProduct.Create(Guid.NewGuid(), Guid.Empty, 5);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("ProductId cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenQuantityIsZero()
    {
        var result = OrderProduct.Create(Guid.NewGuid(), Guid.NewGuid(), 0);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Quantity must be greater than 0", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenQuantityIsNegative()
    {
        var result = OrderProduct.Create(Guid.NewGuid(), Guid.NewGuid(), -1);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Quantity must be greater than 0", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateQuantity_ShouldReturnOk_WhenQuantityIsValid()
    {
        var orderProduct = OrderProduct.Create(Guid.NewGuid(), Guid.NewGuid(), 5).Value;

        var result = orderProduct.UpdateQuantity(10);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(10, orderProduct.Quantity);
    }

    [TestMethod]
    public void UpdateQuantity_ShouldReturnError_WhenQuantityIsZeroOrNegative()
    {
        var orderProduct = OrderProduct.Create(Guid.NewGuid(), Guid.NewGuid(), 5).Value;

        var result = orderProduct.UpdateQuantity(0);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Quantity must be greater than 0", result.Errors[0].Message);
        Assert.AreEqual(5, orderProduct.Quantity);
    }
}
