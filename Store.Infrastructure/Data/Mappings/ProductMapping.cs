using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure (EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_products");

        builder.ToTable("products", "catalog");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamptz")
            .HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamptz")
            .HasColumnName("updated_at");
        builder.Property(e => e.Description)
            .HasMaxLength(200)
            .HasColumnName("description");
        builder.Property(e => e.UnitPrice)
            .HasPrecision(10, 2)
            .HasColumnName("unit_price");
        builder.Property(e => e.Stock)
            .HasDefaultValue(0)
            .HasColumnName("stock");
    }
}
