using System.Collections.Generic;
using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.DTO.Leveling;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories
{
    public interface ILevelingRepository
    {
        Task<List<UserSkillNodeDTO>> GetUserSkillsFlatAsync(int userId);
        Task SaveChangesAsync();
        Task<UserSkill?> GetUserSkillAsync(int userId, int skillId);
        Task<SkillCraftRecipe?> GetSkillCraftRecipeByIdAsync(int craftRecipeId);
        Task<Skill?> GetSkillByIdAsync(int skillId, bool noTracking = false);
    }
}