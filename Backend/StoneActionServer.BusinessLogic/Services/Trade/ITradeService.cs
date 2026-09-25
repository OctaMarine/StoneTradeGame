using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services;

public interface ITradeService
{
    public Task<int> Put(int userId, int itemId, int price);
    public Task<bool> Remove(int userId);
    public Task<bool> Pull(int userId, int itemId);
    public Task<IQueryable<TradeItemDTO>> GetAll();

}