using System.Threading.Tasks;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.BusinessLogic.Services
{
    public interface ICraftingService
    {
        public Task<bool> PerformCrafting(int userId, int craftingRecipeId);
        public Task<List<CraftingRecipeDTO>> GetRecipes();
    }
}