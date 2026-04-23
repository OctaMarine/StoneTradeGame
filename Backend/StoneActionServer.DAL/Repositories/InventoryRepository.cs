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
    
    public int GetCoins(int userId)
    {
        var user = _context.Inventories.FirstOrDefault(i => i.UserId == userId);
        var coins = user.Coins;
        return coins;
    }
    
    public UserMainDTO GetUserData(int userId)
    {
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId == userId);
        var coins = inventory.Coins;
        var name = _context.Users.FirstOrDefault(u => u.Id == userId).UserName;
        var dto = new UserMainDTO
        {
            Name = name,
            Coins = coins
        };
        return dto;
    }

    public IQueryable<UserInventoryItemDTO> GetUserInventoryItems(int userId)
    {
        var inventory = _context.Slots
            .Include(s => s.Inventory)
            .Include(s => s.Item)
            .Where(s => s.Inventory.UserId == userId)
            .Select(s => new UserInventoryItemDTO
            {
                Id = s.Item.Id,
                Quantity = 1
            });
        
        return inventory;
    }

    public async Task<bool> GainCoins(int userId, int coins)
    {
        var inventory = _context.Inventories.FirstOrDefault(i => i.UserId == userId);
        inventory.Coins += coins;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SpendCoins(int userId, int coins)
    {
        var inventory = _context.Inventories.FirstOrDefault(i => i.UserId == userId);
        inventory.Coins -= coins;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> BuyItem(int userId, int itemId)
    {
        var price = 15;
        var inventory = _context.Inventories
            .FirstOrDefault(i => i.UserId == userId);

        var item = _context.Items.FirstOrDefault(i => i.Id == itemId);
        
        if (inventory.Coins < price)
        {
            return false;
        }
        inventory.Coins -= price;


        var slot = new SlotInventory
        {
            Quantity = 1,
            Inventory = inventory,
            Item = item
        };

        await _context.Slots.AddAsync(slot);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SellItem(int userId, int itemId)
    {
        var price = 15;
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId == userId);
        
        var slot = inventory.Slots.FirstOrDefault(s => s.ItemId == itemId);
        if (slot == null)
        {
            return false;
        }
        _context.Slots.Remove(slot);
        inventory.Coins += price;
        await _context.SaveChangesAsync();
        return true;
    }
}