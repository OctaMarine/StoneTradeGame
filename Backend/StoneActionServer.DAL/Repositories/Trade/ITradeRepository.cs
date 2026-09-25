using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface ITradeRepository
{
    public Task<bool> Remove(int tradeId);
    public Task<bool> Pull(int userId, int tradeId);

    public Task<IQueryable<TradeItemDTO>> GetAll();
    public Task AddAsync(TradeSlot tradeSlot);
    public Task<TradeSlot?> GetByIdAsync(int tradeId);

}