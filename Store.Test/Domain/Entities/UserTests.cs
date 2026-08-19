using Store.Domain.Entities;

namespace Store.Test.Domain.Entities;

[TestClass]
public class UserTests
{
    private const string ValidEmail = "daniel.bezerra.mult@outlook.com";

    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidData()
    {
        var result = User.Create("John Doe", ValidEmail, "hashed_password", true, "admin");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("John Doe", result.Value.Name);
        Assert.AreEqual(ValidEmail, result.Value.Email.Value);
        Assert.AreEqual("hashed_password", result.Value.HashedPassword);
        Assert.IsTrue(result.Value.Active);
        Assert.AreEqual("admin", result.Value.Role.Value);
    }

    [TestMethod]
    public void Create_ShouldDefaultActiveToTrue_WhenActiveIsNull()
    {
        var result = User.Create("John Doe", ValidEmail, "hashed_password", null, "admin");

        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(result.Value.Active);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenNameIsEmpty()
    {
        var result = User.Create("", ValidEmail, "hashed_password", true, "admin");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Name cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenPasswordIsEmpty()
    {
        var result = User.Create("John Doe", ValidEmail, "", true, "admin");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Password cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenEmailIsInvalid()
    {
        var result = User.Create("John Doe", "invalid_email", "hashed_password", true, "admin");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid email", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenRoleIsInvalid()
    {
        var result = User.Create("John Doe", ValidEmail, "hashed_password", true, "manager");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid role", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateUser_ShouldReturnOk_WhenUpdatingOnlyOneField()
    {
        var user = User.Create("John Doe", ValidEmail, "hashed_password", true, "admin").Value;

        var result = user.UpdateUser(name: "Jane Doe");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Jane Doe", user.Name);
    }

    [TestMethod]
    public void UpdateUser_ShouldReturnError_WhenEmailIsInvalid()
    {
        var user = User.Create("John Doe", ValidEmail, "hashed_password", true, "admin").Value;

        var result = user.UpdateUser(email: "invalid_email");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid email", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateUser_ShouldReturnError_WhenRoleIsInvalid()
    {
        var user = User.Create("John Doe", ValidEmail, "hashed_password", true, "admin").Value;

        var result = user.UpdateUser(role: "manager");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Invalid role", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateUser_ShouldReturnError_WhenNameIsEmpty()
    {
        var user = User.Create("John Doe", ValidEmail, "hashed_password", true, "admin").Value;

        var result = user.UpdateUser(name: "");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Name cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateUser_ShouldReturnError_WhenPasswordIsEmpty()
    {
        var user = User.Create("John Doe", ValidEmail, "hashed_password", true, "admin").Value;

        var result = user.UpdateUser(hashedPassword: "");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("Password cannot be empty", result.Errors[0].Message);
    }
}
