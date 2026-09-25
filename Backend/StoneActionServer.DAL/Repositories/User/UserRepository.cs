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

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public Task<User?> GetByIdAsync(int userId)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<string?> GetNameById(int userId)
    {
        var user = await GetByIdAsync(userId);
        return user?.UserName;
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Delete(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
