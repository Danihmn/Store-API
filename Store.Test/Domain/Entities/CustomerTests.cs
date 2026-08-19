using Store.Domain.Entities;

namespace Store.Test.Domain.Entities;

[TestClass]
public class CustomerTests
{
    private const string ValidEmail = "daniel.bezerra.mult@outlook.com";
    private const string ValidPhone = "+5519993054611";

    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidData()
    {
        var result = Customer.Create("John Doe", ValidEmail, ValidPhone);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("John Doe", result.Value.Name);
        Assert.AreEqual(ValidEmail, result.Value.Email.Value);
        Assert.AreEqual(ValidPhone, result.Value.Phone.Value);
        Assert.AreNotEqual(Guid.Empty, result.Value.Id);
        Assert.IsNotNull(result.Value.CreatedAt);
        Assert.IsNotNull(result.Value.UpdatedAt);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenNameIsEmpty()
    {
        var result = Customer.Create("", ValidEmail, ValidPhone);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Name cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenEmailIsInvalid()
    {
        var result = Customer.Create("John Doe", "invalid_email", ValidPhone);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid email", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenPhoneIsInvalid()
    {
        var result = Customer.Create("John Doe", ValidEmail, "invalid_phone");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid phone number", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnMultipleErrors_WhenAllFieldsInvalid()
    {
        var result = Customer.Create("", "invalid_email", "invalid_phone");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(3, result.Errors.Count);
    }

    [TestMethod]
    public void UpdateCustomer_ShouldReturnOk_WhenUpdatingOnlyOneField()
    {
        var customer = Customer.Create("John Doe", ValidEmail, ValidPhone).Value;

        var result = customer.UpdateCustomer(name: "Jane Doe");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Jane Doe", customer.Name);
        Assert.AreEqual(ValidEmail, customer.Email.Value);
    }

    [TestMethod]
    public void UpdateCustomer_ShouldReturnError_WhenEmailIsInvalid()
    {
        var customer = Customer.Create("John Doe", ValidEmail, ValidPhone).Value;

        var result = customer.UpdateCustomer(email: "invalid_email");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid email", result.Errors[0].Message);
        Assert.AreEqual(ValidEmail, customer.Email.Value);
    }

    [TestMethod]
    public void UpdateCustomer_ShouldReturnError_WhenPhoneIsInvalid()
    {
        var customer = Customer.Create("John Doe", ValidEmail, ValidPhone).Value;

        var result = customer.UpdateCustomer(phone: "invalid_phone");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid phone number", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateCustomer_ShouldReturnError_WhenNameIsEmpty()
    {
        var customer = Customer.Create("John Doe", ValidEmail, ValidPhone).Value;

        var result = customer.UpdateCustomer(name: "");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Name cannot be empty", result.Errors[0].Message);
    }
}
