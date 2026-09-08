using StoneActionServer.BusinessLogic.Services;

namespace StoneActionServer.WebApi.Controllers;

public class PlayerController : BaseApiController
{
    public PlayerController(ICurrentUserService currentUserService) : base(currentUserService)
    {
        
    }
}