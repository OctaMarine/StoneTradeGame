using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("user_skill");

        // Составной первичный ключ
        builder.HasKey(x => new { x.UserId, x.SkillId });

        // ↓↓↓ Маппинг C# свойства на реальное имя колонки в БД ↓↓↓
        builder.Property(x => x.CurrentLevel)
            .HasColumnName("level")          // ← в БД колонка "level"
            .HasColumnType("int")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Progress)
            .HasColumnName("level_progress") // ← в БД колонка "level_progress"
            .HasColumnType("decimal(18, 2)")
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.IsOpen)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .HasDefaultValue(false)
            .IsRequired();

        // Навигационные свойства (не обязательные)
        builder.Navigation(u => u.User).IsRequired(false);
        builder.Navigation(u => u.Skill).IsRequired(false);
    }
}
