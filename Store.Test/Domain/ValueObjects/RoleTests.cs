using Store.Domain.ValueObjects;

namespace Store.Test.Domain.ValueObjects;

[TestClass]
public class RoleTests
{
    [DynamicData(nameof(GetValidRoles))]
    [TestMethod]
    public void CreateRole_ShouldValidateRole_WhenValidData(string role)
    {
        var result = Role.Create(role);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(role, result.Value.Value);
    }

    [DynamicData(nameof(GetInvalidRoles))]
    [TestMethod]
    public void CreateRole_ShouldInvalidateRole_WhenInvalidData(string role)
    {
        var result = Role.Create(role);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Invalid role", result.Errors[0].Message);
    }

    private static IEnumerable<object[]> GetValidRoles()
    {
        yield return ["admin"];
        yield return ["seller"];
        yield return ["purchaser"];
        yield return ["stock_clerk"];
    }

    private static IEnumerable<object[]> GetInvalidRoles()
    {
        yield return ["adm"];
        yield return ["user"];
        yield return ["manager"];
    }
}