using StoneActionServer.DAL.DTO;

namespace StoneActionServer.DAL.Repositories;

public interface ITradeRepository
{
    public Task<(bool, int)> Set(int userId, int itemId, int price);
    public Task<bool> Remove(int userId, int tradeId);
    public Task<bool> Complete(int userId, int tradeId);

    public Task<IQueryable<TradeItemDTO>> Get();

}