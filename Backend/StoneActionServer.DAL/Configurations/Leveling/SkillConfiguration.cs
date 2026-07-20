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

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasColumnType("text") 
            .IsRequired();


        builder.Property(s => s.ParentSkillId)
            .HasColumnName("parent_skill");
        
        builder.HasOne(s => s.ParentSkill)
            .WithMany(s => s.ChildrenSkills)
            .HasForeignKey(s => s.ParentSkillId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
