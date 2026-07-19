using FluentResults;
using Store.Domain.Validations;

namespace Store.Domain.Entities;

public class OrderProduct
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private OrderProduct(Guid orderId, Guid productId, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }

    public static Result<OrderProduct> Create(Guid orderId, Guid productId, int quantity)
    {
        var errors = new List<IError>();

        errors.NotEmpty(orderId, "InvalidOrderId", "OrderId cannot be empty");
        errors.NotEmpty(productId, "InvalidProductId", "ProductId cannot be empty");
        errors.GreaterThanZero(quantity, "InvalidQuantity", "Quantity must be greater than 0");

        return errors.Count > 0
            ? Result.Fail<OrderProduct>(errors)
            : Result.Ok(new OrderProduct(orderId, productId, quantity));
    }

    public Result UpdateQuantity(int quantity)
    {
        var errors = new List<IError>();

        errors.GreaterThanZero(quantity, "InvalidQuantity", "Quantity must be greater than 0");

        if (errors.Count > 0) return Result.Fail(errors);

        Quantity = quantity;

        return Result.Ok();
    }
}