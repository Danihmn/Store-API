using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Domain.ValueObjects;

namespace Store.Infrastructure.Data.Mappings;

public class AddressMapping : IEntityTypeConfiguration<Address>
{
    public void Configure (EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_addresses");

        builder.ToTable("addresses", "store");

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
        builder.Property(e => e.Street)
            .HasMaxLength(200)
            .HasColumnName("street");
        builder.Property(e => e.City)
            .HasMaxLength(100)
            .HasColumnName("city");
        builder.Property(e => e.State)
            .HasMaxLength(2)
            .IsFixedLength()
            .HasColumnName("state");
        builder.Property(e => e.ZipCode)
            .HasConversion(zc => zc.Value, value => ZipCode.FromPersistence(value))
            .HasMaxLength(9)
            .HasColumnName("zip_code");
    }
}