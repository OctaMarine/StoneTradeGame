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
    
    public int GetCoins(string userId)
    {
        var user = _context.Inventories.FirstOrDefault(i => i.UserId.ToString() == userId);
        var coins = user.Coins;
        return coins;
    }
    
    public UserMainDTO GetUserData(string userId)
    {
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId.ToString() == userId);
        var coins = inventory.Coins;
        var name = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId).UserName;
        var dto = new UserMainDTO
        {
            Name = name,
            Coins = coins
        };
        return dto;
    }

    public IQueryable<UserInventoryItemDTO> GetUserInventoryItems(string userId)
    {
        var inventory = _context.Slots
            .Include(s => s.Inventory)
            .Include(s => s.Item)
            .Where(s => s.Inventory.UserId.ToString() == userId)
            .Select(s => new UserInventoryItemDTO
            {
                Id = s.Item.Id.ToString(),
                Quantity = 1
            });
        
        return inventory;
    }

    public async Task<bool> GainCoins(string userId, int coins)
    {
        var inventory = _context.Inventories.FirstOrDefault(i => i.UserId.ToString() == userId);
        inventory.Coins += coins;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SpendCoins(string userId, int coins)
    {
        var inventory = _context.Inventories.FirstOrDefault(i => i.UserId.ToString() == userId);
        inventory.Coins -= coins;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> BuyItem(string userId, string itemId)
    {
        var price = 15;
        var inventory = _context.Inventories
            .FirstOrDefault(i => i.UserId.ToString() == userId);

        var item = _context.Items.FirstOrDefault(i => i.Id.ToString() == itemId);
        
        if (inventory.Coins < price)
        {
            return false;
        }
        inventory.Coins -= price;


        var slot = new SlotInventory
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Inventory = inventory,
            Item = item
        };

        await _context.Slots.AddAsync(slot);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SellItem(string userId, string itemId)
    {
        var price = 15;
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId.ToString() == userId);
        
        var slot = inventory.Slots.FirstOrDefault(s => s.ItemId.ToString() == itemId);
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