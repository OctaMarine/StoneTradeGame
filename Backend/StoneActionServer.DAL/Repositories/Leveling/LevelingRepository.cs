using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.DTO.Leveling;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories.Leveling;

public class LevelingRepository : ILevelingRepository
{
    private readonly AppDbContext _context;

    public LevelingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserSkillNodeDTO>> GetUserSkillsFlatAsync(int userId)
    {
        var userSkills = await _context.UserSkills
            .AsNoTracking()
            .Where(us => us.UserId == userId)
            .ToListAsync();

        if (!userSkills.Any())
        {
            return new List<UserSkillNodeDTO>();
        }

        var skillIds = userSkills.Select(us => us.SkillId).ToList();
        
        // Загружаем все необходимые поля из таблицы skill
        var skills = await _context.Skills
            .AsNoTracking()
            .Where(s => skillIds.Contains(s.Id))
            .ToListAsync();

        var skillDict = skills.ToDictionary(s => s.Id);
        
        var result = userSkills.Select(us => 
        {
            var skillInfo = skillDict[us.SkillId];
            return new UserSkillNodeDTO
            {
                SkillId = us.SkillId,
                SkillName = skillInfo.Name,
                Description = skillInfo.Description,
                IconFileName = skillInfo.IconFileName,
                ParentSkillId = skillInfo.ParentSkillId,
                CurrentLevel = us.CurrentLevel,
                MaxLevel = skillInfo.MaxLevel,
                Progress = us.Progress,
                IsAvailable = us.IsAvailable,
                IsOpen = us.IsOpen,
                PositionX = skillInfo.PositionX,
                PositionY = skillInfo.PositionY
            };
        }).ToList();

        return result;
    }

    public async Task<bool> UpgradeSkillAsync(int userId, int skillId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var skill = await _context.Skills.AsNoTracking().FirstOrDefaultAsync(s => s.Id == skillId);
            if (skill == null) return false;

            var userSkill = await _context.UserSkills
                .FirstOrDefaultAsync(u => u.UserId == userId && u.SkillId == skillId);

            if (userSkill == null || !userSkill.IsAvailable || userSkill.Progress < 1f)
            {
                return false; // Нельзя прокачать, если нет 100% прогресса или навык недоступен
            }

            if (skill.ParentSkillId.HasValue)
            {
                var parentUserSkill = await _context.UserSkills
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.SkillId == skill.ParentSkillId.Value);

                if (parentUserSkill == null || parentUserSkill.CurrentLevel < 1)
                {
                    return false;
                }
            }

            userSkill.CurrentLevel += 1;
            userSkill.Progress = 0f; // Сбрасываем прогресс для следующего уровня (или оставь 100, если логика другая)
            userSkill.IsAvailable = false; // Блокируем до набора новых 100%
            
            _context.Set<UserSkill>().Update(userSkill);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> AddProgressSkillAsync(int userId, int craftRecipeId)
    {
        SkillCraftRecipe skillCraftRecipe;
        try
        {
            skillCraftRecipe = await _context.SkillCraftRecipes
                .FirstOrDefaultAsync(x => x.CraftRecipeId == craftRecipeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var userSkill = await _context.UserSkills
            .FirstOrDefaultAsync(u => u.UserId == userId && u.SkillId == skillCraftRecipe.SkillId);

        userSkill.Progress += skillCraftRecipe.LevelProgressReward;
        if (userSkill.Progress >= 100f)
        {
            userSkill.Progress = 100f;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}