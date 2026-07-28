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
        Task<bool> UpgradeSkillAsync(int userId, int skillId);
        Task<bool> AddProgressSkillAsync(int userId, int craftRecipeId);
    }
}