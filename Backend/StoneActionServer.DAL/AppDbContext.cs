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
    public DbSet<CraftingRecipe> CraftingRecipe { get; set; }
    public DbSet<CraftingIngredient> CraftingIngredient { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new SlotInventoryConfiguration());
        modelBuilder.ApplyConfiguration(new TradeSlotConfiguration());
        modelBuilder.ApplyConfiguration(new CraftingRecipeConfiguration());
        modelBuilder.ApplyConfiguration(new CraftingIngredientConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}