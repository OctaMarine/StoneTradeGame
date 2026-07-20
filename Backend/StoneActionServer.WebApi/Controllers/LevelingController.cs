using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
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
    [HttpPost("upgradeskill")]
    public async Task<IActionResult> UpgradeSkill([FromForm] int skillId)
    {
        var success = await _levelingService.UpgradeSkillAsync(UserId, skillId);
        
        if (!success)
        {
            return BadRequest(new { message = "Невозможно повысить уровень навыка" });
        }

        return Ok();
    }
}