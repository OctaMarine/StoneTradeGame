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

    public async Task<(bool, int)> Set(int userId, int itemId, int price)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == itemId);
        var user = _context.Users.FirstOrDefault(i => i.Id == userId);

        var tradeSlot = new TradeSlot
        {
            Price = price,
            Item = item,
            User = user
        };
        
        var inventory = _context.Inventories
            .Include(i => i.Slots)
            .FirstOrDefault(i => i.UserId == userId);
        
        var slot = inventory.Slots.FirstOrDefault(s => s.ItemId == itemId);
        if (slot == null)
        {
            return (false,-1);
        }
        _context.Slots.Remove(slot);

        await _context.TradeSlots.AddAsync(tradeSlot);
        await _context.SaveChangesAsync();
        return (true,tradeSlot.Id);
    }

    public async Task<bool> Remove(int userId, int tradeId)
    {
        return true;
    }

    public async Task<bool> Complete(int userId, int tradeId)
    {
        var user = _context.Users
            .Include(x => x.Inventory)
            .FirstOrDefault(i => i.Id == userId);
        var trade = _context.TradeSlots
            .Include(x=> x.Item)
            .FirstOrDefault(i => i.Id == tradeId);

        if (user.Inventory.Coins >= trade.Price)
        {
            _context.TradeSlots.Remove(trade);
            user.Inventory.Coins -= trade.Price;

            var slot = new SlotInventory
            {
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
                Id = s.Id,
                ItemId = s.Item.Id,
                Price = s.Price,
                Seller = s.User.UserName
            });
        
        return items;
    }
}