using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    
    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Add(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<User> GetByUserName(string userName)
    {
        return _context.Users.FirstOrDefault(x => x.UserName == userName);
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }
}