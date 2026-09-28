using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    
    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user,cancellationToken);
    }
    
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }
}