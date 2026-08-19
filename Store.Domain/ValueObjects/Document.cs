using BrDocuments;
using FluentResults;
using Store.Domain.Enums;
using Store.Domain.ValueObjects.Abstractions;

namespace Store.Domain.ValueObjects;

public record Document : ValueObject
{
    public string Value { get; }
    private EDocumentType Type { get; }

    private Document(string value, EDocumentType type)
    {
        Value = value;
        Type = type;
    }

    public static Document FromPersistence(string value)
    {
        var clean = Clean(value);
        var type = clean.Length == 11 ? EDocumentType.Cpf : EDocumentType.Cnpj;

        return new Document(value, type);
    }

    public static Result<Document> Create(string value)
    {
        var clean = Clean(value);

        var typeResult = clean.Length switch
        {
            11 => Result.Ok(EDocumentType.Cpf),
            14 => Result.Ok(EDocumentType.Cnpj),
            _ => Result.Fail<EDocumentType>("Document must have 11 (CPF) or 14 (CNPJ) digits.")
        };

        if (typeResult.IsFailed)
            return Result.Fail<Document>(typeResult.Errors);

        var type = typeResult.Value;

        var isValid = type == EDocumentType.Cpf
            ? Cpf.IsValid(clean)
            : Cnpj.IsValid(clean);

        if (!isValid)
            return Result.Fail($"{type} is invalid.");

        return Result.Ok(new Document(value, type));
    }

    private static string Clean(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Document cannot be empty.")
            : new string(value.Where(char.IsDigit).ToArray());
    }

    public override int GetHashCode() => HashCode.Combine(Value, Type);

    public override string ToString() => Value;
}