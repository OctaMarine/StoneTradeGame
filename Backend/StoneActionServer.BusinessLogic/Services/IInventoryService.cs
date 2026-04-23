using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services;

public interface IInventoryService
{
    public int GetCoins(int userId);
    public UserMainDTO GetUserData(int userId);
    public IQueryable<UserInventoryItemDTO> GetUserInventoryItems(int userId);
    public Task<bool> GainCoins(int userId, int coins);
    public Task<bool> SpendCoins(int userId, int coins);
    public Task<bool> BuyItem(int userId, int itemId);
    public Task<bool> SellItem(int userId, int itemId);
}