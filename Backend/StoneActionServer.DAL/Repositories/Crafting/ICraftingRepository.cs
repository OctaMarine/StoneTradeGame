using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.DAL.Repositories
{
    public interface ICraftingRepository
    {
        public Task<bool> AddCraftedItemByContext(int userId, CraftingContext context);
        public Task<List<CraftingRecipeDTO>> GetAllRecipesAsync();
        public Task<CraftingRecipe?> GetCraftingRecipeAsync(int craftingRecipeId);
    }
}