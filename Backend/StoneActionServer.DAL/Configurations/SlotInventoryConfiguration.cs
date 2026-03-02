using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class SlotInventoryConfiguration : IEntityTypeConfiguration<SlotInventory>
{
    public void Configure(EntityTypeBuilder<SlotInventory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Quantity);

        builder.HasOne(x => x.Inventory)
            .WithMany(i => i.Slots)
            .HasForeignKey(s => s.InventoryId);

        builder.HasOne(x => x.Item)
            .WithMany(i => i.Slots)
            .HasForeignKey(s => s.ItemId);
    }
}