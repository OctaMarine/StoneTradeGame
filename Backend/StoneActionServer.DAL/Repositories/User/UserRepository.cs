using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task<User?> GetByIdAsync(int userId)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<string?> GetNameByIdAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        return user?.UserName;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        return true;
    }
}
