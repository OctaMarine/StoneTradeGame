using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface IAuthRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken);

    public Task<User?> GetByUserNameAsync(string userName);
    public Task<List<User>> GetAllUsersAsync();
}