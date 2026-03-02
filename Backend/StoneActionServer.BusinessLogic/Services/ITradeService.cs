namespace StoneActionServer.BusinessLogic.Services;

public interface ITradeService
{
    public Task<(bool,string)> Set(string userId, string itemId, int price);
    public Task<bool> Remove(string userId, string itemId);
    public Task<bool> Complete(string userId, string itemId);

}