using StoreEntity = Store.Domain.Entities.Store;

namespace Store.Test.Domain.Entities;

[TestClass]
public class StoreTests
{
    private const string ValidCnpj = "60437332000160";

    [TestMethod]
    public void Create_ShouldReturnOk_WhenValidData()
    {
        var addressId = Guid.NewGuid();

        var result = StoreEntity.Create("Acme Ltda", ValidCnpj, addressId, "Acme");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("Acme Ltda", result.Value.LegalName);
        Assert.AreEqual("Acme", result.Value.TradeName);
        Assert.AreEqual(ValidCnpj, result.Value.Cnpj.Value);
        Assert.AreEqual(addressId, result.Value.AddressId);
        Assert.IsTrue(result.Value.Active);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenLegalNameIsEmpty()
    {
        var result = StoreEntity.Create("", ValidCnpj, Guid.NewGuid());

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("LegalName cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenAddressIdIsEmpty()
    {
        var result = StoreEntity.Create("Acme Ltda", ValidCnpj, Guid.Empty);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("AddressId cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void Create_ShouldReturnError_WhenCnpjIsInvalid()
    {
        var result = StoreEntity.Create("Acme Ltda", "12345678000199", Guid.NewGuid());

        Assert.IsTrue(result.IsFailed);
    }

    [TestMethod]
    public void UpdateStore_ShouldReturnOk_WhenUpdatingOnlyOneField()
    {
        var store = StoreEntity.Create("Acme Ltda", ValidCnpj, Guid.NewGuid()).Value;

        var result = store.UpdateStore(legalName: "New Name");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("New Name", store.LegalName);
    }

    [TestMethod]
    public void UpdateStore_ShouldReturnError_WhenCnpjIsInvalid()
    {
        var store = StoreEntity.Create("Acme Ltda", ValidCnpj, Guid.NewGuid()).Value;

        var result = store.UpdateStore(cnpj: "12345678000199");

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(ValidCnpj, store.Cnpj.Value);
    }

    [TestMethod]
    public void UpdateStore_ShouldReturnError_WhenAddressIdIsExplicitlyEmpty()
    {
        var store = StoreEntity.Create("Acme Ltda", ValidCnpj, Guid.NewGuid()).Value;

        var result = store.UpdateStore(addressId: Guid.Empty);

        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("AddressId cannot be empty", result.Errors[0].Message);
    }

    [TestMethod]
    public void UpdateStore_ShouldReturnOk_WhenAddressIdIsOmitted()
    {
        var addressId = Guid.NewGuid();
        var store = StoreEntity.Create("Acme Ltda", ValidCnpj, addressId).Value;

        var result = store.UpdateStore(legalName: "New Name");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(addressId, store.AddressId);
    }
}
