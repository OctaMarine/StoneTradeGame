using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories.Crafting.Models;

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

        public async Task<bool> AddCraftedItemByContext(int userId, CraftingContext context)
        {
            
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.UserId == userId);

            if (context.ResultItemId == 0 || inventory == null)
            {
                throw new Exception("Не найден игрок или предмет");
            }
            var slot = new SlotInventory
            {
                Quantity = context.ResultQuantity,
                Inventory = inventory,
                ItemId = context.ResultItemId
            };

            await _context.Slots.AddAsync(slot);
            
            if (context.ExtraItemIds.Count > 0)
            {
                foreach (var contextExtraItemId in context.ExtraItemIds)
                {
                    var extraSlot = new SlotInventory
                    {
                        Quantity = 1,
                        Inventory = inventory,
                        ItemId = contextExtraItemId
                    };   
                    await _context.Slots.AddAsync(extraSlot);

                }   
            }
            await _context.SaveChangesAsync();
            return true;
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