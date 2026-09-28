using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _context;
    
    public InventoryRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<int?> GetCoinsByUserIdAsync(int userId)
    {
        var coins = await _context.Inventories
            .Where(i => i.UserId == userId)
            .Select(i => (int?)i.Coins)
            .FirstOrDefaultAsync();
        return coins;
    }

    public async Task<Inventory?> GetWithSlotsByUserIdAsync(int userId)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefaultAsync(i => i.UserId == userId);
        return inventory;
    }
    public async Task<Inventory?> GetByUserIdAsync(int userId)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.UserId == userId);
        return inventory;
    }

    public async Task<Models.Item?> GetItemByIdAsync(int id)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<UserInventoryItemDTO>> GetUserItemsAsync(int userId)
    {
        return await _context.Slots
            .Where(s => s.Inventory.UserId == userId)
            .Select(s => new UserInventoryItemDTO
            {
                SlotId = s.Id,
                ItemId = s.ItemId,
                Quantity = s.Quantity
            })
            .ToListAsync();
    }
    
    public async Task AddItemToInventoryAsync(int userId, int itemId,int count)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.UserId == userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
        if (item == null)
        {
            throw new Exception("Item not found");
        }

        var existingSlot = await _context.Slots.FirstOrDefaultAsync(x => x.InventoryId == inventory.Id && x.ItemId == itemId);
        if (existingSlot != null)
        {
            existingSlot.Quantity += count;
        }
        else
        {
            var slot = new SlotInventory
            {
                Quantity = 1,
                Inventory = inventory,
                ItemId = itemId
            };  
            await _context.Slots.AddAsync(slot);
        }
    }

    public async Task<bool> RemoveItemAsync(int userId, int itemId, int count)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefaultAsync(i => i.UserId == userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }
        var slot = inventory.Slots.FirstOrDefault(s => s.ItemId == itemId);
        if (slot == null)
        {
            throw new Exception("Slot not found");
        }
        
        if (slot.Quantity < count)
        {
            throw new InvalidOperationException(
                $"Not enough items");
        }
        slot.Quantity -= count;

        if (slot.Quantity == 0)
        {
            _context.Slots.Remove(slot); 
        }
        return true;
    }
}