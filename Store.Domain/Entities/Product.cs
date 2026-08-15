using FluentResults;
using Store.Domain.Entities.Abstractions;
using Store.Domain.Validations;

namespace Store.Domain.Entities;

public class Product : Entity
{
    public string Description { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int? Stock { get; private set; }

    private Product(string description, decimal unitPrice, int? stock)
    {
        Description = description;
        UnitPrice = unitPrice;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Product> Create(string description, decimal unitPrice, int? stock = null)
    {
        var errors = new List<IError>();

        errors.NotEmpty(description, "Description cannot be empty");
        errors.GreaterThanZero(unitPrice, "UnitPrice must be greater than 0");
        errors.NotNegative(stock, "Stock cannot be negative");

        return errors.Count > 0 ? Result.Fail<Product>(errors) : Result.Ok(new Product(description, unitPrice, stock));
    }

    public Result UpdateProduct(string? description = null, decimal? unitPrice = null, int? stock = null)
    {
        var errors = new List<IError>();

        errors.NotEmptyIfProvided(description, "Description cannot be empty");
        errors.GreaterThanZero(unitPrice, "UnitPrice must be greater than 0");
        errors.NotNegative(stock, "Stock cannot be negative");

        if (errors.Count > 0) return Result.Fail(errors);

        Description = description ?? Description;
        UnitPrice = unitPrice ?? UnitPrice;
        Stock = stock ?? Stock;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}