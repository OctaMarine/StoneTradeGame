using Microsoft.AspNetCore.Mvc;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.WebApi.DTO;

namespace StoneActionServer.WebApi.Controllers;

[ApiController]
[Route("api/v1")]
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
        Console.WriteLine("🔍 [AUTH] Получен запрос на логин");
        Console.WriteLine("🔍 [AUTH] Origin: " + Request.Headers["Origin"].FirstOrDefault());
        Console.WriteLine("🔍 [AUTH] Host: " + Request.Headers["Host"].FirstOrDefault());
        Console.WriteLine("🔍 [AUTH] Cookie передана: " + (Request.Cookies.ContainsKey("accessToken") ? "ДА" : "НЕТ"));

        if (request == null || string.IsNullOrEmpty(request.UserName))
        {
            Console.WriteLine("🔍 [AUTH] ОШИБКА: Тело запроса пустое или неверный формат");
            return BadRequest("Некорректные данные");
        }

        var token = await _authService.Login(request.UserName, request.Password);
    
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("🔍 [AUTH] ОШИБКА: _authService вернул пустой токен (неверный логин/пароль)");
            return BadRequest("Неверный логин или пароль");
        }

        Console.WriteLine("🔍 [AUTH] Токен сгенерирован (длина: " + token.Length + ")");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            //Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("accessToken", token, cookieOptions);
        Console.WriteLine("🔍 [AUTH] Cookie accessToken установлена. Настройки: HttpOnly=true, Secure=false, SameSite=Lax");

        return Ok(new { message = "Login successful", tokenLength = token.Length });
    }
}