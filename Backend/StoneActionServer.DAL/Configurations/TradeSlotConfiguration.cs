using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class TradeSlotConfiguration : IEntityTypeConfiguration<TradeSlot>
{
    public void Configure(EntityTypeBuilder<TradeSlot> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price);

        builder.HasOne(x => x.Item)
            .WithMany(i => i.TradeSlots)
            .HasForeignKey(x => x.ItemId);
        
        builder.HasOne(x => x.User)
            .WithMany(i => i.TradeSlots)
            .HasForeignKey(x => x.UserId);
    }
}