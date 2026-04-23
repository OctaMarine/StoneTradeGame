using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.BusinessLogic.Services
{
    public interface ICraftingService
    {
        public Task<(bool Success, string Message, int? CraftedItemId, int? CraftedQuantity)> PerformCraftingAsync(int userId, int itemIdToCraft, int quantity);
        public Task<List<CraftingRecipeDTO>> GetRecipes();
    }
}