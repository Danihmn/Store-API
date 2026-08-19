using Store.Domain.ValueObjects;

namespace Store.Test.Domain.ValueObjects;

[TestClass]
public class EmailTests
{
    [TestMethod]
    public void CreateEmail_ShouldValidateEmail_WhenValidData()
    {
        const string email = "daniel.bezerra.mult@outlook.com";
        var result = Email.Create(email);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(email, result.Value.Value);
    }

    [TestMethod]
    public void CreateEmail_ShouldReturnError_WhenInvalidData()
    {
        const string email = "invalid_email";
        var result = Email.Create(email);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Invalid email", result.Errors[0].Message);
    }
}