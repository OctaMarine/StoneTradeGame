namespace StoneActionServer.BusinessLogic.Services;

public interface ITradeService
{
    public Task<(bool,int)> Set(int userId, int itemId, int price);
    public Task<bool> Remove(int userId, int itemId);
    public Task<bool> Complete(int userId, int itemId);

}