using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class LevelingController : BaseApiController
{
    public LevelingController(ILevelingService levelingService) : base(levelingService)
    {
        
    }
}