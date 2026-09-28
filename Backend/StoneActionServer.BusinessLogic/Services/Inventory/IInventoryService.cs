using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services;

public interface IInventoryService
{
    public Task<int?> GetCoinsByUserIdAsync(int userId);
    public Task<UserMainDTO> GetUserDataAsync(int userId);
    public Task<List<UserInventoryItemDTO>>  GetUserItemsAsync(int userId);
    public Task<bool> GainCoinsAsync(int userId, int coins);
    public Task<bool> SpendCoinsAsync(int userId, int coins);
    public Task<bool> AddSupplyAsync(int userId);
    public Task AddItemToInventoryAsync(int userId, int itemId, int count, bool noSave = false);
    public Task<bool> RemoveItem(int userId, int itemId, int count,bool noSave = false);
}