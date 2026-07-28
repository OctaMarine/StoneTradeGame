using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("skill");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.Property(s => s.IconFileName)
            .HasMaxLength(255);

        builder.Property(s => s.PositionX)
            .IsRequired();

        builder.Property(s => s.PositionY)
            .IsRequired();

        builder.Property(s => s.MaxLevel)
            .IsRequired()
            .HasDefaultValue(1);

        // Self-referencing связь (родитель -> дети)
        builder.HasOne(s => s.ParentSkill)
            .WithMany(s => s.ChildrenSkills)
            .HasForeignKey(s => s.ParentSkillId)
            .OnDelete(DeleteBehavior.Restrict); // Важно: Restrict, чтобы не сломать дерево при удалении
        
    }
}
