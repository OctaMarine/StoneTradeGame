using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class InventoryController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IInventoryService _inventoryService;
    
    public InventoryController(IAuthService authService, IInventoryService inventoryService)
    {
        _authService = authService;
        _inventoryService = inventoryService;
    }
    
    [Authorize]
    [HttpGet("userdata")]
    public async Task<IActionResult> GetUserData()
    {
        Console.WriteLine("Get Userdata");
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }

        var userId = claim.Value;

        var dto = _inventoryService.GetUserData(userId);
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
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }

        var userId = claim.Value;
        
        var coins = _inventoryService.GetCoins(userId);
        return Ok(coins);
    }
    
    [Authorize]
    [HttpPost("gaincoins")]
    public async Task<IActionResult> GainUserCoins([FromForm] int coins)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
        await _inventoryService.GainCoins(userId,coins);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("spendcoins")]
    public async Task<IActionResult> SpendUserCoins([FromForm] int coins)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
        await _inventoryService.SpendCoins(userId, coins);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("buyitem")]
    public async Task<IActionResult> BuyItem([FromForm] string itemId)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
        await _inventoryService.BuyItem(userId, itemId);
        return Ok();
    }
    
    [Authorize]
    [HttpPost("sellitem")]
    public async Task<IActionResult> SellItem([FromForm] string itemId)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
        await _inventoryService.SellItem(userId, itemId);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("userinventoryitems")]
    public async Task<IActionResult> GetUserInventoryItems()
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }

        var userId = claim.Value;

        var dto = _inventoryService.GetUserInventoryItems(userId).ToList();
        return Ok(dto);
    }
}