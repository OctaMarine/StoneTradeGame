using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models.Modifiers;

namespace StoneActionServer.DAL.Configurations;

public class ModifierConfiguration : IEntityTypeConfiguration<Modifier>
{
    public void Configure(EntityTypeBuilder<Modifier> builder)
    {
        builder.ToTable("skill_modifier");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequiredSkillLevel);
        builder.Property(x => x.ModifierType);
        builder.Property(x => x.Param);

        builder.HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(s => s.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CraftRecipe)
            .WithMany()
            .HasForeignKey(c => c.CraftRecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
