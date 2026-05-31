using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    public BaseApiController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    protected int UserId => _currentUserService.UserId;
    
    protected bool IsAuthenticated => _currentUserService.IsAuthenticated;
}