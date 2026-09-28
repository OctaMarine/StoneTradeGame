using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface ITradeRepository
{
    public Task<bool> RemoveAsync(int tradeId);
    public Task<List<TradeItemDTO>> GetAllAsync();
    public Task AddAsync(TradeSlot tradeSlot);
    public Task<TradeSlot?> GetByIdAsync(int tradeId);

}