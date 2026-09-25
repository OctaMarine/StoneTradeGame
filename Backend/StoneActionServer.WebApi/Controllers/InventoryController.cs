using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1/inventory")]
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
        var dto = await _inventoryService.GetUserData(UserId);
        return Ok(dto);
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _authService.GetAllUsers();
        return Ok(result.ToArray().Length);
    }
    
    [Authorize]
    [HttpGet("coins")]
    public async Task<IActionResult> GetUserCoins()
    {
        
        var coins = await _inventoryService.GetCoinsByUserId(UserId);
        if (!coins.HasValue)
        {
            return BadRequest("Not found coins");
        }
        return Ok(coins.Value);
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
    [HttpGet("userinventoryitems")]
    public async Task<IActionResult> GetUserInventoryItems()
    {
        var dto = await _inventoryService.GetUserItemsAsync(UserId);
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