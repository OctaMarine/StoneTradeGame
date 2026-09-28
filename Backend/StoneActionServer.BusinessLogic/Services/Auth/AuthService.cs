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
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthService(IAuthRepository authRepository, JwtService jwtService, IUnitOfWork unitOfWork)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> RegisterAsync(string userName, string password, string email, CancellationToken cancellationToken = default)
    {
        var existUser = await _authRepository.GetByUserNameAsync(userName);
        if (existUser != null)
        {
            return false;
        }
        
        var inventory = new Inventory
        {
            Coins = 0
        };

        var user = new User
        {
            UserName = userName,
            Email = email,
            Inventory = inventory
        };
        var pasHash = new PasswordHasher<User>().HashPassword(user, password);
        user.PasswordHash = pasHash;

        await _authRepository.AddAsync(user,cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    
    public async Task<string> LoginAsync(string userName, string password)
    {
        var user = await _authRepository.GetByUserNameAsync(userName);
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

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _authRepository.GetAllUsersAsync();
    }
}