using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories
{
    public interface ICraftingRepository
    {
        public Task<List<CraftingRecipeDTO>> GetAllRecipesAsync();
        public Task<CraftingRecipe?> GetCraftingRecipeAsync(int craftingRecipeId);
    }
}