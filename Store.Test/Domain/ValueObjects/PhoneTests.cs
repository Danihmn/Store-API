using Store.Domain.ValueObjects;

namespace Store.Test.Domain.ValueObjects;

[TestClass]
public class PhoneTests
{
    [TestMethod]
    public void CreatePhone_ShouldValidatePhone_WhenValidData()
    {
        const string phone = "+5519993054611";
        var result = Phone.Create(phone);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(phone, result.Value.Value);
    }

    [TestMethod]
    public void CreatePhone_ShouldReturnError_WhenInvalidData()
    {
        const string phone = "invalid_phone";
        var result = Phone.Create(phone);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Invalid phone number", result.Errors[0].Message);
    }
}