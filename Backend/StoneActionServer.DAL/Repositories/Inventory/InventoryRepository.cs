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
    
    public async Task<int?> GetCoinsByUserId(int userId)
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

    public async Task<Models.Item?> GetItemById(int id)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task AddItemToInventory(int userId, int itemId,int count)
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
        var slot = new SlotInventory
        {
            Quantity = 1,
            Inventory = inventory,
            Item = item
        };

        await _context.Slots.AddAsync(slot);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserInventoryItemDTO>> GetUserItemsAsync(int userId)
    {
        return await _context.Slots
            .Where(s => s.Inventory.UserId == userId)
            .Select(s => new UserInventoryItemDTO
            {
                Id = s.Id,
                Quantity = s.Quantity
            })
            .ToListAsync();
    }

    public async Task<bool> RemoveItem(int userId, int itemId, int count)
    {
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId == userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }
        var slot = inventory.Slots.FirstOrDefault(s => s.ItemId == itemId);
        if (slot == null)
        {
            throw new Exception("Slot not found");
        }
        slot.Quantity -= count;

        if (slot.Quantity <= 0)
        {
            _context.Slots.Remove(slot); 
        }
        return true;
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}