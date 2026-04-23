using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

namespace StoneActionServer.BusinessLogic.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly JwtService _jwtService;
    private readonly AppDbContext _context;
    
    public AuthService(IAuthRepository authRepository, JwtService jwtService, AppDbContext context)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
        _context = context;
    }
    
    public async Task<bool> Register(string userName, string password, string email, CancellationToken cancellationToken = default)
    {
        var existUser = await _authRepository.GetByUserName(userName);
        if (existUser != null)
        {
            return false;
        }
        
        var inventory = new Inventory
        {
            Coins = 10
        };

        var user = new User
        {
            UserName = userName,
            Email = email,
            Inventory = inventory
        };
        var pasHash = new PasswordHasher<User>().HashPassword(user, password);
        user.PasswordHash = pasHash;

        await _authRepository.Add(user,cancellationToken);

        return true;
    }
    
    public async Task<string> Login(string userName, string password)
    {
        var user = await _authRepository.GetByUserName(userName);
        if (user == null)
        {
            return String.Empty;
        }
        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Success)
        {
            return _jwtService.GenerateToken(user);
        }
        else
        {
            Console.WriteLine("Cannot authorize");
        }
        return String.Empty;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return _authRepository.GetAllUsers();
    }
}