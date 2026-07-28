using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.DTO.Leveling;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services
{
    public class LevelingService : ILevelingService
    {
        private readonly ILevelingRepository _levelingRepository;

        public LevelingService(ILevelingRepository levelingRepository)
        {
            _levelingRepository = levelingRepository;
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
            return await _levelingRepository.UpgradeSkillAsync(userId, skillId);
        }

        public async Task<bool> AddProgressSkillAsync(int userId, int craftRecipeId)
        {
            return await _levelingRepository.AddProgressSkillAsync(userId, craftRecipeId);
        }
    }
}