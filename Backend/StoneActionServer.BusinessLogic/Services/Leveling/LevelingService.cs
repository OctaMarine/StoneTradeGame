using StoneActionServer.DAL;
using StoneActionServer.DAL.DTO.Leveling;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services
{
    public class LevelingService : ILevelingService
    {
        private readonly ILevelingRepository _levelingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LevelingService(ILevelingRepository levelingRepository, IUnitOfWork unitOfWork)
        {
            _levelingRepository = levelingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserSkillNodeDTO>> GetUserSkillTreeAsync(int userId)
        {
            var flatSkills = await _levelingRepository.GetUserSkillsFlatAsync(userId);

            if (flatSkills == null || !flatSkills.Any())
            {
                return new List<UserSkillNodeDTO>();
            }

            var skillNodes = flatSkills.ToDictionary(s => s.SkillId);
            var rootNodes = new List<UserSkillNodeDTO>();

            foreach (var node in skillNodes.Values)
            {
                if (node.ParentSkillId.HasValue && skillNodes.TryGetValue(node.ParentSkillId.Value, out var parent))
                {
                    parent.Children.Add(node);
                }
                else
                {
                    rootNodes.Add(node);
                }
            }

            return rootNodes;
        }

        public async Task<bool> UpgradeSkillAsync(int userId, int skillId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var skill = await _levelingRepository.GetSkillByIdAsync(skillId,true);
                if (skill == null) return false;

                var userSkill = await _levelingRepository.GetUserSkillAsync(userId,skillId);

                if (userSkill == null || !userSkill.IsAvailable || userSkill.Progress < 1f)
                {
                    return false;
                }

                if (skill.ParentSkillId.HasValue)
                {
                    var parentUserSkill = await _levelingRepository.GetUserSkillAsync(userId,skill.ParentSkillId.Value);
                    if (parentUserSkill == null || parentUserSkill.CurrentLevel < 1)
                    {
                        return false;
                    }
                }

                userSkill.CurrentLevel += 1;
                userSkill.Progress = 0f;
                userSkill.IsAvailable = false; // Блокируем до набора новых 100%
            
                await _levelingRepository.SaveChangesAsync();
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
            var skillCraftRecipe = await _levelingRepository.GetSkillCraftRecipeByIdAsync(craftRecipeId);

            if (skillCraftRecipe == null) 
                return false;
        
            var userSkill = await _levelingRepository.GetUserSkillAsync(userId,skillCraftRecipe.SkillId);

            if (userSkill == null) 
                return false;
        
            userSkill.Progress += skillCraftRecipe.LevelProgressReward;
        
            if (userSkill.Progress >= 1f)
            {
                userSkill.Progress = 1f;
            }

            await _levelingRepository.SaveChangesAsync();
            return true;
        }
    }
}