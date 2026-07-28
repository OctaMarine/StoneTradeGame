using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Configurations;

public class SkillCraftRecipeConfiguration : IEntityTypeConfiguration<SkillCraftRecipe>
{
    public void Configure(EntityTypeBuilder<SkillCraftRecipe> builder)
    {
        builder.ToTable("skill_craft_recipe");
        
        // 1. Составной первичный ключ
        builder.HasKey(x => new { x.CraftRecipeId, x.SkillId });

        // 2. Связь с CraftingRecipe
        // Так как в CraftingRecipe нет коллекции SkillCraftRecipes, используем пустой WithMany()
        builder.HasOne(s => s.CraftingRecipe)
            .WithMany() // <-- ПУСТО, это правильно, так как обратной навигации нет
            .HasForeignKey(s => s.CraftRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        // 3. Связь с Skill
        // Здесь мы явно указываем, что это обратная сторона свойства CraftRecipes из класса Skill
        builder.HasOne(s => s.Skill)
            .WithMany(skill => skill.CraftRecipes) // <-- Явно указываем коллекцию из Skill
            .HasForeignKey(s => s.SkillId)
            .OnDelete(DeleteBehavior.Restrict);

        // 4. Исправление типа данных для PostgreSQL
        // В твоей модели C# это float, а в конфиге было decimal(18,2). Это вызовет ошибку!
        // В PostgreSQL типу float из C# соответствует тип "real" (или "double precision" для double)
        builder.Property(x => x.LevelProgressReward)
            .HasColumnName("level_progress_reward")
            .HasColumnType("real"); 
    }
}
