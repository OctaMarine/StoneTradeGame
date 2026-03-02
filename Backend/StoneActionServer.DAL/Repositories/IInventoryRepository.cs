using StoneActionServer.DAL.DTO;

namespace StoneActionServer.DAL.Repositories;

public interface IInventoryRepository
{
    public int GetCoins(string userId);
    public UserMainDTO GetUserData(string userId);
    public IQueryable<UserInventoryItemDTO> GetUserInventoryItems(string userId);
    public Task<bool> GainCoins(string userId, int coins);
    public Task<bool> SpendCoins(string userId, int coins);
    public Task<bool> BuyItem(string userId, string itemId);
    public Task<bool> SellItem(string userId, string itemId);
}