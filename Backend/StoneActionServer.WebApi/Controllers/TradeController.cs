using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Repositories;
using StoneActionServer.WebApi.DTO.Trade;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class TradeController : BaseApiController
{
    private ITradeRepository _tradeRepository;

    public TradeController(ITradeRepository tradeRepository,ICurrentUserService currentUserService) : base(currentUserService)
    {
        _tradeRepository = tradeRepository;
    }
    
    [Authorize]
    [HttpPost("settrade")]
    public async Task<IActionResult> SetTrade([FromForm] int itemId, [FromForm] int price)
    {
        var (success,id) = await _tradeRepository.Set(UserId, itemId, price);
        return Ok(id);
    }
    
    [Authorize]
    [HttpPost("buytrade")]
    public async Task<IActionResult> BuyTrade([FromBody] TradeItemRequestDTO trade)
    {
        await _tradeRepository.Complete(UserId, trade.TradeId);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("getalltrade")]
    public async Task<IActionResult> GetAllTrade()
    {
        var data = await _tradeRepository.Get();
       var dataList = data.ToList();
       return Ok(dataList);
    }
}