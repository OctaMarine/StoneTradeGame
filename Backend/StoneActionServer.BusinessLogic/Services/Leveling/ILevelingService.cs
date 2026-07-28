using StoneActionServer.DAL.DTO.Leveling;

namespace StoneActionServer.BusinessLogic.Services
{
    public interface ILevelingService
    {
        Task<List<UserSkillNodeDTO>> GetUserSkillTreeAsync(int userId);
        Task<bool> UpgradeSkillAsync(int userId, int skillId);
        Task<bool> AddProgressSkillAsync(int userId, int craftRecipeId);

    }
}
