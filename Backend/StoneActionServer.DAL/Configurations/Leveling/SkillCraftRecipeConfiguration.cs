using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class SkillCraftRecipeConfiguration : IEntityTypeConfiguration<SkillCraftRecipe>
{
    public void Configure(EntityTypeBuilder<SkillCraftRecipe> builder)
    {
        builder.HasKey(x => new { x.CraftRecipeId, x.SkillId });

        builder.HasOne(s => s.CraftingRecipe)
            .WithMany()
            .HasForeignKey(s => s.CraftRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Skill)
            .WithMany()
            .HasForeignKey(s => s.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.LevelProgressReward)
            .HasColumnType("decimal(18, 2)");
    }
}
