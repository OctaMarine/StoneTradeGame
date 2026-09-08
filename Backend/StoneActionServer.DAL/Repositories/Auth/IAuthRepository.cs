using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories;

public interface IAuthRepository
{
    public Task Add(User user, CancellationToken cancellationToken);

    public Task<User> GetByUserName(string userName);
    public List<User> GetAllUsers();
}