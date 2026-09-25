using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services;

public interface IInventoryService
{
    public Task<int?> GetCoinsByUserId(int userId);
    public Task<UserMainDTO> GetUserData(int userId);
    public Task<List<UserInventoryItemDTO>>  GetUserItemsAsync(int userId);
    public Task<bool> GainCoins(int userId, int coins);
    public Task<bool> SpendCoins(int userId, int coins);
    public Task<bool> AddSupply(int userId);
    public Task AddItemToInventory(int userId, int itemId, int count);
    public Task<bool> RemoveItem(int userId, int itemId, int count);
}