using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly IInventoryService _inventoryService;
    
    public UserController(IAuthService authService, IInventoryService inventoryService,ICurrentUserService currentUserService) : base(currentUserService)
    {
        _authService = authService;
        _inventoryService = inventoryService;
    }
    
}