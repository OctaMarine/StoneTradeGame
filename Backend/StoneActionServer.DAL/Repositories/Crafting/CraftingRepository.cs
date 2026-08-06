using System.Linq;
using System.Threading.Tasks;
using StoneActionServer.DAL.Repositories;
using StoneActionServer.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public async Task<bool> CanCraftRecipe(int userId, int craftingRecipeId)
        {
            var slots = _context.Inventories
                .Include(r => r.Slots)
                .Where(x => x.UserId == userId)
                .Select(r => r.Slots)
                .FirstOrDefault()?.ToList();
            
            var requiredItems = _context.CraftingRecipe
                .Include(r => r.RequiredItems)
                .Where(r => r.Id == craftingRecipeId)
                .Select(r => r.RequiredItems).FirstOrDefault()?.ToList();
            if (slots == null || requiredItems == null)
            {
                throw new Exception("Слоты не найдены");
            }
            
            var isCan = requiredItems.All(req => 
                slots.Any(s => s.ItemId == req.ItemId && s.Quantity >= req.Quantity));

            return isCan;

        }

        public async Task<bool> ConsumeMaterials(int userId, int craftingRecipeId)
        {
            var inventory = _context.Inventories
                .Include(i => i.Slots)
                .FirstOrDefault(i => i.UserId == userId);
            
            var recipe = await _context.CraftingRecipe
                .Include(r => r.RequiredItems)
                .FirstOrDefaultAsync(r => r.Id == craftingRecipeId);
        
            foreach (var req in recipe.RequiredItems)
            {
                var slot = inventory.Slots.FirstOrDefault(s => s.ItemId == req.ItemId);
        
                if (slot == null || slot.Quantity < req.Quantity)
                {
                    return false; 
                }

                slot.Quantity -= req.Quantity;
                
                if (slot.Quantity <= 0)
                {
                    _context.Slots.Remove(slot); 
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCraftedItemByContext(int userId, CraftingContext context)
        {
            
            var inventory = _context.Inventories
                .FirstOrDefault(i => i.UserId == userId);

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
            
            if (context.ExtraItemIds != null & context.ExtraItemIds.Count > 0)
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
        
        public async Task<bool> AddCraftedItemByRecipeId(int userId, int craftingRecipeId)
        {
            var itemId = _context.CraftingRecipe
                .Where(r => r.Id == craftingRecipeId)
                .Select(r => r.ResultItemId).FirstOrDefault();
            
            var inventory = _context.Inventories
                .FirstOrDefault(i => i.UserId == userId);

            if (itemId == 0 || inventory == null)
            {
                throw new Exception("Не найден игрок или предмет");
            }
            var slot = new SlotInventory
            {
                Quantity = 1,
                Inventory = inventory,
                ItemId = itemId
            };

            await _context.Slots.AddAsync(slot);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CraftingRecipeDTO>> GetRecipes()
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