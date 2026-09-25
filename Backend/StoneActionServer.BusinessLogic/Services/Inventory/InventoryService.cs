using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserRepository _userRepository;
    
    public InventoryService(IInventoryRepository inventoryRepository,IUserRepository userRepository)
    {
        _inventoryRepository = inventoryRepository;
        _userRepository = userRepository;
    }
    
    public async Task<int?> GetCoinsByUserId(int userId)
    {
        return await _inventoryRepository.GetCoinsByUserId(userId);
    }

    public async Task<UserMainDTO> GetUserData(int userId)
    {
        var inventory = await _inventoryRepository.GetWithSlotsByUserIdAsync(userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }
        var coins = inventory.Coins;
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var dto = new UserMainDTO
        {
            Name = user.UserName,
            Coins = coins
        };
        return dto;
    }

    public async Task<List<UserInventoryItemDTO>> GetUserItemsAsync(int userId)
    {
        return await _inventoryRepository.GetUserItemsAsync(userId);
    }

    public async Task<bool> GainCoins(int userId, int coins)
    {
        var inventory = await _inventoryRepository.GetByUserIdAsync(userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }
        inventory.Coins += coins;
        await  _inventoryRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SpendCoins(int userId, int coins)
    {
        var inventory = await _inventoryRepository.GetByUserIdAsync(userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }

        inventory.Coins -= coins;
        await _inventoryRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddSupply(int userId)
    {
        // itemid = 1 Stone
        try
        {
            _inventoryRepository.AddItemToInventory(userId,1,1);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }

    public Task AddItemToInventory(int userId, int itemId, int count)
    {
        return _inventoryRepository.AddItemToInventory(userId, itemId, count);
    }

    public Task<bool> RemoveItem(int userId, int itemId, int count)
    {
        return _inventoryRepository.RemoveItem(userId, itemId, count);
    }

    
}