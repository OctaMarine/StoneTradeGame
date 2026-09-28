using StoneActionServer.DAL;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public InventoryService(IInventoryRepository inventoryRepository,IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int?> GetCoinsByUserIdAsync(int userId)
    {
        return await _inventoryRepository.GetCoinsByUserIdAsync(userId);
    }

    public async Task<UserMainDTO> GetUserDataAsync(int userId)
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

    public async Task<bool> GainCoinsAsync(int userId, int coins)
    {
        var inventory = await _inventoryRepository.GetByUserIdAsync(userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }
        inventory.Coins += coins;
        await  _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SpendCoinsAsync(int userId, int coins)
    {
        var inventory = await _inventoryRepository.GetByUserIdAsync(userId);
        if (inventory == null)
        {
            throw new Exception("Inventory not found");
        }

        inventory.Coins -= coins;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddSupplyAsync(int userId)
    {
        // itemid = 1 Stone
        try
        {
            await _inventoryRepository.AddItemToInventoryAsync(userId,1,1);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }

    public async Task AddItemToInventoryAsync(int userId, int itemId, int count, bool noSave = false)
    {
        await _inventoryRepository.AddItemToInventoryAsync(userId, itemId, count);
        if (!noSave)
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<bool> RemoveItem(int userId, int itemId, int count,bool noSave = false)
    {
        var result = await _inventoryRepository.RemoveItemAsync(userId, itemId, count);
        if (!noSave)
        {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }

    
}