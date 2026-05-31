using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services
{
    public class CraftingService : ICraftingService
    {
        private readonly ICraftingRepository _craftingRepository;

        public CraftingService(ICraftingRepository craftingRepository)
        {
            _craftingRepository = craftingRepository;
        }

        public async Task<bool> PerformCrafting(int userId, int craftingRecipeId)
        {
            var canCraft = await _craftingRepository.CanCraftRecipe(userId, craftingRecipeId);
            if (!canCraft)
            {
                return false;
            }
            var isConsume =  await _craftingRepository.ConsumeMaterials(userId, craftingRecipeId);
            if (!isConsume)
            {
                return false;
            }
            
            await _craftingRepository.AddCraftedItem(userId, craftingRecipeId);

            return true;

        }

        public async Task<List<CraftingRecipeDTO>> GetRecipes()
        {
            return await _craftingRepository.GetRecipes();
        }
    }
}