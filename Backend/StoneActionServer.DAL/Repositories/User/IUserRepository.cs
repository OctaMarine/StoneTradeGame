using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface IUserRepository
{
    public Task AddAsync(User user);

    public Task<User?> GetByIdAsync(int userId);

    public Task<User?> GetByUserNameAsync(string userName);

    public Task<List<User>> GetAllUsersAsync();

    public void Update(User user);

    public Task<bool> DeleteAsync(int userId);
}
