using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public class TradeRepository : ITradeRepository
{
    private readonly AppDbContext _context;
    
    public TradeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool, string)> Set(string userId, string itemId, int price)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id.ToString() == itemId);
        var user = _context.Users.FirstOrDefault(i => i.Id.ToString() == userId);

        var tradeSlot = new TradeSlot
        {
            Id = Guid.NewGuid(),
            Price = price,
            Item = item,
            User = user
        };
        
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId.ToString() == userId);
        
        var slot = inventory.Slots.FirstOrDefault(s => s.ItemId.ToString() == itemId);
        if (slot == null)
        {
            return (false,String.Empty);
        }
        _context.Slots.Remove(slot);

        await _context.TradeSlots.AddAsync(tradeSlot);
        await _context.SaveChangesAsync();
        return (true,tradeSlot.Id.ToString());
    }

    public async Task<bool> Remove(string userId, string tradeId)
    {
        return true;
    }

    public async Task<bool> Complete(string userId, string tradeId)
    {
        var user = _context.Users
            .Include(x => x.Inventory)
            .FirstOrDefault(i => i.Id.ToString() == userId);
        var trade = _context.TradeSlots
            .Include(x=> x.Item)
            .FirstOrDefault(i => i.Id.ToString() == tradeId);

        if (user.Inventory.Coins >= trade.Price)
        {
            _context.TradeSlots.Remove(trade);
            user.Inventory.Coins -= trade.Price;

            var slot = new SlotInventory
            {
                Id = Guid.NewGuid(),
                Quantity = 1,
                Inventory = user.Inventory,
                Item = trade.Item
            };
            await _context.Slots.AddAsync(slot);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<IQueryable<TradeItemDTO>> Get()
    {
        var items = _context.TradeSlots
            .Include(s => s.User)
            .Include(s => s.Item)
            .Select(s => new TradeItemDTO
            {
                Id = s.Id.ToString(),
                ItemId = s.Item.Id.ToString(),
                Price = s.Price,
                Seller = s.User.UserName
            });
        
        return items;
    }
}