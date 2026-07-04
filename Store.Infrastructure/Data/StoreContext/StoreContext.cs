using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;

namespace Store.Infrastructure.Data.StoreContext;

public class StoreContext (DbContextOptions<StoreContext> options) : DbContext(options)
{
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Store.Domain.Entities.Store> Stores { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);
}
