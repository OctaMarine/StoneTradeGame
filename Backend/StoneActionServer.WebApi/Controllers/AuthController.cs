using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.WebApi.DTO;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService,ICurrentUserService currentUserService) : base(currentUserService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm]string userName, [FromForm]string password, [FromForm]string email)
    {
        var result = await _authService.Register(userName,password,email);
        if (!result)
        {
            return BadRequest();
        }
        return Ok("register" +result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        if (request == null || string.IsNullOrEmpty(request.UserName))
        {
            return BadRequest("Некорректные данные");
        }

        var token = await _authService.Login(request.UserName, request.Password);
    
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Неверный логин или пароль");
        }
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            //Domain = "192.168.0.142",
            Path = "/",
            //Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("accessToken", token, cookieOptions);
        return Ok(new { message = "Login successful", tokenLength = token.Length });
    }
}