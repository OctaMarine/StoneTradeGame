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

    public async Task AddAsync(TradeSlot tradeSlot)
    {
        await _context.TradeSlots.AddAsync(tradeSlot);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Remove(int tradeId)
    {
        var trade = await _context.TradeSlots.FirstOrDefaultAsync(x => x.Id == tradeId);
        if (trade == null)
        {
            throw new Exception("Trade not found");
        }
        _context.TradeSlots.Remove(trade);
        return true;
    }

    public async Task<bool> Pull(int userId, int tradeId)
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

    public async Task<IQueryable<TradeItemDTO>> GetAll()
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

    public Task<TradeSlot?> GetByIdAsync(int tradeId)
    {
        return _context.TradeSlots
            .Include(x=> x.Item)
            .FirstOrDefaultAsync(i => i.Id == tradeId);
    }
}