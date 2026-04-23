using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories
{
    public interface ICraftingRepository
    {
        public Task<bool> CanCraftItemAsync(int userId, int itemId, int quantity);
        public Task ConsumeMaterialsAsync(int userId, int itemId, int quantity);
        public Task AddCraftedItemAsync(int userId, int itemId, int quantity);
        public Task<List<CraftingRecipeDTO>> GetRecipes();

        
    }
}