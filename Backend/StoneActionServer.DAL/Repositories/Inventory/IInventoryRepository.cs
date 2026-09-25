using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface IInventoryRepository
{
    public Task<int?> GetCoinsByUserId(int userId);
    public Task<Inventory?> GetWithSlotsByUserIdAsync(int userId);
    public Task<Inventory?> GetByUserIdAsync(int userId);
    public Task<Models.Item?> GetItemById(int id);
    public Task AddItemToInventory(int userId, int itemId,int count);
    public Task<List<UserInventoryItemDTO>>  GetUserItemsAsync(int userId);
    public Task<bool> RemoveItem(int userId, int itemId, int count);
    public Task SaveChangesAsync();
}