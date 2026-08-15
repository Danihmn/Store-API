using FluentResults;
using Store.Domain.Entities.Abstractions;
using Store.Domain.Validations;
using Store.Domain.ValueObjects;

namespace Store.Domain.Entities;

public class Order : Entity
{
    public Status Status { get; private set; }
    public Currency Total { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid AddressId { get; private set; }

    private Order(Status status, Currency total, Guid customerId, Guid addressId)
    {
        Status = status;
        Total = total;
        CustomerId = customerId;
        AddressId = addressId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Order> Create(string status, decimal total, Guid customerId, Guid addressId)
    {
        var errors = new List<IError>();

        errors.NotEmpty(customerId, "CustomerId cannot be empty");
        errors.NotEmpty(addressId, "AddressId cannot be empty");

        var statusResult = Status.Create(status);
        statusResult.AddErrorsTo(errors);

        var totalResult = Currency.Create(total);
        totalResult.AddErrorsTo(errors);

        return errors.Count > 0
            ? Result.Fail<Order>(errors)
            : Result.Ok(new Order(statusResult.Value, totalResult.Value, customerId, addressId));
    }

    public Result UpdateOrder(string? status = null, decimal? total = null)
    {
        var errors = new List<IError>();
        var newStatus = Status;
        var newTotal = Total;

        if (status != null)
        {
            var statusResult = Status.Create(status);
            statusResult.AddErrorsTo(errors);

            if (statusResult.IsSuccess)
                newStatus = statusResult.Value;
        }

        if (total != null)
        {
            var totalResult = Currency.Create(total.Value);
            totalResult.AddErrorsTo(errors);

            if (totalResult.IsSuccess)
                newTotal = totalResult.Value;
        }

        if (errors.Count > 0)
            return Result.Fail(errors);

        Status = newStatus;
        Total = newTotal;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}