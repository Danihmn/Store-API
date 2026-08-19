using Store.Domain.Entities;

namespace Store.Test.Domain.Entities;

[TestClass]
public class AddressTests
{
    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidData()
    {
        var result = Address.Create("Main St", "Springfield", "SP", "12345678");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Main St", result.Value.Street);
        Assert.AreEqual("Springfield", result.Value.City);
        Assert.AreEqual("SP", result.Value.State);
        Assert.AreEqual("12345678", result.Value.ZipCode.Value);
        Assert.AreNotEqual(Guid.Empty, result.Value.Id);
        Assert.IsNotNull(result.Value.CreatedAt);
        Assert.IsNotNull(result.Value.UpdatedAt);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenStreetIsEmpty()
    {
        var result = Address.Create("", "Springfield", "SP", "12345678");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Street cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenCityIsEmpty()
    {
        var result = Address.Create("Main St", "", "SP", "12345678");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("City cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenStateIsEmpty()
    {
        var result = Address.Create("Main St", "Springfield", "", "12345678");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("State cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenStateIsLongerThanAbbreviation()
    {
        var result = Address.Create("Main St", "Springfield", "Sao Paulo", "12345678");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("State should only have the abbreviation", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenStateIsNull()
    {
        var result = Address.Create("Main St", "Springfield", null!, "12345678");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("State cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenZipCodeIsInvalid()
    {
        var result = Address.Create("Main St", "Springfield", "SP", "1234");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid zip code", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnMultipleErrors_WhenMultipleFieldsInvalid()
    {
        var result = Address.Create("", "", "Sao Paulo", "1234");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(4, result.Errors.Count);
    }

    [TestMethod]
    public void UpdateAddress_ShouldReturnOk_WhenUpdatingOnlyOneField()
    {
        var address = Address.Create("Main St", "Springfield", "SP", "12345678").Value;

        var result = address.UpdateAddress(street: "Second St");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Second St", address.Street);
        Assert.AreEqual("Springfield", address.City);
    }

    [TestMethod]
    public void UpdateAddress_ShouldReturnError_WhenZipCodeIsInvalid()
    {
        var address = Address.Create("Main St", "Springfield", "SP", "12345678").Value;

        var result = address.UpdateAddress(zipCode: "1234");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid zip code", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateAddress_ShouldReturnError_WhenStateIsLongerThanAbbreviation()
    {
        var address = Address.Create("Main St", "Springfield", "SP", "12345678").Value;

        var result = address.UpdateAddress(state: "Sao Paulo");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("State should only have the abbreviation", result.Errors[0].Message);
        Assert.AreEqual("SP", address.State);
    }

    [TestMethod]
    public void UpdateAddress_ShouldReturnMultipleErrors_WhenZipCodeAndStateAreInvalid()
    {
        var address = Address.Create("Main St", "Springfield", "SP", "12345678").Value;

        var result = address.UpdateAddress(state: "Sao Paulo", zipCode: "1234");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(2, result.Errors.Count);
    }
}
