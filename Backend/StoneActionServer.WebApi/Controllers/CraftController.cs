using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StoneActionServer.WebApi.DTO;
using StoneActionServer.BusinessLogic.Services; // Assuming ICraftingService will be here

namespace StoneActionServer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class CraftController : ControllerBase
    {
        private readonly ICraftingService _craftingService;

        public CraftController(ICraftingService craftingService)
        {
            _craftingService = craftingService;
        }

        [Authorize]
        [HttpPost("craft")]
        public async Task<IActionResult> CraftItem([FromBody] CraftRequestData request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            
            int userId = 1; // Placeholder User ID

            var result = await _craftingService.PerformCraftingAsync(userId, request.ItemIdToCraft, request.Quantity);

            if (!result.Success)
            {
                return BadRequest(new CraftResponseData
                {
                    Success = false,
                    Message = result.Message
                });
            }

            return Ok(new CraftResponseData
            {
                Success = true,
                Message = result.Message,
                CraftedItemId = result.CraftedItemId,
                CraftedQuantity = result.CraftedQuantity
            });
        }
        
        [Authorize]
        [HttpGet("recipes")]
        public async Task<IActionResult> GetRecipes()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
            if (claim == null)
            {
                return BadRequest("Claim not found");
            }
            var userId = claim.Value;

            var data = await _craftingService.GetRecipes();
            return Ok(data);
        }
    }
}