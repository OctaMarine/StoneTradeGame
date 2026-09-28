using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Repositories;
using StoneActionServer.WebApi.DTO.Trade;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1/trade")]
public class TradeController : BaseApiController
{
    private ITradeService _tradeService;

    public TradeController(ITradeService tradeService,ICurrentUserService currentUserService) : base(currentUserService)
    {
        _tradeService = tradeService;
    }
    
    [Authorize]
    [HttpPost("settrade")]
    public async Task<IActionResult> SetTrade([FromForm] int itemId, [FromForm] int price)
    {
        var id = await _tradeService.PutAsync(UserId, itemId, price);
        return Ok(id);
    }
    
    [Authorize]
    [HttpPost("buytrade")]
    public async Task<IActionResult> BuyTrade([FromBody] TradeItemRequestDTO trade)
    {
        await _tradeService.PullAsync(UserId, trade.TradeId);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("getalltrade")]
    public async Task<IActionResult> GetAllTrade()
    {
        var data = await _tradeService.GetAllAsync();
        return Ok(data);
    }
}