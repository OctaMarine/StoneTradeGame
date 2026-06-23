using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StoneActionServer.WebApi.DTO;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class CraftController : BaseApiController
    {
        private readonly ICraftingService _craftingService;

        public CraftController(ICraftingService craftingService,ICurrentUserService currentUserService) : base(currentUserService)
        {
            _craftingService = craftingService;
        }

        [Authorize]
        [HttpPost("craft")]
        public async Task<IActionResult> CraftItem([FromBody] CraftItemRequest itemRequest)
        {
            if (itemRequest == null)
            {
                return BadRequest("Invalid request data.");
            }
            
            var result = await _craftingService.PerformCrafting(UserId, itemRequest.CraftingRecipeId);

            if (!result)
            {
                return Ok(new { result = false });;
            }

            return Ok(new { result = true });
        }
        
        [Authorize]
        [HttpGet("recipes")]
        public async Task<IActionResult> GetRecipes()
        {
            var data = await _craftingService.GetRecipes();
            return Ok(data);
        }
    }
}