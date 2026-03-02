using StoneActionServer.DAL.DTO;

namespace StoneActionServer.DAL.Repositories;

public interface ITradeRepository
{
    public Task<(bool, string)> Set(string userId, string itemId, int price);
    public Task<bool> Remove(string userId, string tradeId);
    public Task<bool> Complete(string userId, string tradeId);

    public Task<IQueryable<TradeItemDTO>> Get();

}