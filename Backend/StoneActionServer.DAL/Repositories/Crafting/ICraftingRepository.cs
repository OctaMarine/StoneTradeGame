using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.DAL.Repositories
{
    public interface ICraftingRepository
    {
        public Task<bool> CanCraftRecipe(int userId, int craftingRecipeId);
        public Task<bool> ConsumeMaterials(int userId, int craftingRecipeId);
        public Task<bool> AddCraftedItemByContext(int userId, CraftingContext context);
        public Task<bool> AddCraftedItemByRecipeId(int userId, int craftingRecipeId);
        public Task<List<CraftingRecipeDTO>> GetRecipes();

        
    }
}