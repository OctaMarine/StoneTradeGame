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

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<Skill?> GetSkillByIdAsync(int skillId,bool noTracking = false)
    {
        if (noTracking)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == skillId);
        }
        else
        {
            return await _context.Skills
                .AsNoTracking() 
                .FirstOrDefaultAsync(s => s.Id == skillId);   
        }
    }
    
    public async Task<UserSkill?> GetUserSkillAsync(int userId, int skillId)
    {
        return await _context.UserSkills
            .FirstOrDefaultAsync(u => u.UserId == userId && u.SkillId == skillId);
    }
    
    public async Task<SkillCraftRecipe?> GetSkillCraftRecipeByIdAsync(int craftRecipeId)
    {
        return await _context.SkillCraftRecipes
            .FirstOrDefaultAsync(x => x.CraftRecipeId == craftRecipeId);
    }
}