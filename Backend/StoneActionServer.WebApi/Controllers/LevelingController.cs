using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.DTO.Leveling;
using StoneActionServer.WebApi.DTO.Leveling;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LevelingController : BaseApiController
{
    private readonly ILevelingService _levelingService;

    public LevelingController(
        ILevelingService levelingService,
        ICurrentUserService currentUserService) : base(currentUserService)
    {
        _levelingService = levelingService;
    }

    [Authorize]
    [HttpGet("skills")]
    public async Task<IActionResult> GetUserSkillTree()
    {
        var skillTree = await _levelingService.GetUserSkillTreeAsync(UserId);
        return Ok(skillTree);
    }

    [Authorize]
    [HttpPost("upgrade")]
    public async Task<IActionResult> UpgradeSkill([FromBody] UpgradeSkillRequest request)
    {
        var success = await _levelingService.UpgradeSkillAsync(UserId, request.SkillId);
        
        if (!success)
        {
            return BadRequest(new { message = "Невозможно повысить уровень навыка. Проверьте условия." });
        }

        return Ok(new { message = "Навык успешно улучшен" });
    }
}