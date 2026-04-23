using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    
    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    
    public int GetCoins(int userId)
    {
        return _inventoryRepository.GetCoins(userId);
    }

    public UserMainDTO GetUserData(int userId)
    {
        return _inventoryRepository.GetUserData(userId);
    }

    public IQueryable<UserInventoryItemDTO> GetUserInventoryItems(int userId)
    {
        return _inventoryRepository.GetUserInventoryItems(userId);
    }

    public async Task<bool> GainCoins(int userId, int coins)
    {
        return await _inventoryRepository.GainCoins(userId, coins);
    }

    public async Task<bool> SpendCoins(int userId, int coins)
    {
        return await _inventoryRepository.SpendCoins(userId, coins);
    }

    public async Task<bool> BuyItem(int userId, int itemId)
    {
        return await _inventoryRepository.BuyItem(userId, itemId);
    }

    public async Task<bool> SellItem(int userId, int itemId)
    {
        return await _inventoryRepository.SellItem(userId, itemId);
    }
}