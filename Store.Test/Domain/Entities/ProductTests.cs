using Store.Domain.Entities;

namespace Store.Test.Domain.Entities;

[TestClass]
public class ProductTests
{
    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidDataWithStock()
    {
        var result = Product.Create("Widget", 9.99m, 10);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Widget", result.Value.Description);
        Assert.AreEqual(9.99m, result.Value.UnitPrice);
        Assert.AreEqual(10, result.Value.Stock);
    }

    [TestMethod]
    public void Create_ShouldReturnOk_WhenStockIsNotProvided()
    {
        var result = Product.Create("Widget", 9.99m);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(result.Value.Stock);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenDescriptionIsEmpty()
    {
        var result = Product.Create("", 9.99m);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Description cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenUnitPriceIsZeroOrNegative()
    {
        var result = Product.Create("Widget", 0);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("UnitPrice must be greater than 0", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenStockIsNegative()
    {
        var result = Product.Create("Widget", 9.99m, -1);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Stock cannot be negative", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnMultipleErrors_WhenMultipleFieldsInvalid()
    {
        var result = Product.Create("", 0, -1);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(3, result.Errors.Count);
    }

    [TestMethod]
    public void UpdateProduct_ShouldReturnOk_WhenUpdatingOnlyOneField()
    {
        var product = Product.Create("Widget", 9.99m, 10).Value;

        var result = product.UpdateProduct(description: "Gadget");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Gadget", product.Description);
        Assert.AreEqual(9.99m, product.UnitPrice);
    }

    [TestMethod]
    public void UpdateProduct_ShouldReturnError_WhenDescriptionIsEmpty()
    {
        var product = Product.Create("Widget", 9.99m, 10).Value;

        var result = product.UpdateProduct(description: "");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Description cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateProduct_ShouldReturnError_WhenUnitPriceIsZeroOrNegative()
    {
        var product = Product.Create("Widget", 9.99m, 10).Value;

        var result = product.UpdateProduct(unitPrice: -5);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("UnitPrice must be greater than 0", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateProduct_ShouldReturnError_WhenStockIsNegative()
    {
        var product = Product.Create("Widget", 9.99m, 10).Value;

        var result = product.UpdateProduct(stock: -5);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Stock cannot be negative", result.Errors[0].Message);
    }
}
