using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.WebApi.DTO;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
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
        Console.WriteLine("Login ...");
        var token = await _authService.Login(request.UserName, request.Password);
        if (token == string.Empty)
        {
            return BadRequest();
        }
        Console.WriteLine("Login complete");

        var cookieOptions = new CookieOptions
        {
                     HttpOnly = true, // Самое главное: cookie недоступна через JavaScript
                     //Secure = true,   // Отправлять cookie только по HTTPS
                     Secure = false,              // ← HTTP в разработке
                     SameSite = SameSiteMode.Lax,
                     //SameSite = SameSiteMode.Lax, // Защита от CSRF
                     // Expires = DateTime.UtcNow.AddDays(7) // Установите время жизни cookie
        };
        Console.WriteLine(token);
        Response.Cookies.Append("accessToken", token, cookieOptions);
        return Ok();
    }
}