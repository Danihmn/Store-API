using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Domain.ValueObjects;

namespace Store.Infrastructure.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure (EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id).HasName("pk_users");

            builder.ToTable("users", "store");

            builder.HasIndex(e => e.Email, "uq_users_email").IsUnique();
            builder.HasIndex(e => e.Role, "chk_users_role");

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
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            builder.Property(e => e.Email)
                .HasConversion(e => e.Value, value => Email.FromPersistence(value))
                .HasMaxLength(150)
                .HasColumnName("email");
            builder.Property(e => e.HashedPassword)
                .HasColumnName("hashed_password");
            builder.Property(e => e.Active)
                .HasColumnName("active")
                .HasDefaultValueSql("true");
            builder.Property(e => e.Role)
                .HasConversion(r => r.Value, value => Role.FromPersistence(value))
                .HasMaxLength(20)
                .HasColumnName("role");
        }
    }
}
