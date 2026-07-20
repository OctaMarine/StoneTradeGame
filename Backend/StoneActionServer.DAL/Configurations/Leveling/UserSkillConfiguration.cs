using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.HasKey(x => new { x.UserId, x.SkillId });

        builder.Navigation(u => u.User).IsRequired(false);
        
        builder.Navigation(u => u.Skill).IsRequired(false);

        builder.Property(x => x.CurrentLevel)
            .HasDefaultValue(0)
            .HasColumnType("int");

        builder.Property(x => x.LevelProgressReward)
            .HasColumnType("decimal(18, 2)");
        
        builder.Property(x => x.IsOpen).HasDefaultValue(false);
        builder.Property(x => x.IsAvailable).HasDefaultValue(false);
    }
}
