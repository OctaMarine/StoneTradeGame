using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface IInventoryRepository
{
    public Task<int?> GetCoinsByUserIdAsync(int userId);
    public Task<Inventory?> GetWithSlotsByUserIdAsync(int userId);
    public Task<Inventory?> GetByUserIdAsync(int userId);
    public Task<Models.Item?> GetItemByIdAsync(int id);
    public Task AddItemToInventoryAsync(int userId, int itemId,int count);
    public Task<List<UserInventoryItemDTO>>  GetUserItemsAsync(int userId);
    public Task<bool> RemoveItemAsync(int userId, int itemId, int count);
}