using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.Configurations;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.DAL;

public sealed class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<SlotInventory> Slots { get; set; }
    public DbSet<TradeSlot> TradeSlots { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new SlotInventoryConfiguration());
        modelBuilder.ApplyConfiguration(new TradeSlotConfiguration());

        
        modelBuilder.Entity<User>().HasData(
            new User { Id = Guid.NewGuid(), Email = "email@", PasswordHash = "dsf3sdf", UserName = "Oc"}
        );
        modelBuilder.Entity<Item>().HasData(
            new Item { Id = Guid.NewGuid(), Name = "Iron"}
        );
        base.OnModelCreating(modelBuilder);
    }
}