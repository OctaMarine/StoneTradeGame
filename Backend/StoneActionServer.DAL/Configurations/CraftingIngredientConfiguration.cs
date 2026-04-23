using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class CraftingIngredientConfiguration : IEntityTypeConfiguration<CraftingIngredient>
{
    public void Configure(EntityTypeBuilder<CraftingIngredient> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Quantity);

        builder.HasOne(x => x.CraftingRecipe)
            .WithMany(i => i.RequiredItems)
            .HasForeignKey(s => s.CraftingRecipeId);
        
        builder.HasOne(x => x.Item)
            .WithMany(i => i.CraftingIngredient)
            .HasForeignKey(s => s.ItemId);
    }
}