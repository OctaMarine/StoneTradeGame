using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class InventoryController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly IInventoryService _inventoryService;
    
    public InventoryController(IAuthService authService, IInventoryService inventoryService,ICurrentUserService currentUserService) : base(currentUserService)
    {
        _authService = authService;
        _inventoryService = inventoryService;
    }
    
    [Authorize]
    [HttpGet("userdata")]
    public async Task<IActionResult> GetUserData()
    {
        var dto = _inventoryService.GetUserData(UserId);
        return Ok(dto);
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
        Console.WriteLine(remoteIpAddress +"   - IP");
        var result = await _authService.GetAllUsers();
        return Ok(result.ToArray().Length);
    }
    
    [Authorize]
    [HttpGet("coins")]
    public async Task<IActionResult> GetUserCoins()
    {
        
        var coins = _inventoryService.GetCoins(UserId);
        return Ok(coins);
    }
    
    [Authorize]
    [HttpPost("gaincoins")]
    public async Task<IActionResult> GainUserCoins([FromForm] int coins)
    {
        await _inventoryService.GainCoins(UserId,coins);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("spendcoins")]
    public async Task<IActionResult> SpendUserCoins([FromForm] int coins)
    {
        await _inventoryService.SpendCoins(UserId, coins);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("buyitem")]
    public async Task<IActionResult> BuyItem([FromForm] int itemId)
    {
        await _inventoryService.BuyItem(UserId, itemId);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("sellitem")]
    public async Task<IActionResult> SellItem([FromForm] int itemId)
    {
        await _inventoryService.SellItem(UserId, itemId);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("userinventoryitems")]
    public async Task<IActionResult> GetUserInventoryItems()
    {
        var dto = _inventoryService.GetUserInventoryItems(UserId).ToList();
        return Ok(dto);
    }
    
    [Authorize]
    [HttpPost("addsupply")]
    public async Task<IActionResult> AddSupply()
    {
        await _inventoryService.AddSupply(UserId);
        return Ok();
    }
}