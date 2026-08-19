using Store.Domain.ValueObjects;

namespace Store.Test.Domain.ValueObjects;

[TestClass]
public class DocumentTests
{
    [TestMethod]
    public void Document_ShouldBeValid_WhenValidNotFormattedDocumentIsProvided()
    {
        var validDocument = "60437332000160";
        var document = Document.Create(validDocument);

        Assert.IsTrue(document.IsSuccess);
        Assert.AreEqual(validDocument, document.Value.Value);
    }

    [TestMethod]
    public void Document_ShouldBeValid_WhenValidFormattedDocumentIsProvided()
    {
        const string validDocument = "99.336.499/0001-70";
        var document = Document.Create(validDocument);

        Assert.IsTrue(document.IsSuccess);
        Assert.AreEqual(validDocument, document.Value.Value);
    }

    [TestMethod]
    public void Document_ShouldBeValid_WhenInvalidDocumentIsProvided()
    {
        var invalidDocument = "1234";
        var document = Document.Create(invalidDocument);

        Assert.IsTrue(document.IsFailed);
    }
}