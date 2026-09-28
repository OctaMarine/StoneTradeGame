using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services;

public interface ITradeService
{
    public Task<int> PutAsync(int userId, int itemId, int price);
    public Task<bool> RemoveAsync(int userId);
    public Task<bool> PullAsync(int userId, int itemId);
    public Task<List<TradeItemDTO>> GetAllAsync();

}