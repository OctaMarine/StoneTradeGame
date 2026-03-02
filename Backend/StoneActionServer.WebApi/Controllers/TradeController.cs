using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.DAL.Repositories;
using StoneActionServer.WebApi.DTO.Trade;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class TradeController : ControllerBase
{
    private ITradeRepository _tradeRepository;

    public TradeController(ITradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }
    
    [Authorize]
    [HttpPost("settrade")]
    public async Task<IActionResult> SetTrade([FromForm] string itemId, [FromForm] int price)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
        var (success,id) = await _tradeRepository.Set(userId, itemId, price);
        return Ok(id);
    }
    
    [Authorize]
    [HttpPost("buytrade")]
    public async Task<IActionResult> BuyTrade([FromBody] TradeItemData trade)
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
        await _tradeRepository.Complete(userId, trade.TradeId);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("getalltrade")]
    public async Task<IActionResult> GetAllTrade()
    {
        var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
        if (claim == null)
        {
            return BadRequest("Claim not found");
        }
        var userId = claim.Value;
        
       var data = await _tradeRepository.Get();
       var dataList = data.ToList();
       return Ok(dataList);
    }
}