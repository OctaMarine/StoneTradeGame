using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface IUserRepository
{
    public Task Add(User user);

    public Task<User?> GetByIdAsync(int userId);

    public Task<User?> GetByUserName(string userName);

    public List<User> GetAllUsers();

    public Task Update(User user);

    public Task<bool> Delete(int userId);
}
