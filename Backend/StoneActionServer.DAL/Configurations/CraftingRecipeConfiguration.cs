using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class CraftingRecipeConfiguration : IEntityTypeConfiguration<CraftingRecipe>
{
    public void Configure(EntityTypeBuilder<CraftingRecipe> builder)
    {
        builder.HasKey(x => x.Id);
    }
}