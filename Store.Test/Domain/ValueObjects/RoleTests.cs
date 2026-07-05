using Store.Domain.ValueObjects;

namespace Store.Test.Domain.ValueObjects;

[TestClass]
public class RoleTests
{
    [DynamicData(nameof(GetValidRoles))]
    [TestMethod]
    public void CreateRole_ShouldValidateRole_WhenValidData (string role)
    {
        var result = Role.Create(role);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(role, result.Value.Value);
    }

    [DynamicData(nameof(GetInvalidRoles))]
    [TestMethod]
    public void CreateRole_ShouldInvalidateRole_WhenInvalidData (string role)
    {
        var result = Role.Create(role);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Invalid role", result.Errors[0].Message);
    }

    private static IEnumerable<object[]> GetValidRoles ()
    {
        yield return new object[] { "admin" };
        yield return new object[] { "seller" };
        yield return new object[] { "purchaser" };
        yield return new object[] { "stock_clerk" };
    }

    private static IEnumerable<object[]> GetInvalidRoles ()
    {
        yield return new object[] { "adm" };
        yield return new object[] { "user" };
        yield return new object[] { "manager" };
    }
}