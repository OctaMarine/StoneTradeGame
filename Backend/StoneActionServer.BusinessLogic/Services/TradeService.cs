using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    
    public TradeService(ITradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }
    
    public Task<(bool,string)> Set(string userId, string itemId, int price)
    {
        return _tradeRepository.Set(userId, itemId, price);
    }

    public Task<bool> Remove(string userId, string itemId)
    {
        return _tradeRepository.Remove(userId, itemId);
    }

    public Task<bool> Complete(string userId, string itemId)
    {
        return _tradeRepository.Complete(userId, itemId);
    }
}