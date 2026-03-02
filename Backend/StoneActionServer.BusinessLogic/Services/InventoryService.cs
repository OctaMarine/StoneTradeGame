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
    
    public int GetCoins(string userId)
    {
        return _inventoryRepository.GetCoins(userId);
    }

    public UserMainDTO GetUserData(string userId)
    {
        return _inventoryRepository.GetUserData(userId);
    }

    public IQueryable<UserInventoryItemDTO> GetUserInventoryItems(string userId)
    {
        return _inventoryRepository.GetUserInventoryItems(userId);
    }

    public async Task<bool> GainCoins(string userId, int coins)
    {
        return await _inventoryRepository.GainCoins(userId, coins);
    }

    public async Task<bool> SpendCoins(string userId, int coins)
    {
        return await _inventoryRepository.SpendCoins(userId, coins);
    }

    public async Task<bool> BuyItem(string userId, string itemId)
    {
        return await _inventoryRepository.BuyItem(userId, itemId);
    }

    public async Task<bool> SellItem(string userId, string itemId)
    {
        return await _inventoryRepository.SellItem(userId, itemId);
    }
}