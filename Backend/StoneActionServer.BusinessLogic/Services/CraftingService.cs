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

        public async Task<(bool Success, string Message, int? CraftedItemId, int? CraftedQuantity)> PerformCraftingAsync(int userId, int itemIdToCraft, int quantity)
        {
            // --- Placeholder Logic ---
            // In a real scenario, this would involve:
            // 1. Fetching the recipe for itemIdToCraft.
            // 2. Checking if the user has enough materials using _craftingRepository.CanCraftItemAsync.
            // 3. If yes, consuming materials using _craftingRepository.ConsumeMaterialsAsync.
            // 4. Adding the crafted item using _craftingRepository.AddCraftedItemAsync.
            // 5. Handling potential errors and transactions.

            // For now, simulate a successful craft.
            bool canCraft = await _craftingRepository.CanCraftItemAsync(userId, itemIdToCraft, quantity);
            if (!canCraft)
            {
                return (false, "Not enough materials to craft.", null, null);
            }

            await _craftingRepository.ConsumeMaterialsAsync(userId, itemIdToCraft, quantity);
            await _craftingRepository.AddCraftedItemAsync(userId, itemIdToCraft, quantity);

            return (true, "Item crafted successfully.", itemIdToCraft, quantity);
            // --- End Placeholder Logic ---
        }

        public async Task<List<CraftingRecipeDTO>> GetRecipes()
        {
            return await _craftingRepository.GetRecipes();
        }
    }
}