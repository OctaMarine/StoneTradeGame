using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories
{
    public class CraftingRepository : ICraftingRepository
    {
        private readonly AppDbContext _context;

        public CraftingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CraftingRecipe?> GetCraftingRecipeAsync( int craftingRecipeId)
        {
            return await _context.CraftingRecipe
                .Include(r => r.RequiredItems)
                .FirstOrDefaultAsync(r => r.Id == craftingRecipeId);
        }

        public async Task<List<CraftingRecipeDTO>> GetAllRecipesAsync()
        {
            return await _context.CraftingRecipe
                .Include(r => r.RequiredItems)
                .Select(r => new CraftingRecipeDTO
                {
                    Id = r.Id,
                    ResultItemId = r.ResultItemId,
                    ResultQuantity = r.ResultQuantity,
                    ChanceOfSuccess = r.ChanceOfSuccess,
                    CraftingTimeSeconds = r.CraftingTimeSeconds,
                    CraftingType = r.CraftingType,
                    RequiredItems = r.RequiredItems.Select(i => new CraftingIngredientDTO
                    {
                        Id = i.Id,
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}