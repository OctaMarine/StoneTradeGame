using StoneActionServer.BusinessLogic.Services.Player;
using StoneActionServer.DAL;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;
using StoneActionServer.DAL.Repositories.Item;

namespace StoneActionServer.BusinessLogic.Services;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IInventoryService _inventoryService;
    
    public TradeService(ITradeRepository tradeRepository,IItemRepository itemRepository, 
        IUserService userService,
        IInventoryService inventoryService,
        IUnitOfWork unitOfWork)
    {
        _tradeRepository = tradeRepository;
        _itemRepository = itemRepository;
        _userService = userService;
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Put(int userId, int itemId, int price)
    {
        var item = await _itemRepository.GetById(itemId);
        if (item == null)
        {
            throw new Exception("Item not found");
        }
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var tradeSlot = new TradeSlot
        {
            Price = price,
            Item = item,
            User = user
        };

        try
        {
            await _inventoryService.RemoveItem(userId, itemId,1);
            
        }
        catch (Exception e)
        {
            //TODO: добваить result ошибок Result<int> success message
            Console.WriteLine(e);
            throw;
        }

        await _tradeRepository.AddAsync(tradeSlot);
        await _unitOfWork.SaveChangesAsync();
        return tradeSlot.Id;
    }

    public Task<bool> Remove(int userId)
    {
        return _tradeRepository.Remove(userId);
    }

    public async Task<bool> Pull(int userId, int tradeId)
    {
        var coins = await _inventoryService.GetCoinsByUserId(userId);
        if (!coins.HasValue)
        {
            throw new Exception("Inventory not found");
        }
        var trade =  await _tradeRepository.GetByIdAsync(tradeId);
        if (trade == null)
        {
            throw new Exception("Trade not found");
        }
        if (coins.Value >= trade.Price)
        {
            await _tradeRepository.Remove(tradeId);
            await _inventoryService.SpendCoins(userId, trade.Price);
            await _inventoryService.AddItemToInventory(userId, trade.ItemId, 1);
            return true;
        }

        return false;
    }

    public async Task<IQueryable<TradeItemDTO>> GetAll()
    {
        return await _tradeRepository.GetAll();
    }
}