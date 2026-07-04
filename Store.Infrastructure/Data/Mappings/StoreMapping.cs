using Microsoft.EntityFrameworkCore;
using Store.Domain.ValueObjects;

namespace Store.Infrastructure.Data.Mappings;

public class StoreMapping : IEntityTypeConfiguration<Store.Domain.Entities.Store>
{
    public void Configure
        (Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Store.Domain.Entities.Store> builder)
    {
        builder.HasKey(e => e.Id).HasName("pk_stores");

        builder.ToTable("stores", "store");

        builder.HasIndex(e => e.Cnpj, "uq_stores_cnpj").IsUnique();

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
        builder.Property(e => e.LegalName)
            .HasMaxLength(200)
            .HasColumnName("legal_name");
        builder.Property(e => e.TradeName)
            .HasMaxLength(200)
            .HasColumnName("trade_name");
        builder.Property(e => e.Cnpj)
            .HasConversion(d => d.Value, value => Document.FromPersistence(value))
            .HasMaxLength(14)
            .IsFixedLength()
            .HasColumnName("cnpj");
        builder.Property(e => e.Active)
            .HasDefaultValue(true)
            .HasColumnName("active");
        builder.Property(e => e.AddressId)
            .HasColumnName("address_id");
    }
}
