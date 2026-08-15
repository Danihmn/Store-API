namespace Store.Domain.Entities.Abstractions;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime? CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
}
